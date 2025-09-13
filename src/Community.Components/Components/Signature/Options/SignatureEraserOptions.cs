using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the eraser options for the signature component.
/// </summary>
public class SignatureEraserOptions
{
    /// <summary>
    /// Gets or sets the size of the eraser in pixels.
    /// </summary>
    public double Size { get; set; } = 10.0;

    /// <summary>
    /// Gets or sets the shape of the eraser. Can be either Circle or Square.
    /// </summary>
    public EraserShape Shape { get; set; } = EraserShape.Circle;

    /// <summary>
    /// Gets or sets the opacity of the eraser, ranging from 0.0 (fully transparent) to 1.0 (fully opaque).
    /// </summary>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets a value indicating whether the eraser should have soft edges.
    /// </summary>
    public bool SoftEdges { get; set; }

    /// <summary>
    /// Gets or sets the cursor style when the eraser is active. This can be any valid CSS cursor value.
    /// </summary>
    [JsonIgnore]
    public Cursor Cursor { get; set; } = Cursor.Crosshair;

    /// <summary>
    /// Gets the CSS representation of the cursor style.
    /// </summary>
    public string? CursorCss => Cursor == Cursor.None ? null : Cursor.ToString().ToLowerInvariant().Replace("default", "auto");

    /// <summary>
    /// Gets or sets a value indicating whether the eraser performs a partial erase.
    /// </summary>
    public bool PartialErase { get; set; }

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Size = 10.0;
        Shape = EraserShape.Circle;
        Opacity = 1.0;
        SoftEdges = false;
        Cursor = Cursor.Crosshair;
        PartialErase = false;
    }
}
