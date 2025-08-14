namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a section of a message.
/// </summary>
public record ChatMessageSection
    : IChatMessageSection
{
    /// <summary>
    /// Gets or sets the identifier of the section.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the message.
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// Gets or sets the culture of the section.
    /// </summary>
    public long CultureId { get; set; }

    /// <summary>
    /// Gets or sets the content of the section.
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Gets or sets the created date of the section.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}
