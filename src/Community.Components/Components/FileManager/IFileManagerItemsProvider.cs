// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public interface IFileManagerItemsProvider<TItem> where TItem : class, new()
{
    ValueTask<FileManagerEntry<TItem>> GetItemsAsync();
}

