namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the type of change that occurred in the console, such as a new message being added, a filter being changed, the console being cleared, or a batch of messages being completed. This enumeration is used to identify the specific kind of update that has taken place in the console's state,
///  allowing for appropriate handling and rendering of changes in the user interface.
/// </summary>
public enum ConsoleChangeKind
{
    /// <summary>
    /// Occurs when a new message is added to the system.
    /// </summary>
    MessageAdded,

    /// <summary>
    /// Occurs when the filter criteria has changed.
    /// </summary>
    FilterChanged,

    /// <summary>
    /// Occurs when the console has been cleared.
    /// </summary>
    Cleared,

    /// <summary>
    /// Occurs when a batch operation has completed.
    /// </summary>
    BatchCompleted
}
