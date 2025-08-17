using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a slideshow for a chat message. 
/// </summary>
public partial class ChatMessageSlideShow
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageSlideShow"/> class.
    /// </summary>
    public ChatMessageSlideShow()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the owner of the view.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the message to render.
    /// </summary>
    [Parameter]
    public IChatMessage? Message { get; set; }

    /// <summary>
    /// Gets or sets the loading label.
    /// </summary>
    [Parameter]
    public string? LoadingLabel { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the controls and the indicator are visible.
    /// </summary>
    private bool ShowControlsAndIndicators => Message is not null && (Message.Sections.Count > 0 ? (Message.Files.Count > 0 ? true : false) :
                                              Message.Files.Count > 1);
}
