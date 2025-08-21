using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the dialog to manage the cookies.
/// </summary>
public partial class ManageCookie
    : FluentComponentBase, IDialogContentComponent<CookieData>
{
    /// <summary>
    /// Gets or sets the parent dialog.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content of the dialog.
    /// </summary>
    [Parameter]
    public CookieData Content { get; set; } = default!;

    /// <summary>
    /// Gets a value indicating that the buttons are disabled or not.
    /// </summary>
    private bool ButtonsDisabled => Content.Items.Any(x => !x.IsActive.HasValue);

    /// <summary>
    /// Closes the dialog with a cancel result.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with a cancel result.</returns>
    private async Task OnCancelAsync()
    {
        foreach (var item in Content.Items)
        {
            if (item.Name == FluentCxCookie.GoogleAnalytics)
            {
                continue;
            }

            item.IsActive = null;
        }

        await Dialog.CancelAsync();
    }

    /// <summary>
    /// Closes the dialog with an ok result.
    /// </summary>
    /// <returns>Returns a task which closes the dialog with an ok result.</returns>
    private async Task OnSaveAsync()
    {
        await Dialog.CloseAsync(Content.Items);
    }
}
