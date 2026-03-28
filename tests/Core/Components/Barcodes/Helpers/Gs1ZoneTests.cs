using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Helpers;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers;

public class Gs1ZoneTests
{
    [Fact]
    public void AddDigitZone_AddsCenteredDigits()
    {
        var texts = new List<BarcodeText>();

        Gs1Zone.AddDigitZone(texts, "1234", 0, 4, 0, 4, 5);

        Assert.Equal(4, texts.Count);
        Assert.Equal(0.5, texts[0].X);
        Assert.Equal(3.5, texts[3].X);
        Assert.All(texts, t => Assert.Equal(SvgTextAnchor.Middle, t.Anchor));
    }

    [Fact]
    public void AddOuterDigits_AddsAnchoredText()
    {
        var texts = new List<BarcodeText>();

        Gs1Zone.AddOuterLeftDigit(texts, '1', 0, 1, 5);
        Gs1Zone.AddOuterRightDigit(texts, '9', 4, 1, 5);

        Assert.Equal(-0.5, texts[0].X);
        Assert.Equal(SvgTextAnchor.End, texts[0].Anchor);
        Assert.Equal(4.5, texts[1].X);
        Assert.Equal(SvgTextAnchor.Start, texts[1].Anchor);
    }
}
