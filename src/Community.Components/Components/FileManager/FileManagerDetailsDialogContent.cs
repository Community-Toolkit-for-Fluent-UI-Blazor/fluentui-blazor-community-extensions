// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public record FileManagerDetailsDialogContent<TItem>(
    FileExtensionTypeLabels FileExtensionTypeLabels,
    IEnumerable<FileManagerEntry<TItem>> Entries) where TItem : class, new()
{
}
