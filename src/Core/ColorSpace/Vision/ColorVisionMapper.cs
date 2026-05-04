namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Maps <see cref="ColorVisionType"/> to <see cref="ColorVisionGroup"/> and severity values for use in vision simulation.
/// </summary>
internal static class ColorVisionMapper
{
    /// <summary>
    /// Decomposes a <see cref="ColorVisionType"/> into its corresponding <see cref="ColorVisionGroup"/> and severity level.
    /// </summary>
    /// <param name="type">The color vision type to decompose.</param>
    /// <returns>A tuple containing the color vision group and severity level.</returns>
    public static (ColorVisionGroup Group, double Severity) Decompose(ColorVisionType type)
        => type switch
        {
            ColorVisionType.Protanopia => (ColorVisionGroup.Protan, 1.0),
            ColorVisionType.Protanomaly => (ColorVisionGroup.Protan, 0.5),

            ColorVisionType.Deuteranopia => (ColorVisionGroup.Deutan, 1.0),
            ColorVisionType.Deuteranomaly => (ColorVisionGroup.Deutan, 0.5),

            ColorVisionType.Tritanopia => (ColorVisionGroup.Tritan, 1.0),
            ColorVisionType.Tritanomaly => (ColorVisionGroup.Tritan, 0.5),

            ColorVisionType.Achromatopsia => (ColorVisionGroup.Achroma, 1.0),
            ColorVisionType.Achromatomaly => (ColorVisionGroup.Achroma, 0.5),

            _ => (ColorVisionGroup.Normal, 0.0)
        };
}
