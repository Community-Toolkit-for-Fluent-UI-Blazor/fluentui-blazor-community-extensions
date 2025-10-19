using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginOverlayDescriptionTests : TestBase
{
    public LoginOverlayDescriptionTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void LoginOverlayDescription_DefaultInstance_Renders()
    {
        var cut = RenderComponent<LoginOverlayDescription>();
        Assert.NotNull(cut.Instance);
    }

    [Fact]
    public void LoginOverlayDescription_Css_Includes_BaseClass_And_CustomClass()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginOverlayDescription>(parameters => parameters
            .Add(p => p.Class, "my-custom")
        );

        // Access private property 'Css'
        var cssProp = cut.Instance.GetType().GetProperty("Css", BindingFlags.NonPublic | BindingFlags.Instance);
        var css = (string?)cssProp?.GetValue(cut.Instance);

        // Assert
        Assert.NotNull(css);
        Assert.Contains("login-overlay-container", css);
        Assert.Contains("my-custom", css);
    }

    [Fact]
    public void LoginOverlayDescription_InternalStyle_Includes_MaxWidth_When_Set()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginOverlayDescription>(parameters => parameters
            .Add(p => p.MaxWidth, 512)
            .Add(p => p.Style, "color: red")
        );

        // Access private property 'InternalStyle'
        var styleProp = cut.Instance.GetType().GetProperty("InternalStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        var internalStyle = (string?)styleProp?.GetValue(cut.Instance);

        // Assert
        Assert.NotNull(internalStyle);
        Assert.Contains("max-width: 512px", internalStyle);
        Assert.Contains("color: red", internalStyle);
    }

    [Fact]
    public void LoginOverlayDescription_InternalStyle_DoesNotContain_MaxWidth_When_Null()
    {
        // Arrange & Act
        var cut = RenderComponent<LoginOverlayDescription>(parameters => parameters
            .Add(p => p.MaxWidth, null)
            .Add(p => p.Style, "padding: 2px")
        );

        var styleProp = cut.Instance.GetType().GetProperty("InternalStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        var internalStyle = (string?)styleProp?.GetValue(cut.Instance);

        // Assert
        Assert.NotNull(internalStyle);
        Assert.DoesNotContain("max-width:", internalStyle);
        Assert.Contains("padding: 2px", internalStyle);
    }
}
