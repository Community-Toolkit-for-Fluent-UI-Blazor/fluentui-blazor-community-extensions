using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Rooms;

/// <summary>
/// Represents an item in a chat room list, which can be selected or disabled.
/// It provides visual feedback based on its state and allows for interaction through click events.
/// </summary>
public partial class ChatRoomItem : FluentComponentBase
{
    /// <summary>
    /// Value  indicating whether the mouse is hovering over the component.
    /// </summary>
    private bool _hover;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomItem"/> class with the specified library configuration and
    /// generates a unique identifier.
    /// </summary>
    /// <param name="configuration">The library configuration to use for initializing the chat room item.</param>
    public ChatRoomItem(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is selected.
    /// </summary>
    [Parameter]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets an event callback that is invoked when the component is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the "More" button for additional options related to the chat room item.
    /// </summary>
    [Parameter]
    public bool ShowMoreButton { get; set; }

    private string? CssClass => DefaultClassBuilder
        .AddClass("chat-room-item")
        .AddClass("selected", IsSelected)
        .AddClass("disabled", IsDisabled)
        .AddClass("hover", _hover && !IsDisabled)
        .Build();

    private async Task HandleClick()
    {
        if (IsDisabled)
        {
            return;
        }

        await OnClick.InvokeAsync();
    }
}
