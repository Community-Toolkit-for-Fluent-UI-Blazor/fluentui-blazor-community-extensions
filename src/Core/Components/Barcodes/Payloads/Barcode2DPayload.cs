namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload data for a two-dimensional barcode, including its dimensions and module layout.
/// </summary>
/// <remarks>This class encapsulates the structural information required to render or process a 2D barcode, such
/// as a QR code or Data Matrix. It provides the number of rows and columns, as well as the collection of individual
/// modules that define the barcode's pattern.</remarks>
public sealed class Barcode2DPayload
{
    /// <summary>
    /// Gets the value which represents the original data encoded in the 2D barcode.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Gets the number of rows to display.
    /// </summary>
    public int Rows { get; init; }

    /// <summary>
    /// Gets the number of columns to display.
    /// </summary>
    public int Columns { get; init; }

    /// <summary>
    /// Gets the collection of barcode modules that define the structure of the barcode image.
    /// </summary>
    /// <remarks>Each module represents a rectangular area within the barcode, typically corresponding to a
    /// single bar or space. The collection is read-only and reflects the current state of the barcode's
    /// layout.</remarks>
    public IReadOnlyList<BarcodeRectangle> Modules { get; init; } = [];

    /// <summary>
    /// Gets the collection of recognized text elements within the barcode.
    /// </summary>
    /// <remarks>Each item in the collection represents a distinct text segment detected in the barcode. The
    /// collection is read-only and is initialized to an empty list if no text is recognized.</remarks>
    public IReadOnlyList<BarcodeText> Texts { get; init; } = [];
}
