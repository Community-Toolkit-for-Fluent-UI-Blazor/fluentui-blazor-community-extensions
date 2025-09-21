using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignaturePenPanelTests : TestBase
{
    public SignaturePenPanelTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    private (SignatureLabels, SignaturePenOptions) GetDefaultContent()
    {
        return (
            SignatureLabels.Default,
            new SignaturePenOptions
            {
                Color = "#000000",
                Opacity = 1.0,
                BaseWidth = 5,
                PressureEnabled = false,
                Smoothing = false,
                Shadow = new ShadowOptions
                {
                    Enabled = false,
                    Color = "#000000"
                }
            }
        );
    }

    [Fact]
    public void Renders_All_Labels_And_Fields()
    {
        // Arrange
        var content = GetDefaultContent();

        // Act
        var cut = RenderComponent<SignaturePenPanel>(parameters => parameters
            .Add(p => p.Content, content)
        );

        // Assert
        cut.Markup.Contains(content.Item1.PreviewPenLabel);
        cut.Markup.Contains(content.Item1.PenColorLabel);
        cut.Markup.Contains(content.Item1.PenOpacityLabel.Replace("{0}", content.Item2.Opacity.ToString("P", CultureInfo.CurrentCulture)));
        cut.Markup.Contains(content.Item1.StrokeWidthLabel.Replace("{0}", content.Item2.BaseWidth.ToString("P", CultureInfo.CurrentCulture)));
        cut.Markup.Contains(content.Item1.PressureEnabledLabel);
        cut.Markup.Contains(content.Item1.SmoothingLabel);
        cut.Markup.Contains(content.Item1.ShadowEnabledLabel);
        cut.Markup.Contains(content.Item1.ShadowColorLabel);
    }

}
