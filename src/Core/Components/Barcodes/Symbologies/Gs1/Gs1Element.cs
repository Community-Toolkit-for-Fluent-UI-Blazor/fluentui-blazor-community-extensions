namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single GS1 element, consisting of an application identifier (AI) and its associated value.
/// </summary>
/// <remarks>A GS1 element is a component of a GS1 barcode or data string, where each element is defined by a
/// specific application identifier (AI) and a corresponding value. The IsVariableLength property indicates whether the
/// value for this element can vary in length according to GS1 standards.</remarks>
internal sealed class Gs1Element
{
    /// <summary>
    /// Gets the AI identifier associated with the current instance.
    /// </summary>
    public required string Ai { get; init; }

    /// <summary>
    /// Gets the value represented by this instance.
    /// </summary>
    public required string Value { get; init; }

    /// <summary>
    /// Gets a value indicating whether the length of the data is variable.
    /// </summary>
    public bool IsVariableLength { get; init; }
}

