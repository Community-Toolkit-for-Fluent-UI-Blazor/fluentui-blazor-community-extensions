using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class ChatMessageViewer
    : IDialogContentComponent<ChatMessageViewerContent>
{
    [Parameter]
    public ChatMessageViewerContent Content { get; set; } = default!;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    private async Task OnCloseAsync()
    {
        await Dialog.CloseAsync();
    }
}
