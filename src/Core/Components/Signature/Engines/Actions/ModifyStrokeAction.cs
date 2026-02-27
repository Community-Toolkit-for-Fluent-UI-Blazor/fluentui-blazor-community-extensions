namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an action that modifies a stroke in the signature component.
/// This action can be undone and redone, allowing users to revert changes made
///  to a stroke or reapply them as needed.
/// </summary>
internal sealed class ModifyStrokeAction
    : IUndoableAction
{
    /// <summary>
    /// Represents the current signature stroke being processed.
    /// </summary>
    private readonly SignatureStroke _current;

    /// <summary>
    /// Represents the signature stroke state prior to the current operation.
    /// </summary>
    private readonly SignatureStroke _before;

    /// <summary>
    /// Represents the signature stroke state after an operation or modification.
    /// </summary>
    private readonly SignatureStroke _after;

    /// <summary>
    /// Initialize a new instance of the <see cref="ModifyStrokeAction"/> class with
    ///  the current stroke being modified, the state of the stroke before modification,
    ///  and the state of the stroke after modification.
    /// </summary>
    /// <param name="current">Current stroke being modified. This is the stroke that will be updated when the action is performed or undone.</param>
    /// <param name="before">State of the stroke before modification.</param>
    /// <param name="after">State of the stroke after modification.</param>
    public ModifyStrokeAction(
        SignatureStroke current,
        SignatureStroke before,
        SignatureStroke after)
    {
        _current = current;
        _before = before.Clone();
        _after = after.Clone();
    }

    /// <inheritdoc />
    public void Do()
    {
        _current.ReplaceWith(_after);
    }

    /// <inheritdoc />
    public void Undo()
    {
        _current.ReplaceWith(_before);
    }
}
