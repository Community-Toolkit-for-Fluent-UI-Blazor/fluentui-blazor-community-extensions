using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload data and layout information for a barcode, including its logical dimensions, encoded data,
/// quiet zone, and optional label.
/// </summary>
/// <remarks>This class encapsulates all information required to render or process a barcode, including its
/// logical size, the encoded data, and optional visual elements such as a quiet zone or label. The generic parameter
/// allows flexibility for different barcode data representations.</remarks>
/// <typeparam name="T">The type of the data encoded in the barcode. This can represent either one-dimensional or two-dimensional barcode
/// data, depending on the barcode format.</typeparam>
public sealed class BarcodePayload<T> : ILayerPayload
{
    /// <summary>
    /// Gets the width of the logical frame.
    /// </summary>
    public double Width { get; internal set; }

    /// <summary>
    /// Gets the height of the logical frame.
    /// </summary>
    public double Height { get; internal set; }

    /// <summary>
    /// Gets the barcode data.
    /// </summary>
    [DisallowNull]
    public T Data { get; init; } = default!;

    /// <summary>
    /// Gets the quiet zone configuration for the barcode, which defines the minimum required blank space around the
    /// barcode symbol.
    /// </summary>
    /// <remarks>The quiet zone is an area of blank margin surrounding a barcode that is necessary for proper
    /// scanning. Adjusting this property allows customization of the quiet zone size and behavior according to barcode
    /// standards or specific requirements.</remarks>
    public BarcodeQuietZonePayload? QuietZone { get; init; }

    /// <summary>
    /// Gets the foreground options used to configure the appearance of surface content.
    /// </summary>
    public SurfaceForegroundOptions Foreground { get; init; } = new();

    /// <summary>
    /// Gets the background options used to configure the surface appearance.
    /// </summary>
    public SurfaceBackgroundOptions Background { get; init; } = new();

    /// <summary>
    /// Gets or sets the options that configure the surface view.
    /// </summary>
    public SurfaceViewOptions View { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the appearance and behavior of barcode labels.
    /// </summary>
    public BarcodeLabelOptions LabelOptions { get; set; } = new();

    /// <summary>
    /// Gets the collection of barcode shapes associated with the current instance.
    /// </summary>
    public List<BarcodeRectangle> Shapes { get; } = [];
}

