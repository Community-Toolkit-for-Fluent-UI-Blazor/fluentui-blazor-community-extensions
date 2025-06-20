namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an entry which no data.
/// </summary>
public sealed class NoFileEntryData
    : IDownloadable, IRenamable, IDeletable
{
    /// <inheritdoc />
    public bool IsDownloadAllowed => true;

    /// <inheritdoc />
    public bool IsRenamable => true;

    /// <inheritdoc />
    public bool IsDeleteable => true;
}

