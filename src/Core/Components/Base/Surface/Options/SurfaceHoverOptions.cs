namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for rendering hover effects on signature elements.
/// </summary>
/// <remarks>Use this class to customize the appearance and behavior of signature hover rendering, such as
/// enabling the effect, specifying the color, and setting the width. These options allow developers to tailor the
/// visual feedback when users interact with signature components.</remarks>
public sealed class SignatureHoverRenderingOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the component is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the color used for rendering the hover effect.
    /// </summary>
    public string Color { get; set; } = "rgba(0, 150, 255, 0.8)";

    /// <summary>
    /// Gets or sets the width value for the hover.
    /// </summary>
    public double Width { get; set; } = 3.0;

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component. A value of 1.0 represents
    /// full opacity, while 0.0 represents complete transparency. Typical values range between 0.0 and 1.0.</remarks>
    public double Opacity { get; set; } = 0.8;

    /// <summary>
    /// Resets the internal state of the object to its initial configuration.
    /// </summary>
    internal void Reset()
    {
        Enabled = true;
        Color = "rgba(0, 150, 255, 0.8)";
        Width = 3.0;
        Opacity = 0.8;
    }
}

