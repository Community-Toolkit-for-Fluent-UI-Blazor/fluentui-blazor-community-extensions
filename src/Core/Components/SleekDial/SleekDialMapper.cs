namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a static class that provides mapping functionality for the SleekDial component,
///  allowing for the conversion of linear directions and floating positions to their corresponding
///  popup placements and anchor logical positions. 
/// </summary>
internal static class SleekDialMapper
{
    /// <summary>
    /// Maps a specified linear direction to its corresponding popup placement value.
    /// </summary>
    /// <param name="direction">The linear direction to map to a popup placement. Must be a valid value of the SleekDialLinearDirection
    /// enumeration.</param>
    /// <returns>A PopupPlacement value that corresponds to the specified linear direction. Returns PopupPlacement.Auto if the
    /// direction is not recognized.</returns>
    internal static PopupPlacement MapLinearDirectionToPopupPlacement(SleekDialLinearDirection direction)
    {
        return direction switch
        {
            SleekDialLinearDirection.Left => PopupPlacement.LeftCenter,
            SleekDialLinearDirection.Right => PopupPlacement.RightCenter,
            SleekDialLinearDirection.Up => PopupPlacement.TopCenter,
            SleekDialLinearDirection.Down => PopupPlacement.BottomCenter,
            _ => PopupPlacement.Auto
        };
    }

    /// <summary>
    /// Maps a specified linear direction to its corresponding preferred popup direction.
    /// </summary>
    /// <param name="dir">The linear direction to convert to a preferred popup direction.</param>
    /// <returns>The preferred popup direction that corresponds to the specified linear direction. Returns the default direction
    /// if the input is not recognized.</returns>
    internal static PreferredPopupDirection MapDirectionToPreferred(SleekDialLinearDirection dir)
    {
        return dir switch
        {
            SleekDialLinearDirection.Left => PreferredPopupDirection.Left,
            SleekDialLinearDirection.Right => PreferredPopupDirection.Right,
            SleekDialLinearDirection.Up => PreferredPopupDirection.Up,
            SleekDialLinearDirection.Down => PreferredPopupDirection.Down,
            _ => PreferredPopupDirection.Default
        };
    }

    /// <summary>
    /// Maps a specified floating position to its corresponding anchor logical position.
    /// </summary>
    /// <param name="pos">The floating position to convert to an anchor logical position.</param>
    /// <returns>The anchor logical position that corresponds to the specified floating position. Returns <see
    /// cref="AnchorLogicalPosition.BottomRight"/> if the input value is not recognized.</returns>
    internal static AnchorLogicalPosition MapFloatingPosition(FloatingPosition pos)
    {
        return pos switch
        {
            FloatingPosition.TopLeft => AnchorLogicalPosition.TopLeft,
            FloatingPosition.TopCenter => AnchorLogicalPosition.TopCenter,
            FloatingPosition.TopRight => AnchorLogicalPosition.TopRight,

            FloatingPosition.MiddleLeft => AnchorLogicalPosition.MiddleLeft,
            FloatingPosition.MiddleCenter => AnchorLogicalPosition.MiddleCenter,
            FloatingPosition.MiddleRight => AnchorLogicalPosition.MiddleRight,

            FloatingPosition.BottomLeft => AnchorLogicalPosition.BottomLeft,
            FloatingPosition.BottomCenter => AnchorLogicalPosition.BottomCenter,
            FloatingPosition.BottomRight => AnchorLogicalPosition.BottomRight,

            _ => AnchorLogicalPosition.BottomRight
        };
    }
}
