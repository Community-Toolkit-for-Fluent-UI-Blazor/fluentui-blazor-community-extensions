using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides methods for writing log messages to the console at various severity levels.
/// </summary>
/// <remarks>This class implements the IConsoleWriter interface and is intended for use in logging scenarios where
/// console output is required. It supports different log levels such as Critical, Error, Warning, Info, Debug, and
/// Trace, allowing for flexible logging based on the severity of the messages.</remarks>
internal sealed class ConsoleWriter
    : IConsoleWriter
{
    /// <summary>
    /// Provides a mechanism for managing and restoring console writing context within a defined scope.
    /// </summary>
    /// <remarks>Use this class to establish a scoped console writing context. When the scope ends, the
    /// previous console state is restored, ensuring consistent output management across nested or sequential
    /// operations.</remarks>
    private sealed class ScopeDisposable : IDisposable
    {
        /// <summary>
        /// Represents the ConsoleWriter instance that is managing the console writing context for the current scope.
        /// </summary>
        private readonly ConsoleWriter _writer;

        /// <summary>
        /// Represents the previous console writing context that was active before the current scope was established. This context is restored when the scope is disposed.
        /// </summary>
        private readonly ConsoleWriteContext? _previousContext;

        /// <summary>
        /// Initializes a new instance of the ScopeDisposable class to manage console writing within a specific scope.
        /// </summary>
        /// <remarks>Use this constructor to establish a scoped console writing context, allowing for
        /// context-aware output management. The previousContext parameter enables restoration of an earlier console
        /// state when the scope ends.</remarks>
        /// <param name="writer">The ConsoleWriter instance used to output text to the console within the scope.</param>
        /// <param name="previousContext">An optional ConsoleWriteContext representing the previous console writing context, or null if no prior
        /// context exists.</param>
        public ScopeDisposable(ConsoleWriter writer, ConsoleWriteContext? previousContext)
        {
            _writer = writer;
            _previousContext = previousContext;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_previousContext is not null)
            {
                _writer._currentContext.Value = _previousContext;
            }
        }
    }

    /// <summary>
    /// Provides access to the current state of the console.
    /// </summary>
    /// <remarks>This field is initialized when the containing class is constructed and is used to manage the
    /// console's state throughout the lifecycle of the class.</remarks>
    private readonly IConsoleState _state;

    /// <summary>
    /// Holds the context information for console write operations that is specific to the current asynchronous control
    /// flow.
    /// </summary>
    /// <remarks>This field enables correct management of console output in asynchronous and multi-threaded
    /// scenarios by maintaining a separate context for each logical operation.</remarks>
    private readonly AsyncLocal<ConsoleWriteContext?> _currentContext = new();

    /// <summary>
    /// Gets the collection of console enrichers that provide additional context or information for console output.
    /// </summary>
    /// <remarks>This collection is read-only and cannot be modified after initialization. Each enricher can
    /// enhance the console output with additional data, such as timestamps or user information.</remarks>
    private readonly IReadOnlyList<IConsoleEnricher> _enrichers;

    /// <summary>
    /// Initializes a new instance of the ConsoleWriter class using the specified console state and optional enrichers.
    /// </summary>
    /// <param name="state">The console state that provides the context for writing to the console. This parameter cannot be null.</param>
    /// <param name="enrichers">An optional collection of enrichers that can modify or enhance the output written to the console. If not
    /// provided, an empty collection is used.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="state"/> parameter is null.</exception>
    public ConsoleWriter(IConsoleState state, IEnumerable<IConsoleEnricher> enrichers)
    {
        _state = state ?? throw new ArgumentNullException(nameof(state));
        _enrichers = enrichers?.ToList() ?? [];
    }

    /// <summary>
    /// Merges two ConsoleWriteContext instances and returns a new instance that combines their properties.
    /// </summary>
    /// <remarks>If both parameters are non-null, properties from the current instance take precedence over
    /// those from the parent instance. This method is useful for consolidating context information in logging or
    /// tracing scenarios.</remarks>
    /// <param name="parent">The parent ConsoleWriteContext instance to merge. If null, the current instance is returned.</param>
    /// <param name="current">The current ConsoleWriteContext instance to merge. If null, the parent instance is returned.</param>
    /// <returns>A new ConsoleWriteContext instance containing the merged properties of both the parent and current instances, or
    /// null if both parameters are null.</returns>
    private static ConsoleWriteContext? MergeContexts(
        ConsoleWriteContext? parent,
        ConsoleWriteContext? current)
    {
        if (parent is null)
        {
            return current;
        }

        if (current is null)
        {
            return parent;
        }

        var merged = new ConsoleWriteContext
        {
            Source = current.Source ?? parent.Source,
            Category = current.Category ?? parent.Category,
            CorrelationId = current.CorrelationId ?? parent.CorrelationId,
            ActivityId = current.ActivityId ?? parent.ActivityId,
            Properties = MergeDictionaries(parent.Properties, current.Properties),
            Tags = MergeCollections(parent.Tags, current.Tags)
        };

        return merged;
    }

    /// <summary>
    /// Merges two collections of strings, returning a collection that contains unique elements from both.
    /// </summary>
    /// <remarks>This method ensures that the resulting collection contains no duplicate strings. It uses a
    /// HashSet to maintain uniqueness.</remarks>
    /// <param name="parent">The parent collection of strings to merge. If null, the current collection will be returned.</param>
    /// <param name="current">The current collection of strings to merge. If null, the parent collection will be returned.</param>
    /// <returns>A read-only collection of strings containing unique elements from both the parent and current collections.
    /// Returns null if both collections are null.</returns>
    private static IReadOnlyCollection<string>? MergeCollections(
        IReadOnlyCollection<string>? parent,
        IReadOnlyCollection<string>? current)
    {
        if (parent is null)
        {
            return current;
        }

        if (current is null)
        {
            return parent;
        }

        var merged = new HashSet<string>(parent);

        foreach (var item in current)
        {
            merged.Add(item);
        }

        return [.. merged];
    }

    /// <summary>
    /// Merges two dictionaries by combining their key-value pairs, with values from the current dictionary overwriting
    /// those from the parent dictionary for duplicate keys.
    /// </summary>
    /// <remarks>The returned dictionary is read-only and contains all entries from the parent dictionary,
    /// with any duplicate keys replaced by values from the current dictionary. This method does not modify the input
    /// dictionaries.</remarks>
    /// <param name="parent">The parent dictionary whose entries are included in the merged result. If <see langword="null"/>, only the
    /// current dictionary is returned.</param>
    /// <param name="current">The current dictionary whose entries are merged into the parent dictionary. If <see langword="null"/>, only the
    /// parent dictionary is returned.</param>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing the merged key-value pairs from both dictionaries.
    /// Returns <see langword="null"/> if both dictionaries are <see langword="null"/>.</returns>
    private static IReadOnlyDictionary<string, object?>? MergeDictionaries(
        IReadOnlyDictionary<string, object?>? parent,
        IReadOnlyDictionary<string, object?>? current)
    {
        if (parent is null)
        {
            return current;
        }

        if (current is null)
        {
            return parent;
        }

        var merged = new Dictionary<string, object?>(parent);

        foreach (var kvp in current)
        {
            merged[kvp.Key] = kvp.Value;
        }

        return merged;
    }

    /// <inheritdoc />
    public IDisposable BeginScope(ConsoleWriteContext context)
    {
        var parent = _currentContext.Value;
        _currentContext.Value = MergeContexts(parent, context);

        return new ScopeDisposable(this, parent);
    }

    /// <inheritdoc />
    public Task CriticalAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Critical, message, exception, context);
    }

    /// <inheritdoc />
    public Task DebugAsync(string message, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Debug, message, null, context);
    }

    /// <inheritdoc />
    public Task ErrorAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Error, message, null, context);
    }

    public Task InfoAsync(string message, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Information, message, null, context);
    }

    /// <inheritdoc />
    public Task TraceAsync(string message, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Trace, message, null, context);
    }

    /// <inheritdoc />
    public Task WarningAsync(string message, ConsoleWriteContext? context = null)
    {
        return WriteAsync(ConsoleLevel.Warning, message, null, context);
    }

    /// <inheritdoc />
    public async Task WriteAsync(ConsoleLevel level, string message, Exception? exception = null, ConsoleWriteContext? context = null)
    {
        var mergedContext = MergeContexts(_currentContext.Value, context);
        var bag = new ConsoleEnrichmentBag
        {
            Source = mergedContext?.Source,
            Category = mergedContext?.Category,
            CorrelationId = mergedContext?.CorrelationId,
            ActivityId = mergedContext?.ActivityId
        };

        if (mergedContext?.Properties != null)
        {
            bag.MergeProperties(mergedContext.Properties);
        }

        if (mergedContext?.Tags != null)
        {
            bag.MergeTags(mergedContext.Tags);
        }

        foreach (var enricher in _enrichers)
        {
            enricher.Enrich(bag);
        }

        var consoleMessage = new ConsoleMessage
        {
            Level = level,
            Message = message,
            Exception = exception,
            Source = bag.Source,
            Category = bag.Category ?? mergedContext?.Category,
            CorrelationId = bag.CorrelationId ?? mergedContext?.CorrelationId,
            ActivityId = bag.ActivityId ?? mergedContext?.ActivityId,
            Properties = bag.Properties,
            Tags = bag.Tags,
            ThreadId = Environment.CurrentManagedThreadId,
            MachineName = Environment.MachineName
        };

        await _state.AddAsync(consoleMessage);
    }
}
