using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Payloads;

public class Barcode2DPayloadTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var payload = new Barcode2DPayload();

        Assert.Null(payload.Value);
        Assert.Equal(0, payload.Rows);
        Assert.Equal(0, payload.Columns);
        Assert.Empty(payload.Modules);
        Assert.Empty(payload.Texts);
    }

    [Fact]
    public void InitProperties_PreservesAssignedValues()
    {
        var modules = new List<BarcodeRectangle>
        {
            new(0, 0, 1, 1)
        };
        var texts = new List<BarcodeText>
        {
            new() { X = 2, Y = 3, Text = "QR" }
        };

        var payload = new Barcode2DPayload
        {
            Value = "VALUE",
            Rows = 4,
            Columns = 5,
            Modules = modules,
            Texts = texts
        };

        Assert.Equal("VALUE", payload.Value);
        Assert.Equal(4, payload.Rows);
        Assert.Equal(5, payload.Columns);
        Assert.Same(modules, payload.Modules);
        Assert.Same(texts, payload.Texts);
    }
}
