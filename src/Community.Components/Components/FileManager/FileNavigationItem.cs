namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the navigation bar of the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <param name="Path">Path of the item.</param>
/// <param name="Text">Text of the item.</param>
/// <param name="OnClick">Action to do when the item is clicked.</param>
public record FileNavigationItem(string? Path, string? Text, Action<string>? OnClick)
{
}
