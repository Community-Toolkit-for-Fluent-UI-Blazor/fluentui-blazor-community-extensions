namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents options for configuring the background of a signature component.
/// </summary>
public class SignatureBackgroundOptions
{
    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    public string Color { get; set; } = "transparent";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The value determines the transparency of the component, where 1.0 represents fully opaque and
    /// 0.0 represents fully transparent. Values outside the range 0.0 to 1.0 may result in undefined
    /// behavior.</remarks>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets a value indicating whether the background should show.
    /// </summary>
    public bool Show { get; set; } = true;

    /// <summary>
    /// Resets the background properties to their default values.
    /// </summary>
    internal void Reset()
    {
        Color = "transparent";
        Opacity = 1.0;
        Show = true;
    }
}
