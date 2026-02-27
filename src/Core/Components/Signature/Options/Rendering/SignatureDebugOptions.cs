namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents debug options for signature rendering, including DPI, stroke count, and point count settings.
/// </summary>
public sealed record SignatureDebugOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the feature is enabled.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the background options for the debugging of the signature rendering.
    /// </summary>
    public SignatureBackgroundOptions? Background { get; set; }

    /// <summary>
    /// Gets or sets the options used to configure the display and formatting of debug text for the signature component.
    /// </summary>
    public SignatureDebugTextOptions TextOptions { get; set; } = new SignatureDebugTextOptions();

    /// <summary>
    /// Resets the internal state of the options.
    /// </summary>
    internal void Reset()
    {
        Enabled = true;
        Background = null;
        TextOptions = new();
    }
}
