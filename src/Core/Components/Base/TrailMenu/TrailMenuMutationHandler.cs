namespace FluentUI.Blazor.Community.Components.TrailMenu;

/// <summary>
/// Provides functionality to handle mutation events for the trail menu, including debounced resize operations.
/// </summary>
/// <remarks>This class is intended for internal use to manage trail menu state changes that may require
/// asynchronous refreshes, such as those triggered by resize events. It ensures that only the most recent resize
/// operation is processed, canceling any previous pending refreshes. This approach helps prevent unnecessary refreshes
/// and improves responsiveness in dynamic UI scenarios.</remarks>
internal sealed class TrailMenuMutationHandler
{
    /// <summary>
    /// Gets or sets the cancellation token source used for managing cancellation of resize operations.
    /// </summary>
    /// <remarks>This field is used to signal cancellation requests for ongoing resize operations. It is
    /// important to ensure that the cancellation token is properly disposed of after use to avoid memory
    /// leaks.</remarks>
    private CancellationTokenSource? _resizeCts;

    /// <summary>
    /// Handles an asynchronous resize operation and invokes a refresh action after a short delay, canceling any
    /// previous pending resize operations.
    /// </summary>
    /// <remarks>If a new resize event occurs while a previous one is still pending, the previous operation is
    /// canceled. The method introduces a 50-millisecond delay before executing the refresh action, which helps debounce
    /// rapid consecutive resize events.</remarks>
    /// <param name="refresh">A delegate that represents the asynchronous operation to execute after the resize event is debounced.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task HandleResizeAsync(Func<Task> refresh)
    {
        _resizeCts?.Cancel();
        _resizeCts = new CancellationTokenSource();

        var token = _resizeCts.Token;

        try
        {
            await Task.Delay(50, token);
            await refresh();
        }
        catch (TaskCanceledException)
        {
        }
    }
}

