using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class ManageCookieTests : TestBase
{
    public ManageCookieTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    private static CookieData GetSampleCookieData()
    {
        return new CookieData(
            [
                new() { Name = "A", IsActive = true },
                new() { Name = "B", IsActive = false },
                new() { Name = FluentCxCookie.GoogleAnalytics, IsActive = true }
            ], CookieLabels.Default);
    }

    [Fact]
    public void ButtonsDisabled_ReturnsTrue_IfAnyItemIsActiveNull()
    {
        // Arrange
        var data = GetSampleCookieData();
        data.Items.ElementAt(1).IsActive = null;

        var cut = RenderComponent<FluentDialog>(
            parameters => parameters.AddChildContent<ManageCookie>(p => p.Add(m => m.Content, data))
                                    .Add(p => p.Instance, new DialogInstance(typeof(ManageCookie),
                                    new DialogParameters(), data))
        );

        // Act
        var result = cut.FindComponent<FluentButton>();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Instance.Disabled);
    }

    [Fact]
    public void ButtonsDisabled_ReturnsFalse_IfAllItemsIsActiveSet()
    {
        var data = GetSampleCookieData();

        var cut = RenderComponent<FluentDialog>(
            parameters => parameters.AddChildContent<ManageCookie>(p => p.Add(m => m.Content, data))
                                    .Add(p => p.Instance, new DialogInstance(typeof(ManageCookie),
                                    new DialogParameters(), data))
        );

        // Act
        var result = cut.FindComponent<FluentButton>();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Instance.Disabled);
    }

    [Fact]
    public void OnCancelAsync_SetsIsActiveNull_ExceptGoogleAnalytics_AndCallsDialogCancel()
    {
        // Arrange
        var data = GetSampleCookieData();

        var cut = RenderComponent<FluentDialog>(
            parameters => parameters.AddChildContent<ManageCookie>(p => p.Add(m => m.Content, data))
                                    .Add(p => p.Instance, new DialogInstance(typeof(ManageCookie),
                                    new DialogParameters()
                                    {
                                        PrimaryAction = "Save Changes",
                                        SecondaryAction = "Cancel"
                                    }, data))
        );

        // Act
        // Find cancel button and click it
        var buttons = cut.FindAll("fluent-button");
        var cancelButton = buttons.FirstOrDefault(b => b.InnerHtml.Contains("Cancel"));

        // Assert
        Assert.NotNull(cancelButton);
        cancelButton.Click();
        Assert.Null(data.Items.ElementAt(0).IsActive);
        Assert.Null(data.Items.ElementAt(1).IsActive);
        Assert.True(data.Items.ElementAt(2).IsActive);
    }

    [Fact]
    public void OnSaveAsync_CallsDialogClose_WithItems()
    {
        // Arrange
        var data = GetSampleCookieData();
        data.Items.ElementAt(0).IsActive = false;
        data.Items.ElementAt(1).IsActive = true;

        var cut = RenderComponent<FluentDialog>(
            parameters => parameters.AddChildContent<ManageCookie>(p => p.Add(m => m.Content, data))
                                    .Add(p => p.Instance, new DialogInstance(typeof(ManageCookie),
                                    new DialogParameters()
                                    {
                                        PrimaryAction = "Save Changes",
                                        SecondaryAction = "Cancel"
                                    }, data))
        );

        // Act
        // Find cancel button and click it
        var buttons = cut.FindAll("fluent-button");
        var saveButton = buttons.FirstOrDefault(b => b.InnerHtml.Contains("Save Changes"));

        // Assert
        Assert.NotNull(saveButton);
        saveButton.Click();

        var verifyData = cut.Instance.Instance.Content as CookieData;
        Assert.Equal(verifyData!.Items.ElementAt(0), data.Items.ElementAt(0));
        Assert.Equal(verifyData!.Items.ElementAt(1), data.Items.ElementAt(1));
        Assert.Equal(verifyData!.Items.ElementAt(2), data.Items.ElementAt(2));
    }
}
