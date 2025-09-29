using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureEraserPanelTests
    : TestBase
{
    public SignatureEraserPanelTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddFluentCxUIComponents();
    }

    [Fact]
    public void Renders_All_Controls_With_Correct_Labels()
    {
        var options = new SignatureEraserOptions
        {
            Shape = EraserShape.Circle,
            Size = 10,
            Opacity = 0.5,
            SoftEdges = true,
            PartialErase = false
        };

        // Act
        var cut = RenderComponent<SignatureEraserPanel>(parameters =>
            parameters.Add(p => p.Content, (SignatureLabels.Default, options))
        );

        // Assert
        cut.Markup.Contains("Shape");
        cut.Markup.Contains("Circle");
        cut.Markup.Contains("Square");
        cut.Markup.Contains("Eraser Size : 10px");
        cut.Markup.Contains("Opacity : 0,5");
        cut.Markup.Contains("Eraser Soft Edges");
        cut.Markup.Contains("Partial Erase");
    }
}
