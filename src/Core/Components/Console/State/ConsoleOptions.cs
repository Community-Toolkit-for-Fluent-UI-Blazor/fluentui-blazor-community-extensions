 namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration options for console output behavior, including message limits and threading settings.
/// </summary>
/// <remarks>This class allows customization of console output, such as enabling auto-scrolling, batching of
/// messages, and thread safety. Adjusting these options can enhance performance and usability in multi-threaded
/// applications.</remarks>
public sealed class ConsoleOptions
{
    /// <summary>
    /// Gets or sets the maximum of messages to retain in the console.
    ///  When the limit is exceeded, the oldest messages will be removed to make room for new ones.
    /// </summary>
    public int MaxMessages { get; set; } = 10_000;

    /// <summary>
    /// Gets or sets a value indicating whether the control automatically scrolls to display the active content.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the control will automatically scroll to ensure that the
    /// currently active content is visible. This is particularly useful in scenarios where content may exceed the
    /// visible area of the control.</remarks>
    public bool AutoScroll { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether batching of requests is enabled.
    /// </summary>
    /// <remarks>When enabled, multiple requests can be sent in a single batch, potentially improving
    /// performance by reducing the number of network calls. This property defaults to <see langword="true"/>.</remarks>
    public bool EnableBatching { get; set; } = true;

    /// <summary>
    /// Gets or sets the interval between batches of operations, specified as a TimeSpan.
    /// </summary>
    /// <remarks>The default value is set to 100 milliseconds. Adjusting this value can influence the
    /// frequency of batch processing, which may impact performance and resource utilization.</remarks>
    public TimeSpan BatchInterval { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets a value indicating whether stock exceptions are enabled.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, stock exceptions will be processed; otherwise, they will
    /// be ignored.</remarks>
    public bool StockExceptions { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether thread safety is enabled for the operations performed by this instance.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the instance ensures that operations are thread-safe,
    /// which may introduce performance overhead. If set to <see langword="false"/>, operations may be faster but are
    /// not safe for concurrent access.</remarks>
    public bool EnableThreadSafety { get; set; } = true;
}
