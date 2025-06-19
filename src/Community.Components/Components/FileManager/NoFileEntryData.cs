namespace FluentUI.Blazor.Community.Components;

public sealed class NoFileEntryData
    : IDownloadable, IRenamable, IDeletable
{
    public bool IsDownloadAllowed => true;

    public bool IsRenamable => true;

    public bool IsDeleteable => true;
}

