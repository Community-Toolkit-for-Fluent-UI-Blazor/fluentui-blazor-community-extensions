namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for customizing the behavior and appearance of the signature eraser tool.
/// </summary>
/// <remarks>Use this class to specify eraser properties such as size, shape, opacity, edge softness, cursor
/// style, erasing mode, and stroke deletion threshold. These options allow fine-tuning of the eraser's effect when
/// removing parts of a signature. Changing properties affects subsequent erasing actions. The default values are
/// suitable for general use, but can be adjusted for specific scenarios such as partial erasing or pressure-sensitive
/// input.</remarks>
public class SignatureEraserOptions
{
    /// <summary>
    /// Gets or sets the size value used by the component.
    /// </summary>
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the shape used by the eraser tool.
    /// </summary>
    /// <remarks>The shape determines how the eraser interacts with the canvas. Common shapes include circle
    /// and square. Changing the shape affects the area removed when erasing.</remarks>
    public EraserShape Shape { get; set; } = EraserShape.Circle;

    /// <summary>
    /// Gets or sets a value indicating whether soft edges are applied to the component.
    /// </summary>
    public bool SoftEdges { get; set; }

    /// <summary>
    /// Gets or sets the radius, in pixels, used to apply a soft edge effect to the component.
    /// </summary>
    public double SoftEdgeRadius { get; set; } = 4.0;

    /// <summary>
    /// Gets or sets the cursor style to be displayed when the mouse pointer is over the component.
    /// </summary>
    public Cursor Cursor { get; set; } = Cursor.Crosshair;

    /// <summary>
    /// Gets or sets the eraser mode used to determine how erasing is performed.
    /// </summary>
    /// <remarks>Use this property to select the erasing behavior, such as pixel-based or other supported
    /// modes. Changing the mode affects how the eraser interacts with the target surface.</remarks>
    public EraserMode Mode { get; set; } = EraserMode.Pixel;

    /// <summary>
    /// Gets or sets the threshold value used to determine when a stroke should be considered for deletion.
    /// </summary>
    /// <remarks>A stroke is deleted if its calculated value falls below this threshold. Adjust this value to
    /// control the sensitivity of stroke deletion in the component.</remarks>
    public double Tolerance { get; set; } = 0.6;

    /// <summary>
    /// Gets the radius of the shape, calculated as half of its size.
    /// </summary>
    public double Radius => Size / 2.0;

    /// <summary>
    /// Resets all eraser settings to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the eraser's configuration to its initial state. This is useful
    /// when you want to discard any customizations and revert to standard settings. Calling this method will overwrite
    /// any previously set values for size, shape, opacity, soft edges, pressure sensitivity, cursor type, partial erase
    /// mode, eraser mode, and stroke deletion threshold.</remarks>
    public void Reset()
    {
        Size = 10;
        Shape = EraserShape.Circle;
        SoftEdges = false;
        SoftEdgeRadius = 4.0;
        Cursor = Cursor.Crosshair;
        Mode = EraserMode.Pixel;
        Tolerance = 0.6;
    }
}
