using Microsoft.AspNetCore.StaticFiles;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of an export operation, including the file name, MIME type, and binary data of the exported
/// content.
/// </summary>
/// <param name="FileName">The name of the exported file, including its extension. Cannot be null or empty.</param>
/// <param name="MimeType">The MIME type of the exported content, such as "application/pdf" or "text/csv". Cannot be null or empty.</param>
/// <param name="Data">The binary data of the exported file. Cannot be null.</param>
public sealed record ExportResult(string FileName, string MimeType, byte[] Data)
{
    /// <summary>
    /// 
    /// </summary>
    private static readonly FileExtensionContentTypeProvider _provider = new();

    /// <summary>
    /// Creates an image export result.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="extension">Extension of the file.</param>
    /// <param name="bytes">Content of the file.</param>
    /// <returns>Returns a <see cref="ExportResult"/> instance.</returns>
    internal static ExportResult Create(
        string? fileName,
        string extension,
        byte[] bytes)
    {
        if (!extension.StartsWith('.'))
        {
            extension = $".{extension}";
        }

        var finalFileName = !string.IsNullOrEmpty(fileName)
            ? Path.ChangeExtension(fileName, extension)
            : $"signature{extension}";

        var mime = _provider.TryGetContentType(extension, out var ct)
            ? ct
            : "application/octet-stream";

        return new ExportResult(
            finalFileName,
            mime,
            bytes);
    }
}

