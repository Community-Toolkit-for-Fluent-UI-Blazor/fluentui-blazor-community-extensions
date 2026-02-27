namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the different modes of interaction available for the signature tool, including drawing, erasing, selecting, and panning.
/// </summary>
public enum SignatureToolMode
{
    /// <summary>
    /// Draw mode allows the user to create new strokes on the signature canvas using a pen or stylus input. In this mode, input is interpreted as drawing actions, and new strokes are added to the signature.
    /// </summary>
    Draw,

    /// <summary>
    /// Erase mode enables the user to remove existing strokes from the signature canvas. In this mode, input is interpreted as erasing actions, allowing users to delete parts of their signature by interacting with the strokes they wish to remove.
    /// </summary>
    Erase,

    /// <summary>
    /// Select mode allows the user to select and manipulate existing strokes on the signature canvas. In this mode, input is interpreted as selection actions, enabling users to move, resize, or modify selected strokes as needed.
    /// </summary>
    Select,

    /// <summary>
    /// Pan mode enables the user to navigate around the signature canvas without modifying the strokes. In this mode, input is interpreted as panning actions, allowing users to move the view of the canvas to access different areas without altering their signature.
    /// </summary>
    Pan
}
