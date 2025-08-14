using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the chat room rename dialog.
/// </summary>
public partial class ChatRoomRenameDialog
{
    /// <summary>
    /// Gets or sets the content of the room.
    /// </summary>
    [Parameter]
    public ChatRoomContent Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the reference to the dialog.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Occurs when the dialog is closed.
    /// </summary>
    /// <returns>Returns a task which closes the dialog when completed.</returns>
    private async Task OnCloseAsync()
    {
        await Dialog.CloseAsync(Content.Name);
    }

    /// <summary>
    /// Occurs when the dialog is cancelled.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with cancel result when completed.</returns>
    private async Task OnCancelAsync()
    {
        await Dialog.CancelAsync();
    }
}
