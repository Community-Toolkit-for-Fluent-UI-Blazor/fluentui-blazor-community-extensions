using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a mechanism for managing and observing changes in Fluent components.
/// </summary>
/// <remarks>This class serves as a base for implementing functionality related to observing and reacting to
/// changes in Fluent components. It is sealed and cannot be inherited.</remarks>
public sealed partial class FluentCxObserverProvider
    : FluentComponentBase, IIntersectObserver, IResizeObserver, IMutationObserver, IAsyncDisposable
{
    /// <summary>
    /// Represents a reference to a .NET object that is used to interact with the  <see
    /// cref="FluentCxObserverProvider"/> instance from JavaScript in a Blazor application.
    /// </summary>
    /// <remarks>This field holds a <see cref="DotNetObjectReference{T}"/> instance, which allows the  <see
    /// cref="FluentCxObserverProvider"/> to be passed to JavaScript and invoked from there. It is intended for internal
    /// use and should not be accessed directly by external code.</remarks>
    private readonly DotNetObjectReference<FluentCxObserverProvider> _dotNetRef;

    /// <summary>
    /// Indicates whether the object has been rendered.
    /// </summary>
    /// <remarks>This field is used to track the rendering state of the object.  It is intended for internal
    /// use and should not be accessed directly.</remarks>
    private bool _hasModule;

    /// <summary>
    /// A thread-safe queue used to store pending observer items for processing.
    /// </summary>
    /// <remarks>This queue ensures safe concurrent access and is used to manage items that are awaiting
    /// processing in a multi-threaded environment.</remarks>
    private readonly ConcurrentQueue<ObserverItem> _pendingItems = new();

    /// <summary>
    /// A thread-safe queue that holds pending group observer items to be processed.
    /// </summary>
    /// <remarks>This queue is used to manage and store items of type <see cref="GroupObserverItem"/> that are
    /// awaiting processing. The use of <see cref="ConcurrentQueue{T}"/> ensures thread-safe operations for adding and
    /// removing items in a concurrent environment.</remarks>
    private readonly ConcurrentQueue<GroupObserverItem> _pendingGroups = new();

    /// <summary>
    /// Represents a reference to a JavaScript module that is used for interop operations.
    /// </summary>
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// The relative path to the JavaScript module used by the FluentCxObserverProvider component.
    /// </summary>
    /// <remarks>This path is used to locate the JavaScript file that contains the implementation for the
    /// FluentCxObserverProvider. Ensure that the file exists at the specified location and is correctly deployed as
    /// part of the application.</remarks>
    private const string JsModulePath = "./_content/FluentUI.Blazor.Community.Components/components/observers/FluentCxObserverProvider.razor.js";

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxObserverProvider"/> class.
    /// </summary>
    public FluentCxObserverProvider()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the observer state used to manage and track the state of observed elements.
    /// </summary>
    [Inject]
    private ObserverState ObserverState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the debounce interval for resize events.
    /// </summary>
    [Parameter]
    public TimeSpan DebounceResize { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets the debounce duration for handling intersection events.
    /// </summary>
    [Parameter]
    public TimeSpan DebounceIntersect { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets the threshold for intersection observation.
    /// </summary>
    [Parameter]
    public double Threshold { get; set; } = 0.3;

    /// <summary>
    /// Gets or sets the JavaScript runtime abstraction used to invoke JavaScript functions from .NET.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Notifies the system of changes to the specified group by registering intersection      and resize observers for
    /// each item in the group.
    /// </summary>
    /// <remarks>This method performs no action if the component has not been rendered.      For each item in
    /// the group, intersection and resize observers are registered      to monitor changes related to the item's
    /// associated element.</remarks>
    /// <param name="group">The group containing the items to be observed.      Each item must have a valid identifier and associated
    /// element.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    internal async ValueTask NotifyAsync(GroupObserverItem group)
    {
        if (_hasModule)
        {
            foreach (var item in group.Items)
            {
                if (item.ObserveIntersection)
                {
                    await RegisterIntersectAsync(group.GroupId, item.Id!, item.Element);
                }

                if (item.ObserveResize)
                {
                    await RegisterResizeAsync(group.GroupId, item.Id!, item.Element);
                }

                if (item.ObserveMutation)
                {
                    await RegisterMutationAsync(group.GroupId, item.Id!, item.Element, item.MutationOptions);
                }
            }
        }
        else
        {
            _pendingGroups.Enqueue(group);
        }
    }

    /// <summary>
    /// Notifies the system to register intersection and resize observers for the specified item.
    /// </summary>
    /// <remarks>This method registers intersection and resize observers for the provided item if the system
    /// is in a rendered state. The <paramref name="item"/> must have a non-null identifier and element for the
    /// operation to succeed.</remarks>
    /// <param name="item">The <see cref="ObserverItem"/> containing the identifier and element to be observed.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    internal async ValueTask NotifyAsync(ObserverItem item)
    {
        if (_hasModule)
        {
            if (item.ObserveIntersection)
            {
                await RegisterIntersectAsync(item.Id!, item.Element);
            }

            if (item.ObserveResize)
            {
                await RegisterResizeAsync(item.Id!, item.Element);
            }
        }
        else
        {
            _pendingItems.Enqueue(item);
        }
    }

    /// <summary>
    /// Asynchronously removes notifications for all items in the specified group.
    /// </summary>
    /// <remarks>This method unregisters intersection and resize notifications for each item in the group.  It
    /// has no effect if the group does not exist or if the component has not been rendered.</remarks>
    /// <param name="groupId">The unique identifier of the group whose notifications are to be removed. Cannot be <see langword="null"/> or
    /// empty.</param>
    /// <returns></returns>
    internal async ValueTask UnnotifyGroupAsync(string groupId)
    {
        if (_hasModule)
        {
            var items = ObserverState.GetItems(groupId);

            foreach (var item in items)
            {
                await UnregisterIntersectAsync(groupId, item.Id!);
                await UnregisterResizeAsync(groupId, item.Id!);
            }
        }
    }

    /// <summary>
    /// Removes notifications for the specified item, including intersection and resize event tracking, if the component
    /// is rendered.
    /// </summary>
    /// <param name="id">The unique identifier of the item to unnotify. Cannot be <see langword="null"/> or empty.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    internal async ValueTask UnnotifyItemAsync(string id)
    {
        if (_hasModule)
        {
            await UnregisterIntersectAsync(id, id);
            await UnregisterResizeAsync(id, id);
        }
    }

    /// <inheritdoc />
    public async Task RegisterIntersectAsync(string groupId, string elementId, ElementReference elementReference)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.registerIntersect", groupId, elementId, elementReference, _dotNetRef, new { debounce = (int)DebounceIntersect.TotalMilliseconds, threshold = Threshold });
        }
    }

    /// <inheritdoc />
    public Task RegisterIntersectAsync(string elementId, ElementReference elementReference)
    {
        return RegisterIntersectAsync(elementId, elementId, elementReference);
    }

    /// <inheritdoc />
    public async Task RegisterResizeAsync(string groupId, string elementId, ElementReference elementReference)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.registerResize", groupId, elementId, elementReference, _dotNetRef, new { debounce = (int)DebounceResize.TotalMilliseconds });
        }
    }

    /// <inheritdoc />
    public Task RegisterResizeAsync(string elementId, ElementReference elementReference)
    {
        return RegisterResizeAsync(elementId, elementId, elementReference);
    }

    /// <inheritdoc />
    public Task UnregisterIntersectAsync(string elementId)
    {
        return UnregisterIntersectAsync(elementId, elementId);
    }

    /// <inheritdoc />
    public async Task UnregisterIntersectAsync(string groupId, string elementId)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.unregisterIntersect", groupId, elementId);
        }
    }

    /// <inheritdoc />
    public Task UnregisterResizeAsync(string elementId)
    {
        return UnregisterResizeAsync(elementId, elementId);
    }

    /// <inheritdoc />
    public async Task UnregisterResizeAsync(string groupId, string elementId)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.unregisterResize", groupId, elementId);
        }
    }

    /// <inheritdoc />
    public async Task RegisterMutationAsync(string groupId, string elementId, ElementReference elementReference, MutationObserverOptions options)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.registerMutation", groupId, elementId, elementReference, options);
        }
    }

    /// <inheritdoc />
    public async Task UnregisterMutationAsync(string groupId, string elementId)
    {
        if (_hasModule)
        {
            await _jsModule!.InvokeVoidAsync("fluentCxObserverProvider.unregisterMutation", groupId, elementId);
        }
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ObserverState.SetProviderAsync(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath);
            _hasModule = _jsModule is not null;

            while (_pendingItems.TryDequeue(out var item))
            {
                await NotifyAsync(item);
            }

            while (_pendingGroups.TryDequeue(out var group))
            {
                await NotifyAsync(group);
            }
        }
    }

    /// <summary>
    /// Processes a batch of intersection events and notifies registered observers for each intersected element.
    /// </summary>
    /// <remarks>This method is intended to be invoked from JavaScript via interop. Each entry in the batch is
    /// dispatched to the corresponding observer if it is registered. No notification occurs for elements without a
    /// registered observer.</remarks>
    /// <param name="batch">The batch of intersection entries to process. Must not be null.</param>
    /// <returns>A task that represents the asynchronous operation of notifying observers for each intersection entry.</returns>
    [JSInvokable("OnIntersectBatch")]
    public async Task OnIntersectBatchAsync(IntersectBatch batch)
    {
        var entries = batch.Entries.Select(x => new IntersectEventArgs()
        {
            ElementId = x.Id,
            GroupId = batch.GroupId,
            IsIntersecting = x.IsIntersecting,
            IntersectionRatio = x.IntersectionRatio,
            BoundingClientRect = x.BoundingClientRect,
            IntersectionRect = x.IntersectionRect,
            RootBounds = x.RootBounds
        });

        foreach (var entry in entries)
        {
            var item = ObserverState.GetItem(entry.GroupId, entry.ElementId);

            if (item is not null)
            {
                await item.OnIntersectAsync(entry);
            }
        }
    }

    /// <summary>
    /// Processes a batch of DOM mutation events received from JavaScript and dispatches them to registered observers
    /// asynchronously.
    /// </summary>
    /// <remarks>This method is intended to be invoked from JavaScript via interop. Each mutation entry in the
    /// batch is delivered to the corresponding observer based on its group and identifier. If no observer is registered
    /// for a given entry, that entry is ignored.</remarks>
    /// <param name="batch">The batch of mutation events to process. Must not be null. Contains the collection of mutation entries and the
    /// associated group identifier.</param>
    /// <returns>A task that represents the asynchronous operation of dispatching mutation events to observers.</returns>
    [JSInvokable("OnMutationBatch")]
    public async Task OnMutationBatchAsync(MutationBatch batch)
    {
        var entries = batch.Entries.Select(x => new MutationEventArgs()
        {
            Id = x.Id,
            AddedNodes = x.AddedNodes,
            RemovedNodes = x.RemovedNodes,
            AttributeName = x.AttributeName,
            OldValue = x.OldValue,
            Type = x.Type,
            GroupId = batch.GroupId
        });

        foreach (var entry in entries)
        {
            var item = ObserverState.GetItem(entry.GroupId, entry.Id);

            if (item is not null)
            {
                await item.OnMutationAsync(entry);
            }
        }
    }

    /// <summary>
    /// Processes a batch of resize events and asynchronously notifies observers for each affected element.
    /// </summary>
    /// <remarks>This method is intended to be invoked from JavaScript via interop. Observers are notified
    /// only for elements that are currently registered in the specified group.</remarks>
    /// <param name="batch">The batch of resize event data containing the group identifier and a collection of entries representing elements
    /// that have been resized.</param>
    /// <returns>A task that represents the asynchronous operation of notifying observers for each resize event.</returns>
    [JSInvokable("OnResizeBatch")]
    public async Task OnResizeBatchAsync(ResizeBatch batch)
    {
        var entries = batch.Entries.Select(x => new ResizeEventArgs()
        {
            ElementId = x.Id,
            Width = x.Width,
            Height = x.Height,
            GroupId = batch.GroupId,
            Rect = x.Rect
        });

        foreach (var entry in entries)
        {
            var item = ObserverState.GetItem(batch.GroupId, entry.ElementId);

            if (item is not null)
            {
                await item.OnResizeAsync(entry);
            }
        }
    }

    /// <summary>
    /// Invokes window resize event handlers for all observers that are subscribed to window resize notifications.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop when a window resize event
    /// occurs. Only observers with window resize observation enabled will be notified. The method completes when all
    /// observer handlers have finished processing the event.</remarks>
    /// <param name="e">An object containing details about the window resize event, such as the new dimensions.</param>
    /// <returns>A task that represents the asynchronous operation of notifying all relevant observers of the window resize
    /// event.</returns>
    [JSInvokable("OnWindowResize")]
    public async Task OnWindowResizeAsync(ResizeWindowEventArgs e)
    {
        var tasks = new List<Task>();

        foreach (var item in ObserverState.GetAllItems())
        {
            if (item.ObserveWindowResize)
            {
                tasks.Add(item.OnWindowResizeAsync(e));
            }
        }

        await Task.WhenAll(tasks);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            await ObserverState.SetProviderAsync(null);
        }
        catch (JSDisconnectedException)
        {

        }
    }
}

