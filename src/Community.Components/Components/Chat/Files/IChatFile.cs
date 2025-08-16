namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interface for a chat file.
/// </summary>
public interface IChatFile
{
    /// <summary>
    /// Gets the identifier of the file.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets the identifier of the message the file belongs to.
    /// </summary>
    long MessageId { get; }

    /// <summary>
    /// Gets the creation date of the file.
    /// </summary>
    DateTime CreatedDate { get; }

    /// <summary>
    /// Gets the content type of the file.
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the length of the file.
    /// </summary>
    long Length { get; }

    /// <summary>
    /// Gets the owner of the file.
    /// </summary>
    ChatUser Owner { get; }
}
