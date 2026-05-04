using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Payloads;

public class Barcode1DStackedPayloadTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var payload = new Barcode1DStackedPayload();

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
            new(1, 2, 3, 4)
        };
        var texts = new List<BarcodeText>
        {
            new() { X = 7, Y = 8, Text = "STACK" }
        };

        var payload = new Barcode1DStackedPayload
        {
            Value = "DATA",
            Rows = 2,
            Columns = 3,
            Modules = modules,
            Texts = texts
        };

        Assert.Equal("DATA", payload.Value);
        Assert.Equal(2, payload.Rows);
        Assert.Equal(3, payload.Columns);
        Assert.Same(modules, payload.Modules);
        Assert.Same(texts, payload.Texts);
    }
}
