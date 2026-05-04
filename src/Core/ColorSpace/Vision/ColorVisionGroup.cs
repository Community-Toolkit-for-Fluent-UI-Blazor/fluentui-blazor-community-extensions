namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents the different types of color vision deficiencies that can affect how colors are perceived.
/// </summary>
internal enum ColorVisionGroup
{
    /// <summary>
    /// No color vision deficiency; normal color perception.
    /// </summary>
    Normal,

    /// <summary>
    /// Color vision deficiency due to protanopia or protanomaly.
    /// </summary>
    Protan,

    /// <summary>
    /// Color vision deficiency due to deuteranopia or deuteranomaly.
    /// </summary>
    Deutan,

    /// <summary>
    /// Color vision deficiency due to tritanopia or tritanomaly.
    /// </summary>
    Tritan,

    /// <summary>
    /// Color vision deficiency due to achromatopsia or achromatomaly.
    /// </summary>
    Achroma
}
