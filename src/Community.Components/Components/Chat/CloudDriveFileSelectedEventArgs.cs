namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args when a file is selected on the cloud drive.
/// </summary>
/// <param name="Id">Identifier of the file.</param>
/// <param name="Name">Name of the file.</param>
/// <param name="ContentType">Content type of the file.</param>
/// <param name="Data">Data of the file.</param>
public record CloudDriveFileSelectedEventArgs(string Id, string Name, string ContentType, Func<Task<byte[]>> Data)
{
}
