namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a theme for the background of a signature component, specifying the CSS color value to use.
/// </summary>
public class SignatureBackgroundTheme
{
    /// <summary>
    /// Gets or sets the CSS color value used to render the background.
    /// </summary>
    public string Color { get; set; } = "transparent";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The value determines the transparency of the component, where 1.0 represents fully opaque and
    /// 0.0 represents fully transparent. Values outside the range of 0.0 to 1.0 may result in undefined
    /// behavior.</remarks>
    public double Opacity { get; set; } = 1.0;
}

