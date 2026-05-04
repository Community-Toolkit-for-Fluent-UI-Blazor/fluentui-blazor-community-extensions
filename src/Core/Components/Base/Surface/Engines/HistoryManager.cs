namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides undo and redo functionality for actions that implement the IUndoableAction interface. Maintains a history
/// stack to allow reversing and reapplying actions as needed.
/// </summary>
/// <remarks>Use this class to manage a sequence of user actions that can be undone and redone, such as in editing
/// scenarios. Actions must implement the IUndoableAction interface, which defines how to perform and reverse the
/// action. The history is cleared when a new action is executed after undoing previous actions. This class is not
/// thread-safe.</remarks>
public sealed class HistoryManager
{
    /// <summary>
    /// Represents the configuration options for undo and redo functionality.
    /// </summary>
    public SignatureUndoRedoOptions _options;

    /// <summary>
    /// Represents the stack of actions that have been performed and can be undone.
    /// </summary>
    private readonly Stack<IUndoableAction> _undo = new();

    /// <summary>
    /// Represents the stack of actions available to be redone after an undo operation.
    /// </summary>
    private readonly Stack<IUndoableAction> _redo = new();

    /// <summary>
    /// Initializes a new instance of the HistoryManager class with the specified undo and redo options.
    /// </summary>
    /// <param name="options">The configuration options that define the behavior for undo and redo operations. Cannot be null.</param>
    public HistoryManager(SignatureUndoRedoOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Gets a value indicating whether the current operation or process is suspended.
    /// </summary>
    public bool IsSuspended { get; private set; }

    /// <summary>
    /// Occurs when the state or value associated with the component changes.
    /// </summary>
    /// <remarks>Subscribers can use this event to respond to changes in the component's state. The event is
    /// raised whenever a relevant change occurs, allowing external handlers to update accordingly.</remarks>
    public event EventHandler? OnChanged;

    /// <summary>
    /// Gets a value indicating whether an undo operation can currently be performed.
    /// </summary>
    /// <remarks>An undo operation is available only when undo functionality is enabled and there is at least
    /// one action to undo.</remarks>
    public bool CanUndo => _options.Enabled && _undo.Count > 0;

    /// <summary>
    /// Gets a value indicating whether a redo operation can be performed.
    /// </summary>
    /// <remarks>A redo operation is available when the component is enabled and there are actions in the redo
    /// stack. Use this property to determine if calling the redo command is valid in the current state.</remarks>
    public bool CanRedo => _options.Enabled && _redo.Count > 0;

    /// <summary>
    /// Executes the specified undoable action and adds it to the undo stack.
    /// </summary>
    /// <remarks>After execution, the action is pushed onto the undo stack, and the redo stack is cleared.
    /// This ensures that redo operations are only available for actions that have been undone, maintaining the
    /// integrity of the undo/redo history.</remarks>
    /// <param name="action">The action to execute. Must implement the <see cref="IUndoableAction"/> interface.</param>
    /// <param name="isEraserAction">Indicates whether the action is an eraser action. If true and eraser actions are not tracked, the action will be executed without being added to the undo stack.</param>
    /// <param name="isOptionChange">Indicates whether the action is an option change. If true and option changes are not tracked, the action will be executed without being added to the undo stack.</param>
    public void Execute(
        IUndoableAction action,
        bool isEraserAction = false,
        bool isOptionChange = false)
    {
        if (!_options.Enabled || IsSuspended)
        {
            action.Do();
            return;
        }

        if (isEraserAction && !_options.TrackEraserActions)
        {
            action.Do();
            return;
        }

        if (isOptionChange && !_options.TrackOptionChanges)
        {
            action.Do();
            return;
        }

        action.Do();
        _undo.Push(action);
        _redo.Clear();
        Trim();
        NotifyChanges();
    }

    private void NotifyChanges()
    {
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Undo the last action.
    /// </summary>
    public bool Undo()
    {
        if (!CanUndo)
        {
            return false;
        }

        var action = _undo.Pop();
        action.Undo();
        _redo.Push(action);
        NotifyChanges();

        return true;
    }

    /// <summary>
    /// Redo the last undone action.
    /// </summary>
    public bool Redo()
    {
        if (!CanRedo)
        {
            return false;
        }

        var action = _redo.Pop();
        action.Do();
        _undo.Push(action);
        NotifyChanges();

        return true;
    }

    /// <summary>
    /// Clears both undo and redo stacks.
    /// </summary>
    public void Clear()
    {
        _undo.Clear();
        _redo.Clear();
        NotifyChanges();
    }

    /// <summary>
    /// Temporarily suspends history recording.
    /// </summary>
    public void Suspend() => IsSuspended = true;

    /// <summary>
    /// Resumes history recording.
    /// </summary>
    public void Resume() => IsSuspended = false;

    /// <summary>
    /// Returns the last undoable action without removing it.
    /// </summary>
    public IUndoableAction? PeekUndo() => _undo.Count > 0 ? _undo.Peek() : null;

    /// <summary>
    /// Returns the last redoable action without removing it.
    /// </summary>
    public IUndoableAction? PeekRedo() => _redo.Count > 0 ? _redo.Peek() : null;

    /// <summary>
    /// Replaces the last undo entry (useful for merging actions).
    /// </summary>
    public void ReplaceLast(IUndoableAction action)
    {
        if (_undo.Count == 0)
        {
            return;
        }

        _undo.Pop();
        _undo.Push(action);
        NotifyChanges();
    }

    /// <summary>
    /// Ensures the undo stack does not exceed MaxUndoSteps.
    /// </summary>
    private void Trim()
    {
        if (_options.MaxUndoSteps <= 0)
        {
            return;
        }

        while (_undo.Count > _options.MaxUndoSteps)
        {
            RemoveBottom(_undo);
        }
    }

    /// <summary>
    /// Removes the bottom element from the specified stack of undoable actions.
    /// </summary>
    /// <remarks>This method modifies the input stack by removing its oldest element, effectively reducing its
    /// count by one. The order of the remaining elements is preserved.</remarks>
    /// <param name="stack">The stack from which to remove the bottom element. Cannot be null.</param>
    private static void RemoveBottom(Stack<IUndoableAction> stack)
    {
        var items = stack.ToArray();
        Array.Reverse(items);
        stack.Clear();

        for (var i = 1; i < items.Length; i++)
        {
            stack.Push(items[i]);
        }
    }
}
