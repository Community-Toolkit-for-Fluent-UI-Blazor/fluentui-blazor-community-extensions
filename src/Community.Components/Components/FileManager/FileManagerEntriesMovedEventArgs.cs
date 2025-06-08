// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public record FileManagerEntriesMovedEventArgs<TItem>(
    FileManagerEntry<TItem> DestinationFolder,
    IEnumerable<FileManagerEntry<TItem>> MovedEntries) where TItem : class, new()
{
}
