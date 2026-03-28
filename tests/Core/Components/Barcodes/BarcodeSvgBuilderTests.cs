using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace Components.Tests.Components.Barcodes;

public class BarcodeSvgBuilderTests
{
    [Fact]
    public void Barcode2DSvgBuilder_Build_RendersBackgroundShapesAndText()
    {
        var payload = new BarcodePayload<Barcode2DPayload>
        {
            Width = 100,
            Height = 80,
            Background = new SurfaceBackgroundOptions { Color = "#ffffff" },
            Foreground = new SurfaceForegroundOptions { Color = "#111111" },
            QuietZone = new BarcodeQuietZonePayload { X = 2, Y = 3, Width = 4, Height = 5 },
            LabelOptions = new BarcodeLabelOptions
            {
                FontFamily = "Segoe",
                FontSize = 14,
                Color = "#123456"
            },
            Data = new Barcode2DPayload
            {
                Texts =
                [
                    new BarcodeText { X = 10, Y = 11, Text = "ABC", Anchor = SvgTextAnchor.End }
                ]
            }
        };

        payload.Shapes.Add(new BarcodeRectangle(5, 6, 7, 8));

        var builder = new SvgBuilder();

        Barcode2DSvgBuilder.Build(builder, payload);

        var svg = builder.Build();

        Assert.Contains("<rect x=\"0\" y=\"0\" width=\"100\" height=\"80\" fill=\"#ffffff\"", svg);
        Assert.Contains("<rect x=\"7\" y=\"9\" width=\"7\" height=\"8\" fill=\"#111111\"", svg);
        Assert.Contains("<text x=\"12\" y=\"14\" font-family=\"Segoe\" font-size=\"14\" fill=\"#123456\" text-anchor=\"end\" dominant-baseline=\"alphabetic\">ABC</text>", svg);
    }

    [Fact]
    public void Barcode1DStackedSvgBuilder_Build_RendersBackgroundShapesAndText()
    {
        var payload = new BarcodePayload<Barcode1DStackedPayload>
        {
            Width = 120,
            Height = 60,
            Background = new SurfaceBackgroundOptions { Color = "#000000" },
            Foreground = new SurfaceForegroundOptions { Color = "#ff00ff" },
            QuietZone = new BarcodeQuietZonePayload { X = 4, Y = 5, Width = 0, Height = 0 },
            LabelOptions = new BarcodeLabelOptions
            {
                FontFamily = "Arial",
                FontSize = 10,
                Color = "#0f0f0f"
            },
            Data = new Barcode1DStackedPayload
            {
                Texts =
                [
                    new BarcodeText { X = 2, Y = 3, Text = "STACK", Anchor = SvgTextAnchor.Middle }
                ]
            }
        };

        payload.Shapes.Add(new BarcodeRectangle(1, 2, 3, 4));

        var builder = new SvgBuilder();

        Barcode1DStackedSvgBuilder.Build(builder, payload);

        var svg = builder.Build();

        Assert.Contains("<rect x=\"0\" y=\"0\" width=\"120\" height=\"60\" fill=\"#000000\"", svg);
        Assert.Contains("<rect x=\"5\" y=\"7\" width=\"3\" height=\"4\" fill=\"#ff00ff\"", svg);
        Assert.Contains("<text x=\"6\" y=\"8\" font-family=\"Arial\" font-size=\"10\" fill=\"#0f0f0f\" text-anchor=\"middle\" dominant-baseline=\"alphabetic\">STACK</text>", svg);
    }

    [Fact]
    public void Barcode1DSvgBuilder_Build_RendersBarsBackgroundShapesAndText()
    {
        var payload = new BarcodePayload<Barcode1DPayload>
        {
            Width = 200,
            Height = 100,
            Background = new SurfaceBackgroundOptions { Color = "#eeeeee" },
            Foreground = new SurfaceForegroundOptions { Color = "#123456" },
            QuietZone = new BarcodeQuietZonePayload { X = 2, Y = 4, Width = 20, Height = 0 },
            LabelOptions = new BarcodeLabelOptions
            {
                FontFamily = "Tahoma",
                FontSize = 9,
                Color = "#abcdef"
            },
            Data = new Barcode1DPayload
            {
                Bars =
                [
                    new Barcode1DBar { X = 1, Y = 2, Width = 3, Height = 15 }
                ],
                Texts =
                [
                    new BarcodeText { X = 4, Y = 6, Text = "CODE", Anchor = SvgTextAnchor.Middle }
                ]
            }
        };

        payload.Shapes.Add(new BarcodeRectangle(8, 9, 10, 11));

        var builder = new SvgBuilder();

        Barcode1DSvgBuilder.Build(builder, payload);

        var svg = builder.Build();

        Assert.Contains("<rect x=\"2\" y=\"4\" width=\"180\" height=\"15\" fill=\"#eeeeee\"", svg);
        Assert.Contains("<rect x=\"3\" y=\"6\" width=\"3\" height=\"15\" fill=\"#123456\"", svg);
        Assert.Contains("<rect x=\"10\" y=\"13\" width=\"10\" height=\"11\" fill=\"#123456\"", svg);
        Assert.Contains("<text x=\"6\" y=\"10\" font-family=\"Tahoma\" font-size=\"9\" fill=\"#abcdef\" text-anchor=\"middle\" dominant-baseline=\"alphabetic\">CODE</text>", svg);
    }
}
