namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for the event that occurs when a scheduler item is resized.
/// </summary>
/// <typeparam name="TItem">The type of the data associated with the scheduler item.</typeparam>
/// <param name="Item">The scheduler item that is being resized.</param>
/// <param name="Direction">The direction in which the item is being resized.</param>
/// <param name="X">The horizontal position, in pixels, where the resize event occurred.</param>
/// <param name="Y">The vertical position, in pixels, where the resize event occurred.</param>
public record SchedulerResizeEventArgs<TItem>(SchedulerItem<TItem> Item, ResizeDirection Direction, float X, float Y)
{
}
