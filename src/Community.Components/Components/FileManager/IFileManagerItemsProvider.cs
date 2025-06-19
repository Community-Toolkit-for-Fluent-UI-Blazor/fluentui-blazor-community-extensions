namespace FluentUI.Blazor.Community.Components;

public interface IFileManagerItemsProvider<TItem> where TItem : class, new()
{
    ValueTask<FileManagerEntry<TItem>> GetItemsAsync();
}

