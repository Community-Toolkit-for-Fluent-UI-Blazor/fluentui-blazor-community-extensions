using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using Microsoft.FluentUI.AspNetCore.Components.Tests;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignaturePenPreviewerTests : TestBase
{
    [Fact]
    public void RendersNothing_WhenOptionsIsNull()
    {
        // Act
        var cut = RenderComponent<SignaturePenPreviewer>(parameters => parameters
            .Add(p => p.Options, null)
        );

        // Assert
        cut.MarkupMatches(string.Empty);
    }

    [Fact]
    public void RendersLineWithoutShadow_WhenShadowIsNull()
    {
        var options = new SignaturePenOptions
        {
            Color = "red",
            BaseWidth = 2,
            Shadow = null
        };

        var cut = RenderComponent<SignaturePenPreviewer>(parameters => parameters
            .Add(p => p.Options, options)
        );

        cut.Find("line").MarkupMatches(
            @"<line x1=""20"" y1=""50"" x2=""180"" y2=""50"" stroke=""red"" stroke-width=""2""></line>"
        );
    }

    [Fact]
    public void RendersLineWithShadow_WhenShadowIsSet()
    {
        var options = new SignaturePenOptions
        {
            Color = "blue",
            BaseWidth = 3,
            Shadow = new ShadowOptions
            {
                Color = "black",
                OffsetX = 1,
                OffsetY = 2,
                Blur = 4
            }
        };

        var cut = RenderComponent<SignaturePenPreviewer>(parameters => parameters
            .Add(p => p.Options, options)
        );

        // Vérifie la présence du filtre et de l'attribut filter sur la ligne
        cut.Markup.Contains("filter=\"url(#shadow)\"");
        cut.Markup.Contains("flood-color=\"black\"");
        cut.Markup.Contains("stroke=\"blue\"");
        cut.Markup.Contains("stroke-width=\"3\"");
    }
}
