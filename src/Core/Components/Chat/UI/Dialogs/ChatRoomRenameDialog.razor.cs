using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Represents the dialog to create or rename a chat room.
/// </summary>
public partial class ChatRoomRenameDialog
{
    /// <summary>
    /// Gets or sets the dialog instance from the parent component.
    /// </summary>
    [CascadingParameter]
    private IDialogInstance Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the value of the rename.
    /// </summary>
    [Parameter]
    public string Value { get; set; } = string.Empty;

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await Dialog.CloseAsync(Value);
        }
        else
        {
            await Dialog.CancelAsync();
        }
    }
}
