using System.ComponentModel.DataAnnotations;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content on the dialog for renaming a chat room.
/// </summary>
public record ChatRoomContent
{
    /// <summary>
    /// Gets or sets the label on the text field.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Gets or sets the placeholder on the text field.
    /// </summary>
    public string? Placeholder { get; init; }

    /// <summary>
    /// Gets or sets the name of the room.
    /// </summary>
    [Required]
    [MinLength(3)]
    public string? Name { get; set; }
}
