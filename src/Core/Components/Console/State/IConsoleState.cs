namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of a console, providing access to console messages and filtering capabilities.
/// </summary>
/// <remarks>Implementations of this interface allow for the management of console messages, including adding new
/// messages, filtering existing messages, and notifying observers of state changes. The event 'Changed' is raised
/// whenever the console state changes, enabling clients to react to updates. Observers can be registered to receive
/// notifications, and notifications can be temporarily suspended if needed. Filtering enables clients to view a subset
/// of messages based on specified criteria.</remarks>
public interface IConsoleState : IDisposable
{
    /// <summary>
    /// Occurs when the console state changes, providing information about the new state.
    /// </summary>
    /// <remarks>This event can be used to respond to changes in the console's operational state, such as when
    /// it is opened, closed, or modified. Subscribers to this event should handle it appropriately to ensure that any
    /// necessary updates or actions are taken in response to the state change.</remarks>
    event EventHandler<ConsoleStateChangedEventArgs>? Changed;

    /// <summary>
    /// Gets the collection of console messages.
    /// </summary>
    IReadOnlyList<ConsoleMessage> Messages { get; }

    /// <summary>
    /// Gets the list of console messages that satisfy the current filtering criteria.
    /// </summary>
    /// <remarks>The returned list is read-only and reflects the messages that meet the applied filters. The
    /// contents update automatically when the filtering conditions change.</remarks>
    IReadOnlyList<ConsoleMessage> FilteredMessages { get; }

    /// <summary>
    /// Gets the current filter settings applied to console output messages.
    /// </summary>
    /// <remarks>The filter determines which messages are displayed in the console based on their severity
    /// level. Adjusting the filter can help in debugging by controlling the verbosity of the output.</remarks>
    ConsoleFilter Filter { get; }

    /// <summary>
    /// Adds a new console message in an asynchronous manner.
    /// </summary>
    /// <param name="message">Console message to add.</param>
    /// <returns>A task that represents the asynchronous operation of adding a console message. The task completes when the message has been successfully added to the console state.</returns>
    Task AddAsync(ConsoleMessage message);

    /// <summary>
    /// Adds a collection of console messages in an asynchronous manner..
    /// </summary>
    /// <remarks>This method allows for batch addition of messages, improving performance compared to adding
    /// messages individually. Ensure that the collection is not modified while this method is executing.</remarks>
    /// <param name="messages">The collection of console messages to be added. This collection cannot be null and must contain valid
    /// ConsoleMessage objects.</param>
    /// <returns>A task that represents the asynchronous operation of adding multiple console messages. The task completes when all messages have been successfully added to the console state.</returns>
    Task AddRangeAsync(IEnumerable<ConsoleMessage> messages);

    /// <summary>
    /// Removes all items from the collection, resetting its state to empty.
    /// </summary>
    /// <remarks>After calling this method, the collection contains no elements. Subsequent operations will
    /// reflect the empty state. This method does not throw exceptions under normal circumstances.</remarks>
    Task ClearAsync();

    /// <summary>
    /// Sets the console filter to the specified filter configuration, controlling which messages are displayed in the
    /// console based on their severity or category.
    /// </summary>
    /// <remarks>Use this method to dynamically adjust the console output. Ensure that the provided filter is
    /// properly configured to avoid unintentionally hiding important messages.</remarks>
    /// <param name="filter">The filter configuration to apply. Determines which console messages are visible according to their severity
    /// level or category. Cannot be null.</param>
    Task SetFilterAsync(ConsoleFilter filter);

    /// <summary>
    /// Suspends notification delivery, allowing temporary suppression of events until resumed.
    /// </summary>
    /// <remarks>Use this method to batch changes or operations without triggering notifications for each
    /// individual event. Ensure that the returned <see cref="IDisposable"/> is disposed to restore standard
    /// notification behavior. This is particularly useful in scenarios where performance or consistency is important
    /// during bulk updates.</remarks>
    /// <returns>An <see cref="IDisposable"/> instance that, when disposed, resumes normal notification delivery.</returns>
    IDisposable SuspendNotifications();

    /// <summary>
    /// Registers an observer that will receive notifications when the console state changes.
    /// </summary>
    /// <remarks>Use this method to subscribe to console state updates. The observer should implement the
    /// necessary logic to handle state change notifications. Passing a null observer will result in an
    /// exception.</remarks>
    /// <param name="observer">The observer instance to be notified of console state changes. This parameter must not be null.</param>
    void RegisterObserver(IConsoleStateObserver observer);

    /// <summary>
    /// Unregisters the specified observer so that it no longer receives notifications about console state changes.
    /// </summary>
    /// <remarks>Call this method when the observer is no longer interested in receiving updates, such as when
    /// it is being disposed or removed from the application.</remarks>
    /// <param name="observer">The observer to remove from the notification list. This parameter cannot be null and must have been previously
    /// registered.</param>
    void UnregisterObserver(IConsoleStateObserver observer);
}
