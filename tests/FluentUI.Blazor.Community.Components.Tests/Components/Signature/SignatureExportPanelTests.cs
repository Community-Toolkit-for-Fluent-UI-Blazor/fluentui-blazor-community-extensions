using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureExportPanelTests : TestBase
{
    public SignatureExportPanelTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_Select_And_Slider()
    {
        // Arrange
        var options = new SignatureExportOptions
        {
            Format = SignatureImageFormat.Png,
            Quality = 80
        };

        // Act
        var cut = RenderComponent<SignatureExportPanel>(parameters =>
            parameters.Add(p => p.Content, (SignatureLabels.Default, options))
        );

        // Assert
        cut.Markup.Contains("Format");
        cut.Markup.Contains("Quality");
        cut.Find("fluent-select");
        cut.Find("fluent-slider");
    }
}
