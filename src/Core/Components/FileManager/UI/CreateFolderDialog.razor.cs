using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dialog component for creating a new folder within the application.
/// </summary>
/// <remarks>Use this component to prompt users for folder creation input in a user interface. The dialog
/// typically collects the folder name and confirms the creation action. Integrate this dialog into workflows where
/// users need to organize or add new folders.</remarks>
public partial class CreateFolderDialog : FluentDialogInstance
{
    /// <summary>
    /// Represents the name of the folder to be created, as entered by the user in the dialog.
    /// </summary>
    private string? _folderName;

    /// <inheritdoc />
    protected override async Task OnActionClickedAsync(bool primary)
    {
        if (primary)
        {
            await DialogInstance.CloseAsync(_folderName);
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

        primaryAction.Disabled = string.IsNullOrEmpty(_folderName) ||
                                 _folderName.Length > 50;
    }
}
