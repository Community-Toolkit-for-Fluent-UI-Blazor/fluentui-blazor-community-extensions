using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.Room;

/// <summary>
/// Represents an action that can be performed on a chat room.
/// </summary>
public sealed class ChatRoomAction
{
    /// <summary>
    /// Gets the label of the action.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Gets the icon associated with the action.
    /// </summary>
    public Icon? Icon { get; init; }

    /// <summary>
    /// Gets the function to execute when the action is invoked.
    /// </summary>
    public Func<Task> Action { get; init; } = default!;

    /// <summary>
    /// Gets a value indicating whether this action is a separator.
    /// </summary>
    public bool IsSeparator { get; init; }

    /// <summary>
    /// Gets a predefined instance of <see cref="ChatRoomAction"/> that represents a separator in the list of actions.
    /// </summary>
    public static ChatRoomAction Separator => new() { IsSeparator = true };
}

