using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Audio;

/// <summary>
/// Represents a component that handles audio processing for chat functionality.
/// </summary>
public partial class ChatAudioProcessing
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatAudioProcessing"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatAudioProcessing(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }
}
