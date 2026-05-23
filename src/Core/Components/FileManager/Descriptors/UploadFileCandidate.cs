namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a file selected for upload, including its index, name, and size.
/// </summary>
public sealed class UploadFileCandidate
{
    /// <summary>
    /// Gets the zero-based index associated with this instance.
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Gets the size of the item in bytes.
    /// </summary>
    public long Size { get; init; }
}

