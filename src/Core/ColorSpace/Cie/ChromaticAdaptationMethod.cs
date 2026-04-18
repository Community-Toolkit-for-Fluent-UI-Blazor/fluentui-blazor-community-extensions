namespace FluentUI.Blazor.Community.Components.ColorSpace.Cie;

/// <summary>
/// Represents the method of chromatic adaptation to be applied when converting between two color spaces with different white points.
/// </summary>
public enum ChromaticAdaptationMethod
{
    /// <summary>
    /// Represents a scaling of the illuminants
    /// </summary>
    /// <remarks>
    /// If the first illuminant is D50 (XYZ: 0.96422, 1.00000, 0.82521) and the second 
    /// illuminant is D65 (XYZ: 0.95047, 1.00000, 1.08883), then the 
    /// XYZ method is (X: 0.96422 / 0.95047, Y: 1, Z: 0.82521 / 1.08883)
    /// </remarks>
    XYZScaling,

    /// <summary>
    /// Represents a method of chromatic adaptation using the Von Kries transform.
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Chromatic_adaptation#Von_Kries_transform
    /// </remarks>
    VonKries,

    /// <summary>
    /// Represents a method of chromatic adaptation using the Bradford transform.
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Chromatic_adaptation#Bradford_transform
    /// </remarks>
    Bradford
}
