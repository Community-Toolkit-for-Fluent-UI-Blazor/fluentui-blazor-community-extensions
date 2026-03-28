namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload for a 1D barcode, containing the collection of bars
///  that define the structure of the barcode.
/// </summary>
public sealed class Barcode1DPayload
{
    /// <summary>
    /// Gets the collection of bars that make up the 1D barcode.
    /// </summary>
    /// <remarks>Each bar in the collection represents a segment of the barcode, including its position and
    /// width. The collection is read-only and reflects the structure of the barcode as generated or parsed.</remarks>
    public IReadOnlyList<Barcode1DBar> Bars { get; init; } = [];

    /// <summary>
    /// Gets the collection of text elements associated with the barcode.
    /// </summary>
    public IReadOnlyList<BarcodeText> Texts { get; init; } = [];

    /// <summary>
    /// Gets the value associated with this property.
    /// </summary>
    public string Value { get; init; } = string.Empty;
}
