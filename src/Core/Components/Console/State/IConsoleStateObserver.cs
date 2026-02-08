namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for receiving notifications when the console state changes, such as when the console is cleared or
/// new messages are added.
/// </summary>
/// <remarks>Implementing this interface enables a class to respond to console state changes, allowing custom
/// actions to be performed when the console is modified. Typical scenarios include updating UI elements or logging
/// changes when the console is cleared or messages are appended.</remarks>
public interface IConsoleStateObserver
{
    /// <summary>
    /// Invoked when the object's state is cleared, allowing observers to perform cleanup or reset operations.
    /// </summary>
    /// <remarks>Implementations should use this method to release resources or reset properties to their
    /// default values as needed. This method is typically called when the observed object's state is reset, and may be
    /// used to ensure consistency or prepare for subsequent operations.</remarks>
    void OnCleared();

    /// <summary>
    /// Handles newly added console messages for processing or observation.
    /// </summary>
    /// <remarks>This method is typically invoked when new messages are generated in the console, allowing
    /// observers to react in real time, such as logging or updating UI elements. Implementations should not modify the
    /// contents of the provided list.</remarks>
    /// <param name="messages">A read-only list containing the console messages that have been added. Cannot be null.</param>
    void OnMessagesAdded(IReadOnlyList<ConsoleMessage> messages);
}
