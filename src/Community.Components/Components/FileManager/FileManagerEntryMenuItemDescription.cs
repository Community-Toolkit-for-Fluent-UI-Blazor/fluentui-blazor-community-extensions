using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public record FileManagerEntryMenuItemDescription<TItem> where TItem : class, new()
{
    public Func<FileManagerEntry<TItem>, Task>? OnClick { get; set; }

    public Icon? Icon { get; set; }

    public string? Label { get; set; }
}
