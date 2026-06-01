namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a section of a message.
/// </summary>
public record ChatMessageSection
{
    /// <summary>
    /// Gets or sets the unique identifier of the section.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the message that the section belongs to.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the culture.
    /// </summary>
    public long CultureId { get; set; }

    /// <summary>
    /// Gets or sets the name of the culture.
    /// </summary>
    public string? CultureName { get; set; }

    /// <summary>
    /// Gets or sets the content of the section.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the date when the section was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}
