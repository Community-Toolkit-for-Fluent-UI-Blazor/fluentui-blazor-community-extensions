namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an undoable action for adding a stroke to the signature.
/// </summary>
/// <param name="manager">Represents the history manager that tracks undoable actions for the signature component.</param>
/// <param name="stroke">Stroke represents the signature stroke that is being added.</param>
internal sealed class AddStrokeAction(StrokeManager manager, SignatureStroke stroke)
    : IUndoableAction
{
    /// <inheritdoc />
    public void Do()
    {
        manager.AddStroke(stroke);
    }

    /// <inheritdoc />
    public void Undo()
    {
        manager.RemoveStroke(stroke);
    }
}
