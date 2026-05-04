namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the different types of cursors that can be used in the Fluent UI Blazor Community components.
/// </summary>
public enum Cursor
{
    /// <summary>
    /// No cursor is specified.
    /// </summary>
    None,

    /// <summary>
    /// Cursor is set automatically based on context.
    /// </summary>
    Auto,

    /// <summary>
    /// Cursor is the default arrow cursor.
    /// </summary>
    Default,

    /// <summary>
    /// Cursor indicates a context menu is available, typically a pointer with a small menu icon.
    /// </summary>
    ContextMenu,

    /// <summary>
    /// Cursor indicates help is available, typically a question mark or a pointer with a question mark.
    /// </summary>
    Help,

    /// <summary>
    /// Cursor indicates a clickable item, typically a hand icon.
    /// </summary>
    Pointer,

    /// <summary>
    /// Cursor indicates a progress action is occurring, typically a pointer with a spinning circle or hourglass.
    /// </summary>
    Progress,

    /// <summary>
    /// Cursor indicates that the user should wait, typically an hourglass or spinning circle.
    /// </summary>
    Wait,

    /// <summary>
    /// Cursor indicates a cell selection, typically a plus sign.
    /// </summary>
    Cell,

    /// <summary>
    /// Cursor indicates crosshair selection, typically a crosshair icon.
    /// </summary>
    Crosshair,

    /// <summary>
    /// Cursor indicates text selection, typically an I-beam icon.
    /// </summary>
    Text,

    /// <summary>
    /// Cursor indicates vertical text selection, typically an horizontal I-beam icon.
    /// </summary>
    VerticalText,

    /// <summary>
    /// Cursor indicates an alias action, typically a pointer with a small arrow.
    /// </summary>
    Alias,

    /// <summary>
    /// Cursor indicates a copy action, typically a pointer with a small plus sign.
    /// </summary>
    Copy,

    /// <summary>
    /// Cursor indicates a move action, typically a pointer with four arrows.
    /// </summary>
    Move,

    /// <summary>
    /// Cursor indicates no drop action, typically a circle with a line through it.
    /// </summary>
    NoDrop,

    /// <summary>
    /// Cursor indicates that an action is not allowed, typically a circle with a line through it.
    /// </summary>
    NotAllowed,

    /// <summary>
    /// Cursor indicates grabbing action, typically a hand icon.
    /// </summary>
    Grab,

    /// <summary>
    /// Cursor indicates grabbing action, typically a closed hand icon.
    /// </summary>
    Grabbing,

    /// <summary>
    /// Cursor indicates all-direction scrolling, typically a four-arrow icon.
    /// </summary>
    AllScroll,

    /// <summary>
    /// Cursor indicates column resizing, typically a horizontal double arrow.
    /// </summary>
    ColResize,

    /// <summary>
    /// Cursor indicates row resizing, typically a vertical double arrow.
    /// </summary>
    RowResize,

    /// <summary>
    /// Cursor indicates north resizing, typically a vertical double arrow pointing up and down.
    /// </summary>
    NResize,

    /// <summary>
    /// Cursor indicates east resizing, typically a horizontal double arrow pointing left and right.
    /// </summary>
    EResize,

    /// <summary>
    /// Cursor indicates south resizing, typically a vertical double arrow pointing up and down.
    /// </summary>
    SResize,

    /// <summary>
    /// Cursor indicates west resizing, typically a horizontal double arrow pointing left and right.
    /// </summary>
    WResize,

    /// <summary>
    /// Cursor indicates northeast resizing, typically a diagonal double arrow pointing top-right and bottom-left.
    /// </summary>
    NeResize,

    /// <summary>
    /// Cursor indicates northwest resizing, typically a diagonal double arrow pointing top-left and bottom-right.
    /// </summary>
    NwResize,

    /// <summary>
    /// Cursor indicates southeast resizing, typically a diagonal double arrow pointing top-left and bottom-right.
    /// </summary>
    SeResize,

    /// <summary>
    /// Cursor indicates southwest resizing, typically a diagonal double arrow pointing top-right and bottom-left.
    /// </summary>
    SwResize,

    /// <summary>
    /// Cursor indicates east-west resizing, typically a horizontal double arrow pointing left and right.
    /// </summary>
    EwResize,

    /// <summary>
    /// Cursor indicates north-south resizing, typically a vertical double arrow pointing up and down.
    /// </summary>
    NsResize,

    /// <summary>
    /// Cursor indicates northeast-southwest resizing, typically a diagonal double arrow pointing top-right and bottom-left.
    /// </summary>
    NeswResize,

    /// <summary>
    /// Cursor indicates northwest-southeast resizing, typically a diagonal double arrow pointing top-left and bottom-right.
    /// </summary>
    NwseResize,

    /// <summary>
    /// Cursor indicates zooming in, typically a magnifying glass with a plus sign.
    /// </summary>
    ZoomIn,

    /// <summary>
    /// Cursor indicates zooming out, typically a magnifying glass with a minus sign.
    /// </summary>
    ZoomOut
}
