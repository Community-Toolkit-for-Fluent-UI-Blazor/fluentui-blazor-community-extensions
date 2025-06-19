namespace FluentUI.Blazor.Community.Components;

public record FileNavigationItem(string? Path, string? Text, Action<string>? OnClick)
{
}
