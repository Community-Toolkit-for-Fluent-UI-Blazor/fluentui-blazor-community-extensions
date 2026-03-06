namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs during a file upload stream operation.
/// </summary>
/// <param name="Index">The zero-based index of the current chunk or segment in the upload stream.</param>
/// <param name="Name">The name of the file being uploaded.</param>
/// <param name="Buffer">The buffer containing the data for the current chunk or segment of the file.</param>
public sealed record UploadStreamEventArgs(int Index, string Name, byte[] Buffer)
{
}

