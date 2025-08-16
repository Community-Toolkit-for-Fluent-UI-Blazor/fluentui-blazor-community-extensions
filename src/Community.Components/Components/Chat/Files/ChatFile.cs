namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a base chat file.
/// </summary>
public abstract class ChatFile
    : IChatFile
{
    /// <summary>
    /// Gets or sets the identifier of the file.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the message the file belongs to.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// Gets or sets the created date.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the content type.
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the length of the file.
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// Gets or sets the owner of the file.
    /// </summary>
    public ChatUser Owner { get; set; } = default!;
}
