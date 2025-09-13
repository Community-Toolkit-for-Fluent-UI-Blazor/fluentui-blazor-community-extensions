namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when a signature is exported.
/// </summary>
/// <remarks>This class contains information about the exported signature, including the file name, content type,
/// and binary data.</remarks>
/// <param name="FileName">The name of the file to which the signature is exported. This value cannot be null or empty.</param>
/// <param name="ContentType">The MIME type of the exported signature file, such as "application/pdf" or "image/png".</param>
/// <param name="Data">The binary data of the exported signature. This value cannot be null.</param>
public record class SignatureExportEventArgs(string FileName, string ContentType, byte[] Data)
{
}
