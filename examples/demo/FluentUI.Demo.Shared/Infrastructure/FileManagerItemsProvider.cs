using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Shared.Infrastructure;

internal class FileManagerItemsProvider
    : IFileManagerItemsProvider<NoFileEntryData>
{
    public ValueTask<FileManagerEntry<NoFileEntryData>> GetItemsAsync()
    {
        var entry = FileManagerEntry<NoFileEntryData>.Home;
        return ValueTask.FromResult(entry);
    }
}
