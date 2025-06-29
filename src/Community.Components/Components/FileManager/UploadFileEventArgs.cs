namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args for an uploaded file.
/// </summary>
/// <param name="Name">Name of the file.</param>
/// <param name="Data">Data of the file.</param>
/// <param name="Extension">Extension of the file.</param>
public record UploadFileEventArgs(string Name, byte[] Data, string Extension);
