using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents a dialog component that allows users to select emojis.
/// </summary>
public partial class EmojiPickerDialog
{
    /// <summary>
    /// Gets or sets the dialog instance that manages the state and behavior of the emoji picker dialog.
    /// </summary>
    [CascadingParameter]
    public required IDialogInstance Dialog { get; set; }

    /// <summary>
    /// Gets or sets the emoji font provider.
    /// </summary>
    [Parameter]
    public IEmojiFontProvider? FontProvider { get; set; } = new MicrosoftEmojiProvider();

    /// <summary>
    /// Gets or sets the number of emojis to display per row in the emoji picker dialog.
    /// </summary>
    [Parameter]
    public int EmojisPerRow { get; set; } = 8;

    /// <summary>
    /// Handles the emoji selection change and closes the dialog with the selected emoji.
    /// </summary>
    /// <param name="emoji">The selected emoji.</param>
    /// <returns>A task that represents the asynchronous operation of closing the dialog.</returns>
    private async Task OnEmojiChangedAsync(FluentCxEmoji emoji)
    {
        await Dialog.CloseAsync(emoji);
    }
}
