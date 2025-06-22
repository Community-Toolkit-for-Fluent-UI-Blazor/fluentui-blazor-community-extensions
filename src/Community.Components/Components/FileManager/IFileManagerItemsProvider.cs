namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the items provider of the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public interface IFileManagerItemsProvider<TItem> where TItem : class, new()
{
    /// <summary>
    /// Gets the items of the <see cref="FluentCxFileManager{TItem}"/> in an asynchronous way.
    /// </summary>
    /// <returns>Returns a <see cref="ValueTask{TResult}"/> which contains the items.</returns>
    ValueTask<FileManagerEntry<TItem>> GetItemsAsync();
}

