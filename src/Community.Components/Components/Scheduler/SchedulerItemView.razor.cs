using System.Drawing;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view component for displaying and interacting with a scheduled item within a scheduler interface.
/// </summary>
/// <remarks>Use this component to render individual scheduler items, customize their appearance via templates,
/// and handle user interactions such as deletion, dragging, resizing, and double-click events. The component exposes
/// parameters for event callbacks and item positioning, allowing integration with various scheduling
/// scenarios.</remarks>
/// <typeparam name="TItem">The type of the data associated with the scheduled item.</typeparam>
public partial class SchedulerItemView<TItem>
{
    /// <summary>
    /// Initializes a new instance of the SchedulerItemView class.
    /// </summary>
    public SchedulerItemView()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the scheduler item associated with this component.
    /// </summary>
    /// <remarks>Use this property to provide the data and configuration for the scheduler item rendered by
    /// the component. The value should be a valid instance of <see cref="SchedulerItem{TItem}"/> representing the item
    /// to display or interact with.</remarks>
    [Parameter]
    public SchedulerItem<TItem> Item { get; set; } = default!;

    /// <summary>
    /// Gets or sets the position and size of the element as a tuple containing the X and Y coordinates, width, and
    /// height.
    /// </summary>
    /// <remarks>The tuple represents (X, Y, Width, Height) in double-precision values. All values are in the
    /// coordinate space relevant to the containing layout or control. Setting this property updates the element's
    /// layout accordingly.</remarks>
    [Parameter]
    public RectangleF Position { get; set; }

    /// <summary>
    /// Gets or sets the template used to render each item in the scheduler.
    /// </summary>
    /// <remarks>The template receives a <see cref="SchedulerItem{TItem}"/> as its context, allowing
    /// customization of how individual items are displayed. If not set, a default rendering will be used.</remarks>
    [Parameter]
    public RenderFragment<SchedulerItem<TItem>>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a resize operation begins on a scheduler item.
    /// </summary>
    /// <remarks>The callback receives a tuple containing the affected scheduler item and the mouse event
    /// arguments associated with the resize start action. Use this event to implement custom logic when a user
    /// initiates resizing of a scheduler item, such as displaying a visual indicator or preparing for
    /// changes.</remarks>
    [Parameter]
    public EventCallback<SchedulerResizeEventArgs<TItem>> OnResizeStarted { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a resize operation on a scheduler item has completed.
    /// </summary>
    /// <remarks>Use this callback to perform custom actions after a user finishes resizing a scheduler item,
    /// such as updating data or triggering additional UI changes. The callback receives the affected scheduler item as
    /// its parameter.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> OnResizeEnded { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a scheduler item is double-clicked.
    /// </summary>
    /// <remarks>The callback receives a tuple containing the scheduler item and the mouse event arguments
    /// associated with the double-click action. Use this property to handle custom logic when users double-click items
    /// in the scheduler component.</remarks>
    [Parameter]
    public EventCallback<Tuple<SchedulerItem<TItem>, MouseEventArgs>> OnDoubleClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component can be closed by the user.
    /// </summary>
    [Parameter]
    public bool CanClose { get; set; } = true;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxScheduler{TItem}"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// rendered within a parent <see cref="FluentCxScheduler{TItem}"/>. Accessing this property allows child components
    /// to interact with or retrieve state from the parent scheduler.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem> Parent { get; set; } = default!;

    /// <summary>
    /// Gets or sets a value indicating whether the scheduler item is currently being resized.
    /// </summary>
    [Parameter]
    public bool IsResizing { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a drag operation starts for a scheduler item.
    /// </summary>
    /// <remarks>The callback receives the scheduler item being dragged as its argument. Use this event to
    /// perform custom logic or update UI state when a drag begins. This parameter is typically set in a parent
    /// component to handle drag initiation.</remarks>
    [Parameter]
    public EventCallback<DragEventArgs> OnDragStarted { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a drag operation for a scheduler item has ended.
    /// </summary>
    /// <remarks>The callback receives the affected scheduler item as its argument. Use this event to perform
    /// actions such as updating item positions or persisting changes after a drag-and-drop operation
    /// completes.</remarks>
    [Parameter]
    public EventCallback<SchedulerItem<TItem>> OnDragEnded { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the top anchor navigation element is displayed.
    /// </summary>
    [Parameter]
    public bool ShowTopAnchor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a bottom anchor is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowBottomAnchor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the left anchor is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowLeftAnchor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the right anchor is displayed in the component.
    /// </summary>
    [Parameter]
    public bool ShowRightAnchor { get; set; }

    /// <summary>
    /// Asynchronously handles the delete operation for the current item by delegating to the parent context, if
    /// available.
    /// </summary>
    /// <remarks>If the parent context is not set, no action is performed. This method should be called when
    /// an item needs to be deleted within a hierarchical context.</remarks>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    private async Task OnDeleteAsync()
    {
        if (Parent is not null)
        {
            await Parent.OnDeleteAsync(Item);
        }
    }

    /// <summary>
    /// Handles the drag start event asynchronously and invokes the drag started callback if it is set.
    /// </summary>
    /// <param name="e">The event data associated with the drag start operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDragStartAsync(DragEventArgs e)
    {
        if (OnDragStarted.HasDelegate)
        {
            await OnDragStarted.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Handles the completion of a drag operation and invokes the associated drag-ended event asynchronously.
    /// </summary>
    /// <param name="e">The event data for the drag operation that has ended.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDragEndedAsync(DragEventArgs e)
    {
        if (OnDragEnded.HasDelegate)
        {
            await OnDragEnded.InvokeAsync(Item);
        }
    }

    /// <summary>
    /// Invokes the double-click event handler asynchronously when a double-click mouse event occurs.
    /// </summary>
    /// <remarks>This method checks whether a double-click event handler is assigned before invoking it. The
    /// event handler receives a tuple containing the associated item and the mouse event arguments.</remarks>
    /// <param name="e">The <see cref="MouseEventArgs"/> instance containing information about the mouse event that triggered the
    /// double-click.</param>
    /// <returns>A task that represents the asynchronous operation of invoking the double-click event handler.</returns>
    private async Task OnDoubleClickAsync(MouseEventArgs e)
    {
        if (OnDoubleClick.HasDelegate)
        {
            await OnDoubleClick.InvokeAsync(Tuple.Create(Item, e));
        }
    }

    /// <summary>
    /// Begins an asynchronous resize operation in response to a mouse event, initializing the resize state and
    /// direction.
    /// </summary>
    /// <param name="e">The mouse event data that triggered the resize operation. Provides the cursor position and event context.</param>
    /// <param name="direction">The direction in which the item will be resized. Determines how the item's bounds will be adjusted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task StartResizeAsync(MouseEventArgs e, ResizeDirection direction)
    {
        if (OnResizeStarted.HasDelegate)
        {
            await OnResizeStarted.InvokeAsync(new(Item, direction, (float)e.ClientX, (float)e.ClientY));
        }
    }
}
