namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an event args for a chat file.
/// </summary>
/// <param name="Id">Id of the file.</param>
/// <param name="Name">Name of the file.</param>
/// <param name="ContentType">Content type of the file.</param>
/// <param name="Data">Data of the file.</param>
public record ChatFileEventArgs(string Id, string Name, string ContentType, Func<Task<byte[]>> Data)
{
}
