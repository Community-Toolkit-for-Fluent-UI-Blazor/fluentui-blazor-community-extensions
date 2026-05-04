using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for the appearance of a surface's foreground layer, including its visibility,
/// opacity, and color.
/// </summary>
public sealed class SurfaceForegroundOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the foreground layer is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the foreground layer.
    /// </summary>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the color applied to the foreground overlay.
    /// </summary>
    public string Color { get; set; } = StylesVariables.Colors.Brand.Foreground1;
}
