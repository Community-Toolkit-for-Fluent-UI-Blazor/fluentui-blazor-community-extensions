using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatMessageSlideShow
    : FluentComponentBase
{
    public ChatMessageSlideShow()
    {
        Id = Identifier.NewId();
    }

    [Parameter]
    public ChatUser? Owner { get; set; }

    [Parameter]
    public IChatMessage? Message { get; set; }
}
