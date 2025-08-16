using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatAudioProcessing
    : FluentComponentBase
{
    [Parameter]
    public string AudioProcessingLabel { get; set; }
}
