namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the different tools that can be used to create strokes in the signature component, such as a pen for drawing, an eraser for removing parts of strokes, a selection tool for selecting and manipulating existing strokes, and a highlighter for emphasizing certain areas of the signature. Each tool may have different behaviors and visual effects when applied to strokes, allowing users to create and edit their signatures in various ways.
/// </summary>
public enum SignatureStrokeTool
{
    /// <summary>
    /// Represents a pen tool used for drawing strokes in the signature component.
    /// </summary>
    Pen,

    /// <summary>
    /// Represents an eraser tool used for removing parts of strokes in the signature component.
    /// </summary>
    Eraser,

    /// <summary>
    /// Represents the selection tool used for selecting and manipulating existing strokes in the signature component.
    /// </summary>
    Selection,

    /// <summary>
    /// Represents the highlighter tool used for emphasizing certain areas of the signature in the signature component.
    /// </summary>
    Highlighter,

    /// <summary>
    /// Represents a pointer tool used for interacting with strokes in the signature component without modifying them,
    ///  such as for hovering.
    /// </summary>
    Pointer
}
