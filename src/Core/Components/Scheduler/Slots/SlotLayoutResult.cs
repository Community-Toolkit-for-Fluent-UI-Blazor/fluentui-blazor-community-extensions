namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of assigning an item to a slot within a scheduler layout, including its position and layout
/// information.
/// </summary>
/// <remarks>Use this class to obtain details about how a scheduler item is positioned within a multi-column
/// layout, such as its assigned column and the total number of columns available. This is typically used in scheduling
/// or calendar applications to manage item placement and rendering.</remarks>
/// <typeparam name="TItem">The type of the item contained within the scheduler slot.</typeparam>
public class SlotLayoutResult<TItem>
{
    /// <summary>
    /// Gets or sets the scheduler item associated with this instance.
    /// </summary>
    public SchedulerItem<TItem> Item { get; set; } = default!;

    /// <summary>
    /// Gets or sets the zero-based index of the column in which the item is placed.
    /// </summary>
    public int ColumnIndex { get; set; }

    /// <summary>
    /// Gets or sets the number of columns in the collection.
    /// </summary>
    public int ColumnCount { get; set; }
}
