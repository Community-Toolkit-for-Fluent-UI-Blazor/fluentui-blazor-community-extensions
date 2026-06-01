namespace FluentUI.Blazor.Community.Components.Chat.Files;

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
    /// Gets the owner of the file.
    /// </summary>
    long OwnerId { get; }

    /// <summary>
    /// Gets the creation date of the file.
    /// </summary>
    DateTimeOffset CreatedDate { get; }

    /// <summary>
    /// Gets the deletion date of the file, if it has been deleted.
    /// </summary>
    DateTimeOffset? DeletedDate { get; }

    /// <summary>
    /// Gets a value indicating whether the file has been deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the content type of the file.
    /// </summary>
    string ContentType { get; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    string Name { get; }
}
