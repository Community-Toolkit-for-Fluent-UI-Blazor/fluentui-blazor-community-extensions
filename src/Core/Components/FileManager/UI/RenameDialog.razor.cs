using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dialog component for renaming a directory or a file.
/// </summary>
public partial class RenameDialog : FluentDialogInstance
{
    /// <summary>
    /// Gets or sets the name associated with this component or parameter.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the item represents a directory.
    /// </summary>
    [Parameter]
    public bool IsDirectory { get; set; }

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await DialogInstance.CloseAsync(Name);
        }
        else
        {
            await DialogInstance.CancelAsync();
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        OnValidateInput();
    }

    private void OnValidateInput()
    {
        var footer = DialogInstance.Options.Footer;
        var primaryAction = footer.PrimaryAction;

        primaryAction.Disabled = string.IsNullOrEmpty(Name) ||
                                 Name.Length > 50;
    }

    private Icon GetIcon()
    {
        return IsDirectory ? new Size16.Folder() : new Size16.Document();
    }
}
