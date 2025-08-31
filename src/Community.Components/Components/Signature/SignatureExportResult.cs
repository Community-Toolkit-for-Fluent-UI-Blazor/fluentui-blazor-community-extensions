namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of exporting a signature.
/// </summary>
/// <param name="FileName">Name of the file.</param>
/// <param name="Data">Raw data of the file.</param>
/// <param name="ContentType">Content type of the file.</param>
/// <param name="Format">Format of the image.</param>
public record SignatureExportResult(
    string FileName,
    byte[] Data,
    string ContentType,
    SignatureExportFormat Format)
{
}
