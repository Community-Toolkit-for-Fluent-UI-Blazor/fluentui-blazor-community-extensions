namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the visual theme settings for a signature grid, including colors, opacity, grid display mode, and stroke
/// properties.
/// </summary>
/// <remarks>Use this class to configure the appearance of signature components, such as background color, grid
/// style, and drawing parameters. These settings control how the signature grid is rendered and can be customized to
/// match application branding or user preferences.</remarks>
public class SignatureThemeDefinition
{
    /// <summary>
    /// Gets or sets the background theme used for the signature component.
    /// </summary>
    public SignatureBackgroundTheme Background { get; set; } = new();

    /// <summary>
    /// Gets or sets the grid theme used for the signature component.
    /// </summary>
    public SignatureGridTheme Grid { get; set; } = new();

    /// <summary>
    /// Gets or sets the axes theme used for the signature component.
    /// </summary>
    public SignatureAxesTheme Axes { get; set; } = new();

    /// <summary>
    /// Gets or sets the watermark theme used for the signature component.
    /// </summary>
    public SignatureWatermarkTheme Watermark { get; set; } = new();
}
