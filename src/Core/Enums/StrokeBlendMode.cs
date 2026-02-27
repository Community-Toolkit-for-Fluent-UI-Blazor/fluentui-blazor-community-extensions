namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the available blend modes for rendering a stroke, determining how the stroke's color is composited with
/// the background.
/// </summary>
/// <remarks>Use this enumeration to control the visual blending effect applied when drawing strokes. Different
/// blend modes can be used to achieve various artistic or visual effects, such as darkening, lightening, or enhancing
/// contrast between the stroke and the background.</remarks>
public enum StrokeBlendMode
{
    /// <summary>
    /// The default blend mode where the stroke is drawn normally without any blending effects.
    /// </summary>
    Normal,

    /// <summary>
    /// The stroke is drawn using a multiply blend mode, which multiplies the stroke's color with the background color, resulting in a darker effect.
    /// </summary>
    Multiply,

    /// <summary>
    /// The stroke is drawn using an additive blend mode, which adds the stroke's color to the background color, resulting in a lighter effect.
    /// </summary>
    Additive,

    /// <summary>
    /// The stroke is drawn using a screen blend mode, which inverts both the stroke and background colors, multiplies them, and then inverts the result, creating a lighter effect.
    /// </summary>
    Screen,

    /// <summary>
    /// The stroke is drawn using an overlay blend mode, which combines multiply and screen blend modes to create a contrast-enhancing effect.
    /// </summary>
    Overlay
}
