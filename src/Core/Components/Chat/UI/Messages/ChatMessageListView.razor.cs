using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Messages;

/// <summary>
/// Represents a component that displays a list of chat messages in a chat interface.
/// </summary>
public partial class ChatMessageListView
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageListView"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatMessageListView(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }
}
