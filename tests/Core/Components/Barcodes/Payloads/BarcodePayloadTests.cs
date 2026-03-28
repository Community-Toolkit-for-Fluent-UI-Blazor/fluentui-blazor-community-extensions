using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.Barcodes.Payloads;

public class BarcodePayloadTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var payload = new BarcodePayload<Barcode1DPayload>();

        Assert.Equal(0, payload.Width);
        Assert.Equal(0, payload.Height);
        Assert.NotNull(payload.Foreground);
        Assert.NotNull(payload.Background);
        Assert.NotNull(payload.View);
        Assert.NotNull(payload.LabelOptions);
        Assert.Empty(payload.Shapes);
    }

    [Fact]
    public void InitProperties_PreservesAssignedValues()
    {
        var quietZone = new BarcodeQuietZonePayload { X = 1, Y = 2, Width = 3, Height = 4 };
        var foreground = new SurfaceForegroundOptions { Color = "#123" };
        var background = new SurfaceBackgroundOptions { Color = "#abc" };
        var view = new SurfaceViewOptions { RenderLeft = 5, RenderTop = 6 };
        var label = new BarcodeLabelOptions { FontSize = 14 };
        var data = new Barcode1DPayload();

        var payload = new BarcodePayload<Barcode1DPayload>
        {
            Width = 10,
            Height = 11,
            Data = data,
            QuietZone = quietZone,
            Foreground = foreground,
            Background = background,
            View = view,
            LabelOptions = label
        };
        payload.Shapes.Add(new BarcodeRectangle(0, 0, 1, 1));

        Assert.Equal(10, payload.Width);
        Assert.Equal(11, payload.Height);
        Assert.Same(data, payload.Data);
        Assert.Same(quietZone, payload.QuietZone);
        Assert.Same(foreground, payload.Foreground);
        Assert.Same(background, payload.Background);
        Assert.Same(view, payload.View);
        Assert.Same(label, payload.LabelOptions);
        Assert.Single(payload.Shapes);
    }
}
