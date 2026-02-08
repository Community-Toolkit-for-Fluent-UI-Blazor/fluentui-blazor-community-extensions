namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of the console, including the collection of messages, applied filters, and notification
/// mechanisms for state changes.
/// </summary>
/// <remarks>The ConsoleState class provides methods to add, clear, and filter console messages, as well as to
/// register observers that are notified when the state changes. It is designed to be used as the central point for
/// managing console output and its associated state within an application. This class implements the IConsoleState
/// interface, ensuring compatibility with components that depend on standardized console state management.</remarks>
internal sealed class ConsoleState : IConsoleState
{
    private readonly List<ConsoleMessage> _messages = [];
    private readonly List<ConsoleMessage> _filteredMessages = [];
    private readonly List<IConsoleStateObserver> _observers = [];
    private readonly ConsoleOptions _options;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private ConsoleFilter _filter = new();
    private int _suspendNotificationCount;

    /// <summary>
    /// Initializes a new instance of the ConsoleState class using the specified console options.
    /// </summary>
    /// <param name="options">The options that configure the behavior of the console state. Cannot be null.</param>
    public ConsoleState(ConsoleOptions options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public IReadOnlyList<ConsoleMessage> Messages => _messages;

    /// <inheritdoc />
    public IReadOnlyList<ConsoleMessage> FilteredMessages => _filteredMessages;

    /// <inheritdoc />
    public ConsoleFilter Filter => _filter;

    /// <inheritdoc />
    public event EventHandler<ConsoleStateChangedEventArgs>? Changed;

    /// <summary>
    /// Adds a console message to the internal message collection, enforcing the maximum message limit and updating
    /// filtered messages as needed.
    /// </summary>
    /// <remarks>If the total number of messages exceeds the configured maximum, the oldest messages are
    /// removed to maintain the limit. Messages that satisfy the current filter are also added to the filtered messages
    /// collection.</remarks>
    /// <param name="message">The console message to add. This parameter cannot be null.</param>
    private void AddInternal(ConsoleMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messages.Add(message);

        if (_messages.Count > _options.MaxMessages)
        {
            var itemsToRemove = _messages.Count - _options.MaxMessages;
            _messages.RemoveRange(0, itemsToRemove);
            RebuildFilteredMessages();
        }
        else
        {
            if (_filter.Match(message))
            {
                _filteredMessages.Add(message);
            }
        }
    }

    /// <summary>
    /// Notifies all registered observers that the state has been cleared.
    /// </summary>
    /// <remarks>This method invokes the OnCleared method on each registered observer. It is typically used to
    /// inform observers that the subject's state has been reset, allowing them to update their own state or perform
    /// necessary cleanup actions in response.</remarks>
    private void NotifyObservers()
    {
        foreach(var observer in _observers)
        {
            observer.OnCleared();
        }
    }

    /// <summary>
    /// Rebuilds the list of filtered messages based on the current filter criteria.
    /// </summary>
    private void RebuildFilteredMessages()
    {
        _filteredMessages.Clear();
        _filteredMessages.AddRange(_messages.Where(_filter.Match));
    }

    /// <summary>
    /// Notifies subscribers of a change in the console state.
    /// </summary>
    /// <remarks>No notification is sent if notifications are currently suspended. This method raises the
    /// Changed event to inform subscribers of the specified change.</remarks>
    /// <param name="changeKind">The type of change that occurred in the console. This value is specified by the ConsoleChangeKind enumeration
    /// and indicates the nature of the state change being reported.</param>
    private void Notify(ConsoleChangeKind changeKind)
    {
        if (_suspendNotificationCount > 0)
        {
            return;
        }

        Changed?.Invoke(this, new ConsoleStateChangedEventArgs(changeKind));
    }

    /// <summary>
    /// Resumes notifications that were previously suspended. Decrements the suspension count and triggers a
    /// notification if all suspensions have been lifted.
    /// </summary>
    /// <remarks>Call this method after suspending notifications to re-enable them. Notifications are only
    /// sent when the suspension count reaches zero, ensuring that all suspension requests have been released. This
    /// method is typically used in conjunction with a corresponding method that suspends notifications.</remarks>
    private void ResumeNotifications()
    {
        if (Interlocked.Decrement(ref _suspendNotificationCount) == 0)
        {
            Notify(ConsoleChangeKind.BatchCompleted);
        }
    }

    /// <summary>
    /// Notifies all registered observers that new console messages have been added.
    /// </summary>
    /// <remarks>Each observer receives the provided messages through its OnMessagesAdded method. Observers
    /// must be registered before this method is called to receive notifications.</remarks>
    /// <param name="messages">A read-only list of console messages to be delivered to each observer. Cannot be null.</param>
    private void NotifyObservers(IReadOnlyList<ConsoleMessage> messages)
    {
        foreach(var observer in _observers)
        {
            observer.OnMessagesAdded(messages);
        }
    }

    /// <inheritdoc />
    public async Task AddAsync(ConsoleMessage message)
    {
        if (_options.EnableThreadSafety)
        {
            await _semaphore.WaitAsync();
            AddInternal(message);
            _semaphore.Release();
        }
        else
        {
            AddInternal(message);
        }

        Notify(ConsoleChangeKind.MessageAdded);
        NotifyObservers([message]);
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<ConsoleMessage> messages)
    {
        ArgumentNullException.ThrowIfNull(messages);

        if (_options.EnableThreadSafety)
        {
            await _semaphore.WaitAsync();

            foreach (var message in messages)
            {
                AddInternal(message);
            }

            _semaphore.Release();
        }
        else
        {
            foreach (var message in messages)
            {
                AddInternal(message);
            }
        }

        Notify(ConsoleChangeKind.MessageAdded);
        NotifyObservers([.. messages]);
    }

    /// <inheritdoc />
    public async Task ClearAsync()
    {
        if (_options.EnableThreadSafety)
        {
            await _semaphore.WaitAsync();

            _messages.Clear();
            _filteredMessages.Clear();

            _semaphore.Release();
        }
        else
        {
            _messages.Clear();
            _filteredMessages.Clear();
        }

        Notify(ConsoleChangeKind.Cleared);
        NotifyObservers();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _observers.Clear();
         _semaphore.Dispose();
    }

    /// <inheritdoc />
    public void RegisterObserver(IConsoleStateObserver observer)
    {
        _observers.Add(observer);
    }

    /// <inheritdoc />
    public async Task SetFilterAsync(ConsoleFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        if (_options.EnableThreadSafety)
        {
            await _semaphore.WaitAsync();

            _filter = filter;

            RebuildFilteredMessages();

            _semaphore.Release();
        }
        else
        {
            _filter = filter;
            RebuildFilteredMessages();
        }

        Notify(ConsoleChangeKind.FilterChanged);
    }

    /// <inheritdoc />
    public IDisposable SuspendNotifications()
    {
        Interlocked.Increment(ref _suspendNotificationCount);

        return new NotificationSuspender(this);
    }

    /// <inheritdoc />
    public void UnregisterObserver(IConsoleStateObserver observer)
    {
        _observers.Remove(observer);
    }

    private sealed class NotificationSuspender : IDisposable
    {
        private readonly ConsoleState _consoleState;
        public NotificationSuspender(ConsoleState consoleState)
        {
            _consoleState = consoleState;
        }

        public void Dispose()
        {
            _consoleState.ResumeNotifications();
        }
    }
}
