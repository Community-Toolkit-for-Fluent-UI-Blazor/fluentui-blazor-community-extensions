using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Dialogs;

/// <summary>
/// Represents the dialog to select users for a chat group.
/// </summary>
public partial class ChatUserGroupSelectorDialog
{
    /// <summary>
    /// Represents the selected users.
    /// </summary>
    private IEnumerable<ChatUser> _selectedItems = [];

    /// <summary>
    /// Gets or sets the dialog instance from the parent component.
    /// </summary>
    [CascadingParameter]
    private IDialogInstance Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the function to search users.
    /// </summary>
    [Parameter]
    public Func<string, StringComparison, CancellationToken, Task<IEnumerable<ChatUser>>> OnSearchProvider { get; set; } = default!;

    /// <summary>
    /// Gets or sets the string comparison to use when searching users.
    /// </summary>
    [Parameter]
    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// Gets or sets the owner of the chat group, which is used to exclude the owner from the search results.
    /// </summary>
    [Parameter]
    public ChatUser Owner { get; set; } = default!;

    /// <summary>
    /// Occurs to search the user in an asynchronous way.
    /// </summary>
    /// <param name="e">Event args associated to the methods.</param>
    /// <returns>Returns a task which find the users when completed.</returns>
    private async Task OnSearchAsync(OptionsSearchEventArgs<ChatUser> e)
    {
        if (OnSearchProvider is null)
        {
            throw new InvalidOperationException("OnSearchFunction cannot be null.");
        }

        using var cts = new CancellationTokenSource();
        var items = await OnSearchProvider(e.Text, StringComparison, cts.Token);
        e.Items = items.Except([Owner]);
    }

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await Dialog.CloseAsync(_selectedItems);
        }
        else
        {
            await Dialog.CancelAsync();
        }
    }
}
