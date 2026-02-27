namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for displaying debug text in a signature component, including font family, font
/// size, and color.
/// </summary>
/// <remarks>This class is typically used to customize the appearance of debug text for diagnostic or development
/// purposes. All properties are immutable and must be set at initialization.</remarks>
public sealed class SignatureDebugTextOptions
{
    /// <summary>
    /// Gets the font family used for rendering text in the component.
    /// </summary>
    public string FontFamily { get; init; } = "monospace";

    /// <summary>
    /// Gets the font size used to render text content.
    /// </summary>
    public double FontSize { get; init; } = 14;

    /// <summary>
    /// Gets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; init; } = "#FF0000";
}

