namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a section of a message.
/// </summary>
public interface IChatMessageSection
{
    /// <summary>
    /// Gets the identifier of the section.
    /// </summary>
    long Id { get; }

    /// <summary>
    /// Gets the identifier of the message.
    /// </summary>
    long MessageId { get; }

    /// <summary>
    /// Gets the identifier of the culture.
    /// </summary>
    long CultureId { get; }

    /// <summary>
    /// Gets or sets the content of the section.
    /// </summary>
    string? Content { get; }

    /// <summary>
    /// Gets the created date of the section.
    /// </summary>
    DateTime CreatedDate { get; }
}
