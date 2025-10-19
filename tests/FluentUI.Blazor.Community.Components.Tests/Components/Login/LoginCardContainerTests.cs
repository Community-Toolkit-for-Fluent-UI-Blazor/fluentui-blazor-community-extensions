using Bunit;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Xunit;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginCardContainerTests : TestBase
{
    public LoginCardContainerTests()
    {
        // Configure JSInterop and required services similarly to other tests in the suite
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_Title_Body_And_Footer_When_Provided()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginCardContainer>(parameters => parameters
            .Add(p => p.Title, "My Title")
            .Add(p => p.Body, builder => builder.AddContent(0, "Body content"))
            .Add(p => p.Footer, builder => builder.AddContent(0, "Footer"))
        );

        // Assert
        Assert.NotNull(cut.Instance);
        Assert.Contains("My Title", cut.Markup);
        Assert.Contains("Body content", cut.Markup);
        Assert.Contains("Footer", cut.Markup);
    }

    [Fact]
    public void TitleTemplate_Is_Rendered_When_Provided()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginCardContainer>(parameters => parameters
            .Add(p => p.Title, "Default Title")
            .Add(p => p.TitleTemplate, builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "custom-title");
                builder.AddContent(2, "Custom title");
                builder.CloseElement();
            })
        );

        // Assert
        Assert.NotNull(cut.Instance);
        Assert.Contains("Custom title", cut.Markup);
    }

    [Fact]
    public void TitleIcon_Can_Be_Set_And_Instance_Receives_It()
    {
        // Arrange
        var icon = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size20.Person();

        // Act
        var cut = RenderComponent<LoginCardContainer>(parameters => parameters
            .Add(p => p.TitleIcon, icon)
            .Add(p => p.Title, "With an icon")
        );

        Assert.NotNull(cut.Instance);
        Assert.Same(icon, cut.Instance.TitleIcon);
        Assert.Contains("With an icon", cut.Markup);
    }

    [Fact]
    public void Renders_With_No_Parameters()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginCardContainer>();

        // Assert
        Assert.NotNull(cut.Instance);
    }
}
