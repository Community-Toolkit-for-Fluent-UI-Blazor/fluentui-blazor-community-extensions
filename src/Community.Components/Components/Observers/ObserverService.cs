using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a service for managing observers that monitor changes to elements in the DOM.
/// </summary>
public sealed class ObserverService
    : IDisposable
{
    /// <summary>
    /// Value indicating whether the service has been initialized.
    /// </summary>
    private bool _isInitialized;

    /// <summary>
    /// Reference to the JavaScript module for observer functionality.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Reference ID for the provider associated with this service.
    /// </summary>
    private string? _providerId;

    /// <summary>
    /// Gets a dictionary of observers managed by the service.
    /// </summary>
    internal Dictionary<string, IObserver> Observers { get; } = [];

    /// <summary>
    /// Adds an observer to the service and initializes it if the service is already initialized.
    /// </summary>
    /// <param name="observer">Observer to add into the service.</param>
    /// <returns>Returns a task which adds the observer into the service when completed.</returns>
    /// <exception cref="ArgumentException">Occurs if <see cref="IObserver.Id"/> is not provided.</exception>
    public async Task AddObserverAsync(IObserver observer)
    {
        if (string.IsNullOrEmpty(observer.Id))
        {
            throw new ArgumentException("Observer Id cannot be null or empty.", nameof(observer));
        }

        await WaitInitializationCompleted();

        Observers.TryAdd(observer.Id, observer);

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("addObserver", _providerId, observer.Id);
            await _module.InvokeVoidAsync("measureElement", _providerId, observer.Id);
        }
    }

    /// <summary>
    /// Removes an observer to the service and initializes it if the service is already initialized.
    /// </summary>
    /// <param name="observer">Observer to remove from the service.</param>
    /// <returns>Returns a task which removes the observer from the service when completed.</returns>
    /// <exception cref="ArgumentException">Occurs if <see cref="IObserver.Id"/> is not provided.</exception>
    public async Task RemoveObserverAsync(IObserver observer)
    {
        if (string.IsNullOrEmpty(observer.Id))
        {
            throw new ArgumentException("Observer Id cannot be null or empty.", nameof(observer));
        }

        await WaitInitializationCompleted();

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("removeObserver", _providerId, observer.Id);
        }
    }

    /// <summary>
    /// Waits until the service is initialized.
    /// </summary>
    /// <returns>Returns a task which waits until the service is initialized.</returns>
    private async Task WaitInitializationCompleted()
    {
        while (!_isInitialized)
        {
            await Task.Delay(50);
        }
    }

    /// <summary>
    /// Initializes the instance with the specified provider ID and JavaScript module reference.
    /// </summary>
    /// <param name="id">The unique identifier for the provider. Cannot be null or empty.</param>
    /// <param name="module">The JavaScript module reference to associate with the instance. Cannot be null.</param>
    internal void Initialize(string? id, IJSObjectReference? module)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentException.ThrowIfNullOrEmpty(id);

        _providerId = id;
        _module = module;
        _isInitialized = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _module = null;
        _isInitialized = false;
    }
}
