namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload of a stacked 1D barcode, including its data, structure, and associated text elements.
/// </summary>
/// <remarks>This type encapsulates the logical structure of a stacked 1D barcode, such as those used in certain
/// postal or inventory applications. It provides access to the original encoded value, the arrangement of rows and
/// columns, the run-length encoded line data, and any optional human-readable text elements. Instances of this type are
/// typically produced by barcode decoding operations and are immutable.</remarks>
internal sealed class Barcode1DStackedPayload
{
    /// <summary>
    /// Gets or sets the original data encoded in the stacked barcode.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Gets or sets the number of logical rows (stacked 1D lines).
    /// </summary>
    public int Rows { get; init; }

    /// <summary>
    /// Gets or sets the number of modules per row.
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
    /// Optional text elements (e.g., human-readable labels).
    /// </summary>
    public IReadOnlyList<BarcodeText> Texts { get; init; } = [];
}

