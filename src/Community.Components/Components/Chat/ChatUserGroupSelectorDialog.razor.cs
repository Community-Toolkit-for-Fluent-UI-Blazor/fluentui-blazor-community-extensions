using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the dialog to select users for a chat group.
/// </summary>
public partial class ChatUserGroupSelectorDialog
    : FluentComponentBase, IDialogContentComponent<ChatUserGroupSelectorContent>
{
    /// <summary>
    /// Represents the <see cref="FluentAutocomplete{TOption}"/>.
    /// </summary>
    private FluentAutocomplete<ChatUser>? _userListRef;

    /// <summary>
    /// Represents the selected users.
    /// </summary>
    private IEnumerable<ChatUser>? _selectedItems;

    /// <summary>
    /// Gets or sets the dialog reference.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    [Parameter]
    public ChatUserGroupSelectorContent Content { get; set; } = default!;

    /// <summary>
    /// Occurs to search the user in an asynchronous way.
    /// </summary>
    /// <param name="e">Event args associated to the methods.</param>
    /// <returns>Returns a task which find the users when completed.</returns>
    private async Task OnSearchAsync(OptionsSearchEventArgs<ChatUser> e)
    {
        e.Items = await Content.OnSearchFunction(e.Text);
    }

    /// <summary>
    /// Dismiss the specified user from the list.
    /// </summary>
    /// <param name="user">User to dismiss.</param>
    /// <returns>Returns a task which remove the <paramref name="user"/> when completed.</returns>
    private async Task OnDismissAsync(ChatUser user)
    {
        if (_userListRef != null)
        {
            await _userListRef!.RemoveSelectedItemAsync(user);
        }
    }

    /// <summary>
    /// Close the dialog with the selected users.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with the selected users when completed.</returns>
    private async Task OnValidateAsync()
    {
        await Dialog.CloseAsync(_selectedItems);
    }

    /// <summary>
    /// Close the dialog.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with a cancel result when completed.</returns>
    private async Task OnCloseAsync()
    {
        await Dialog.CancelAsync();
    }
}
