using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class SleekDialViewTests
    : TestBase
{
    [Fact]
    public void SleekDialView_CanBeInstantiated()
    {
        // Arrange & Act
        var cut = RenderComponent<SleekDialView>();

        // Assert
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void SleekDialView_WithNoParent_ReturnsEmptyMarkup()
    {
        // Arrange & Act
        var cut = RenderComponent<SleekDialView>();

        // Assert
        Assert.Empty(cut.Markup);
    }

    [Fact]
    public void SleekDialView_WithLinearParent_ReturnsStackPanelMarkup()
    {
        var parent = new FluentCxSleekDial();

        // Arrange & Act
        var cut = RenderComponent<SleekDialView>(param => param
            .Add(p => p.Parent, parent));

        // Assert
        Assert.Contains("stack-", cut.Markup);
    }

    [Fact]
    public void SleekDialView_WithRadialParent_ReturnsUlMarkup()
    {
        var parent = new FluentCxSleekDial()
        {
            Mode = SleekDialMode.Radial
        };

        // Arrange & Act
        var cut = RenderComponent<SleekDialView>(param => param
            .Add(p => p.Parent, parent));

        // Assert
        Assert.Contains("<ul", cut.Markup);
    }
}
