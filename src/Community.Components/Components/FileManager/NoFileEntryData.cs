// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public sealed class NoFileEntryData
    : IDownloadable, IRenamable, IDeletable
{
    public bool IsDownloadAllowed => true;

    public bool IsRenamable => true;

    public bool IsDeleteable => true;
}

