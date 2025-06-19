// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Components;

namespace FluentUI.Demo.Shared.Providers;

public class FileManagerItemsProvider
    : IFileManagerItemsProvider<NoFileEntryData>
{
    public ValueTask<FileManagerEntry<NoFileEntryData>> GetItemsAsync()
    {
        var entry = FileManagerEntry<NoFileEntryData>.Home;
        return ValueTask.FromResult(entry);
    }
}
