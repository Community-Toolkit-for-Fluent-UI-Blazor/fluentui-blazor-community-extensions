using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Login;

public class LoginOverlayTextTests : TestBase
{
    public LoginOverlayTextTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
    }

    [Fact]
    public void LoginOverlayText_DefaultPropertyValues()
    {
        var cut = RenderComponent<LoginOverlayText>();

        // Defaults as declared in the class
        Assert.Null(cut.Instance.Text);
        Assert.Equal("white", cut.Instance.Color);
        Assert.Equal("sans-serif", cut.Instance.FontFamily);
        Assert.Equal("24px", cut.Instance.FontSize);
        Assert.Equal("400", cut.Instance.FontWeight);
        Assert.Equal("0", cut.Instance.LetterSpacing);
        Assert.Equal("64px", cut.Instance.LineHeight);
        Assert.Equal("0", cut.Instance.Margin);
    }

    [Fact]
    public void LoginOverlayText_InternalStyle_Contains_Defaults()
    {
        var cut = RenderComponent<LoginOverlayText>();

        var styleProp = cut.Instance.GetType().GetProperty("InternalStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        var internalStyle = (string?)styleProp?.GetValue(cut.Instance);

        Assert.NotNull(internalStyle);
        Assert.Contains("color: white", internalStyle);
        Assert.Contains("font-family: sans-serif", internalStyle);
        Assert.Contains("font-size: 24px", internalStyle);
        Assert.Contains("font-weight: 400", internalStyle);
        Assert.Contains("letter-spacing: 0", internalStyle);
        Assert.Contains("line-height: 64px", internalStyle);
        Assert.Contains("margin: 0", internalStyle);
    }

    [Fact]
    public void LoginOverlayText_InternalStyle_Reflects_CustomValues()
    {
        var cut = RenderComponent<LoginOverlayText>(parameters => parameters
            .Add(p => p.Color, "#123456")
            .Add(p => p.FontFamily, "serif")
            .Add(p => p.FontSize, "18px")
            .Add(p => p.FontWeight, "700")
            .Add(p => p.LetterSpacing, "1px")
            .Add(p => p.LineHeight, "20px")
            .Add(p => p.Margin, "4px")
            .Add(p => p.Style, "background: yellow")
        );

        var styleProp = cut.Instance.GetType().GetProperty("InternalStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        var internalStyle = (string?)styleProp?.GetValue(cut.Instance);

        Assert.NotNull(internalStyle);
        Assert.Contains("color: #123456", internalStyle);
        Assert.Contains("font-family: serif", internalStyle);
        Assert.Contains("font-size: 18px", internalStyle);
        Assert.Contains("font-weight: 700", internalStyle);
        Assert.Contains("letter-spacing: 1px", internalStyle);
        Assert.Contains("line-height: 20px", internalStyle);
        Assert.Contains("margin: 4px", internalStyle);
        Assert.Contains("background: yellow", internalStyle);
    }
}
