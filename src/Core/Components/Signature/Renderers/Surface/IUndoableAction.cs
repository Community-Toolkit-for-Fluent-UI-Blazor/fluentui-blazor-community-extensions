namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines the contract for an action that supports both execution and reversal of its effects.
/// </summary>
/// <remarks>Implementations of this interface enable undo and redo functionality by providing methods to perform
/// and revert an action. This is commonly used in scenarios such as command patterns, editors, or applications
/// requiring reversible operations.</remarks>
public interface IUndoableAction
{
    /// <summary>
    /// Do the action, performing the necessary operations.
    /// </summary>
    void Do();

    /// <summary>
    /// Undo the action, reversing any state changes made by the Do method.
    /// </summary>
    void Undo();
}
