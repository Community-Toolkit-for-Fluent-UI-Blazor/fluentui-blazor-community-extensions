namespace FluentUI.Blazor.Community.Components.Chat.Messages;

/// <summary>
/// Represents a section of a message.
/// </summary>
public record ChatMessageSection
    : IChatMessageSection
{
    /// <inheritdoc />
    public long Id { get; set; }

    /// <inheritdoc />
    public long MessageId { get; set; }

    /// <inheritdoc />
    public long CultureId { get; set; }

    /// <inheritdoc />
    public string? Content { get; set; }

    /// <inheritdoc />
    public DateTime CreatedDate { get; set; }
}
