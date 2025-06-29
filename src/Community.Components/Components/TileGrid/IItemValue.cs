namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a value for an item.
/// </summary>
/// <typeparam name="TItem">Type of the value.</typeparam>
public interface IItemValue<TItem>
{
    /// <summary>
    /// Gets the value of the item.
    /// </summary>
    TItem? Value { get; }
}
