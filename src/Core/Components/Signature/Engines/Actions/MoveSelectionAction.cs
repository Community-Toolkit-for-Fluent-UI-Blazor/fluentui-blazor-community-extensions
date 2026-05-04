namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an action that moves a selection of strokes in the signature component.
/// </summary>
internal sealed class MoveSelectionAction
    : IUndoableAction
{
    /// <summary>
    /// Represents the strokes that are being moved as part of this action. These strokes will be translated by the specified delta values when the action is performed, and will be translated back when the action is undone.
    /// </summary>
    private readonly IReadOnlyList<SignatureStroke> _strokes;

    /// <summary>
    /// Represents the horizontal distance (deltaX) that the selected strokes will be moved when the action is performed. These values will be negated when the action is undone to move the strokes back to their original positions.
    /// </summary>
    private readonly double _deltaX;

    /// <summary>
    /// Represents the vertical distance (deltaY) that the selected strokes will be moved when the action is performed. These values will be negated when the action is undone to move the strokes back to their original positions.
    /// </summary>
    private readonly double _deltaY;

    /// <summary>
    /// Represents an action that moves a selection of strokes in the signature component.
    /// </summary>
    /// <param name="strokes">Strokes that are being moved as part of this action.</param>
    /// <param name="deltaX">Horizontal distance that the selected strokes will be moved when the action is performed.</param>
    /// <param name="deltaY">Vertical distance that the selected strokes will be moved when the action is performed.</param>
    public MoveSelectionAction(
        IReadOnlyList<SignatureStroke> strokes,
        double deltaX,
        double deltaY)
    {
        _strokes = strokes;
        _deltaX = deltaX;
        _deltaY = deltaY;
    }

    /// <inheritdoc />
    public void Do()
    {
        foreach (var stroke in _strokes)
        {
            stroke.Translate(_deltaX, _deltaY);
        }
    }

    /// <inheritdoc />
    public void Undo()
    {
        foreach(var stroke in _strokes)
        {
            stroke.Translate(-_deltaX, -_deltaY);
        }
    }
}
