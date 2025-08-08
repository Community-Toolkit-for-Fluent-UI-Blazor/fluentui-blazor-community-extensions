namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args when a file is selected from the hard drive.
/// </summary>
/// <param name="Name">Name of the file.</param>
/// <param name="ContentType">Content type of the file.</param>
/// <param name="Data">Data of the file.</param>
public record HardDriveFileSelectedEventArgs(string Name, string ContentType, byte[] Data)
{
    /// <summary>
    /// Gets or sets the identifier of the file.
    /// </summary>
    public long Id { get; set; }
}
