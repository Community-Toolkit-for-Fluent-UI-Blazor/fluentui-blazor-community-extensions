namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an undoable action that removes one or more strokes and optionally
/// adds new strokes (used by pixel and hybrid erasers).
/// </summary>
internal sealed class EraseStrokeAction : IUndoableAction
{
    /// <summary>
    /// Provides access to the stroke management functionality for handling stroke-related operations.
    /// </summary>
    private readonly StrokeManager _manager;

    /// <summary>
    /// Strokes that were removed by the eraser.
    /// </summary>
    private readonly IReadOnlyList<SignatureStroke> _removed;

    /// <summary>
    /// Strokes that were added as a result of erasing (e.g., split segments).
    /// </summary>
    private readonly IReadOnlyList<SignatureStroke> _added;

    /// <summary>
    /// Initializes a new instance of the EraseStrokeAction class to represent the removal and addition of signature
    /// strokes in the specified manager.
    /// </summary>
    /// <param name="manager">The stroke manager that tracks the collection of signature strokes affected by this action.</param>
    /// <param name="removed">The collection of signature strokes that have been removed as part of this action.</param>
    /// <param name="added">The collection of signature strokes that have been added as part of this action.</param>
    public EraseStrokeAction(
        StrokeManager manager,
        IReadOnlyList<SignatureStroke> removed,
        IReadOnlyList<SignatureStroke> added)
    {
        _manager = manager;
        _removed = removed;
        _added = added;
    }

    /// <inheritdoc />
    public void Do()
    {
        foreach (var stroke in _removed)
        {
            _manager.RemoveStroke(stroke);
        }

        foreach (var stroke in _added)
        {
            _manager.AddStroke(stroke);
        }
    }

    /// <inheritdoc />
    public void Undo()
    {
        foreach (var stroke in _added)
        {
            _manager.RemoveStroke(stroke);
        }

        foreach (var stroke in _removed)
        {
            _manager.AddStroke(stroke);
        }
    }
}
