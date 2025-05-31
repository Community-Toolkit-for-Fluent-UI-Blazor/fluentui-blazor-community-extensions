using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Shared.Providers;

public class FileManagerItemsProvider
    : IFileManagerItemsProvider<NoFileEntryData>
{
    private static async ValueTask<FileManagerEntry<NoFileEntryData>> BuildAsync()
    {
        var entry = FileManagerEntry<NoFileEntryData>.Home;
        var bookEntry = entry.CreateDirectory("Books");
        var documentsEntry = entry.CreateDirectory("Documents");
        var journeyEntry = entry.CreateDirectory("Journey");
        var powerbiEntry = entry.CreateDirectory("Powerbi");


        return await ValueTask.FromResult(entry);
    }

    

    public ValueTask<FileManagerEntry<NoFileEntryData>> GetItemsAsync()
    {
        return BuildAsync();
    }
}
