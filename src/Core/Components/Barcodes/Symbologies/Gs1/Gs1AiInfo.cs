namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents metadata information for a GS1 Application Identifier (AI), including its code, length characteristics,
/// and content constraints.
/// </summary>
/// <remarks>This type is used to describe the structure and validation rules for a specific GS1 Application
/// Identifier, which is commonly used in barcoding and supply chain standards. It provides details such as whether the
/// AI has a fixed or variable length, the expected length or maximum length, and whether the value must consist of
/// digits only.</remarks>
internal sealed class Gs1AiInfo
{
    /// <summary>
    /// Gets the AI identifier associated with the current instance.
    /// </summary>
    public required string Ai { get; init; }

    /// <summary>
    /// Gets a value indicating whether the length of the data is fixed.
    /// </summary>
    public required bool IsFixedLength { get; init; }

    /// <summary>
    /// Gets the fixed length value for the associated entity.
    /// </summary>
    public required int Length { get; init; }

    /// <summary>
    /// Gets the maximum number of characters allowed.
    /// </summary>
    public required int MaxLength { get; init; }

    /// <summary>
    /// Gets a value indicating whether only digit characters are allowed.
    /// </summary>
    public bool DigitsOnly { get; init; } = true;
}
