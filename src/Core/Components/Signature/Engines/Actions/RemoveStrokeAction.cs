namespace FluentUI.Blazor.Community.Components;

internal class RemoveStrokeAction(StrokeManager manager, SignatureStroke stroke)
    : IUndoableAction
{
    /// <inheritdoc />
    public void Do()
    {
        manager.RemoveStroke(stroke);
    }

    /// <inheritdoc />
    public void Undo()
    {
        manager.AddStroke(stroke);
    }
}
