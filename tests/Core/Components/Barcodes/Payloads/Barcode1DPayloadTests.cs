using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Payloads;

public class Barcode1DPayloadTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var payload = new Barcode1DPayload();

        Assert.Empty(payload.Bars);
        Assert.Empty(payload.Texts);
        Assert.Equal(string.Empty, payload.Value);
    }

    [Fact]
    public void InitProperties_PreservesAssignedValues()
    {
        var bars = new List<Barcode1DBar>
        {
            new() { X = 1, Y = 2, Width = 3, Height = 4 }
        };
        var texts = new List<BarcodeText>
        {
            new() { X = 5, Y = 6, Text = "ABC" }
        };

        var payload = new Barcode1DPayload
        {
            Bars = bars,
            Texts = texts,
            Value = "123"
        };

        Assert.Same(bars, payload.Bars);
        Assert.Same(texts, payload.Texts);
        Assert.Equal("123", payload.Value);
    }
}
