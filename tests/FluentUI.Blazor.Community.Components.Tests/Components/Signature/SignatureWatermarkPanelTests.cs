using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureWatermarkPanelTests
    : TestBase
{
    public SignatureWatermarkPanelTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_All_Fields_With_Initial_Values()
    {
        var options = new SignatureWatermarkOptions
        {
            Text = "Test",
            Color = "#FF0000",
            FontSize = 12,
            Opacity = 0.5,
            Rotation = 45,
            LetterSpacing = 2,
            Repeat = true
        };

        // Act
        var cut = RenderComponent<SignatureWatermarkPanel>(parameters =>
            parameters.Add(p => p.Content, (SignatureLabels.Default, options))
        );

        // Assert
        cut.Markup.Contains("Test");
        cut.Markup.Contains("#FF0000");
        Assert.Contains("Font Size (12px)", cut.Markup);
        Assert.Contains("Opacity (50,00 %)", cut.Markup);
        Assert.Contains("Rotation (45°)", cut.Markup);
        Assert.Contains("Letter Spacing (2px)", cut.Markup);
        Assert.Contains("Repeat", cut.Markup);
    }
}
