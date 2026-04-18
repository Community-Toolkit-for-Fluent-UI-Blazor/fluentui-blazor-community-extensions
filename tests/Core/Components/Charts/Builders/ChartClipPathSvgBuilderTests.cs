using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Charts.Builders;
using FluentUI.Blazor.Community.Components.Charts.Payloads;
using Xunit;

namespace Components.Tests.Components.Charts.Builders;

public class ChartClipPathSvgBuilderTests
{
    [Fact]
    public void ChartClipPathSvgBuilder_Build_RendersClipPath()
    {
        var svg = new SvgBuilder();
        var payload = new ClipPathPayload
        {
            X = 1,
            Y = 2,
            Width = 30,
            Height = 40
        };

        ChartClipPathSvgBuilder.Build(svg, payload);

        var markup = svg.Build();

        Assert.Contains("clipPath", markup, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("width=\"30\"", markup);
    }

    [Fact]
    public void ChartClipPathSvgBuilder_Build_IgnoresNullPayload()
    {
        var svg = new SvgBuilder();

        ChartClipPathSvgBuilder.Build(svg, null!);

        Assert.DoesNotContain("clipPath", svg.Build(), StringComparison.OrdinalIgnoreCase);
    }
}
