namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for stroke selection and highlighting behavior in signature input components.
/// </summary>
/// <remarks>Use this class to customize how strokes are selected and visually highlighted within signature input
/// controls. The options include enabling or disabling selection, adjusting the selection sensitivity, and specifying
/// highlight appearance. All properties can be reset to their default values using the Reset method.</remarks>
public sealed class SignatureSelectionRenderingOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the feature or component is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to highlight the selected stroke.
    /// </summary>
    public bool Highlight { get; set; } = true;

    /// <summary>
    /// Gets or sets the color used to highlight the selected stroke.
    /// </summary>
    public string Color { get; set; } = "rgba(0, 120, 215, 0.3)";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component, where 1.0 is fully opaque and
    /// 0.0 is fully transparent. Values outside the range of 0.0 to 1.0 may not be supported by all rendering
    /// engines.</remarks>
    public double Opacity { get; set; } = 0.6;

    /// <summary>
    /// Gets or sets the width value for the selection.
    /// </summary>
    public double Width { get; set; } = 2.0;

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Highlight = true;
        Color = "rgba(0, 120, 215, 0.3)";
        Opacity = 0.6;
        Width = 2.0;
    }
}
