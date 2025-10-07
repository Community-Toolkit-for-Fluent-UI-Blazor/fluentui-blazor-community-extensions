using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an observer item which can be used within a <see cref="GroupObserverItem"/> to monitor or manage changes in a grouped collection of components.
/// </summary>
public partial class ObserverItem
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObserverItem"/> class.
    /// </summary>
    public ObserverItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent <see cref="GroupObserverItem"/> component that this observer item is associated with.
    /// </summary>
    [CascadingParameter]
    private GroupObserverItem? Parent { get; set; }

    /// <summary>
    /// Gets or sets the state of the observer, which is used to manage and track the observer's behavior and data.
    /// </summary>
    [Inject]
    private ObserverState ObserverState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback that is invoked when the component is resized.
    /// </summary>
    /// <remarks>Use this property to handle resize events and respond to changes in the component's
    /// dimensions.  The callback is triggered whenever a resize event occurs, providing information such as the new
    /// size.</remarks>
    [Parameter]
    public EventCallback<ResizeEventArgs> OnResize { get; set; }

    /// <summary>
    /// Gets or sets the callback to be invoked when an intersection event occurs.
    /// </summary>
    /// <remarks>The callback is triggered with an <see cref="IntersectEventArgs"/> instance containing
    /// details about the intersection event. Use this property to handle intersection events, such as when an element
    /// enters or exits the viewport.</remarks>
    [Parameter]
    public EventCallback<IntersectEventArgs> OnIntersect { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a mutation event occurs within the component.
    /// </summary>
    /// <remarks>Assign a delegate to handle mutation events, such as changes to the component's data or
    /// state. The event provides details through the <see cref="MutationEventArgs"/> parameter. This callback is
    /// typically used to respond to or process mutations initiated by user actions or other triggers.</remarks>
    [Parameter]
    public EventCallback<MutationEventArgs> OnMutation { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component observes changes to its size and responds to resize
    /// events.
    /// </summary>
    [Parameter]
    public bool ObserveResize { get; set; } = true;

    /// <summary>
    /// Gets or sets the options used to configure the behavior of the mutation observer for this component.
    /// </summary>
    /// <remarks>Specify mutation observer options to control which DOM changes are monitored, such as
    /// attribute modifications, child list changes, or subtree updates. If not set, default observation behavior will
    /// be used.</remarks>
    [Parameter]
    public MutationObserverOptions MutationOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether intersection observation is enabled for the component.
    /// </summary>
    /// <remarks>When enabled, the component will monitor its visibility or intersection with a parent
    /// container, which can be used to trigger actions such as lazy loading or animations. This property is typically
    /// used in scenarios where you need to respond to changes in the component's visibility within the
    /// viewport.</remarks>
    [Parameter]
    public bool ObserveIntersection { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether changes to the component's content should be observed for mutations.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component will monitor its content for dynamic
    /// changes, such as additions or removals of child elements. This can be useful for scenarios where the content is
    /// updated outside of the normal rendering cycle.</remarks>
    [Parameter]
    public bool ObserveMutation { get; set; } = false;

    /// <summary>
    /// Handles the resize event and invokes the associated delegate asynchronously, if one is assigned.
    /// </summary>
    /// <param name="e">The event arguments containing details about the resize event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected internal virtual async Task OnResizeAsync(ResizeEventArgs e)
    {
        if (OnResize.HasDelegate)
        {
            await OnResize.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Handles the intersection event by invoking the associated event callback asynchronously.
    /// </summary>
    /// <remarks>This method checks if the <see cref="OnIntersect"/> event has any registered delegates  and
    /// invokes them asynchronously if present. Ensure that the event arguments provided  are valid and contain the
    /// necessary data for the event handlers.</remarks>
    /// <param name="e">The event arguments containing details about the intersection event.</param>
    /// <returns></returns>
    protected internal virtual async Task OnIntersectAsync(IntersectEventArgs e)
    {
        if (OnIntersect.HasDelegate)
        {
            await OnIntersect.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Handles a mutation event asynchronously when a data change occurs.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom mutation handling logic. The base
    /// implementation throws a <see cref="NotImplementedException"/>.</remarks>
    /// <param name="entry">The event arguments containing details about the mutation to process.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="NotImplementedException">Thrown if the method is not overridden in a derived class.</exception>
    protected internal virtual async Task OnMutationAsync(MutationEventArgs entry)
    {
        if (OnMutation.HasDelegate)
        {
            await OnMutation.InvokeAsync(entry);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (Parent is not null)
            {
                Parent.AddObserverItem(this);
            }
            else
            {
                await ObserverState.AddItemAsync(this);
            }
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        if (Parent is not null)
        {
            Parent.RemoveItem(this);
        }
        else
        {
            await ObserverState.RemoveItemAsync(this);
        }
    }
}
