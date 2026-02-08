namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides thread-related information to a console enrichment bag for enhanced logging and debugging output.
/// </summary>
internal sealed class ThreadEnricher : IConsoleEnricher
{
    /// <inheritdoc />
    public void Enrich(ConsoleEnrichmentBag bag)
    {
        bag.Properties["Thread.Id"] = Environment.CurrentManagedThreadId;
        bag.Properties["Thread.IsThreadPool"] = Thread.CurrentThread.IsThreadPoolThread;
        bag.Properties["Thread.IsBackground"] = Thread.CurrentThread.IsBackground;
        bag.Properties["Thread.State"] = Thread.CurrentThread.ThreadState.ToString();
        bag.Properties["Thread.Priority"] = Thread.CurrentThread.Priority.ToString();
        bag.Properties["Thread.Name"] = Thread.CurrentThread.Name;
    }
}
