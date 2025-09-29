using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureGridPanelTests : TestBase
{
    public SignatureGridPanelTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_Correctly()
    {
        var options = new SignatureGridOptions
        {
            DisplayMode = GridDisplayMode.Lines,
            BackgroundColor = "#FFFFFF",
            Color = "#000000",
            CellSize = 20,
            Opacity = 0.5,
            BoldEvery = 10,
            StrokeWidth = 2,
            Margin = 3,
            PointRadius = 2,
            ShowAxes = true
        };

        // Act
        var cut = RenderComponent<SignatureGridPanel>(parameters =>
            parameters.Add(p => p.Content, (SignatureLabels.Default, options))
        );

        // Assert
        cut.Markup.Contains("Display Mode");
        cut.Markup.Contains("Background Color");
        cut.Markup.Contains("Show Axes");
        Assert.NotNull(cut.Find("fluent-slider"));
    }
}
