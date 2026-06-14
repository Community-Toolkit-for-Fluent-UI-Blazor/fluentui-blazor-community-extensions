namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of a console export operation, including the exported file's name, content type, and binary
/// content.
/// </summary>
public sealed class ConsoleExportResult
{
    /// <summary>
    /// Gets the name of the file associated with the current instance.
    /// </summary>
    public string FileName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the media type of the associated resource.
    /// </summary>
    /// <remarks>The content type is represented as a string and typically follows the MIME type format, such
    /// as "application/json" or "text/html". This value can be used to specify how the resource should be interpreted
    /// by clients or downstream systems.</remarks>
    public string ContentType { get; init; } = string.Empty;

    /// <summary>
    /// Gets the content as a byte array that represents the data associated with this object.
    /// </summary>
    /// <remarks>The content is initialized to an empty array. This property is read-only and can only be set
    /// during object initialization.</remarks>
    public byte[] Content { get; init; } = [];
}
