using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a reply message.
/// </summary>
public partial class ChatMessageReply
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageReply"/>.
    /// </summary>
    /// <param name="libraryConfiguration">The library configuration.</param>
    public ChatMessageReply(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the css to use.
    /// </summary>
    private string? ClassValue => DefaultClassBuilder
        .AddClass("chat-reply")
        .AddClass("chat-reply-in-card", InsideCard)
        .Build();

    /// <summary>
    /// Gets or sets the text of the reply.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the reply can be dismissed.
    /// </summary>
    [Parameter]
    public bool AllowDismiss { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the reply is dismissed.
    /// </summary>
    [Parameter]
    public EventCallback OnDismiss { get; set; }

    /// <summary>
    /// Gets or sets if the reply is inside the card.
    /// </summary>
    [Parameter]
    public bool InsideCard { get; set; }

    /// <summary>
    /// Occurs when the reply is dismissed.
    /// </summary>
    /// <returns>Returns a task which dismiss the reply when completed.</returns>
    private async Task OnDismissAsync()
    {
        if (OnDismiss.HasDelegate)
        {
            await OnDismiss.InvokeAsync();
        }
    }
}
