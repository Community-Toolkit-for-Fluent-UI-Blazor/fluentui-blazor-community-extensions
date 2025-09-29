using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the menu component for the signature component.
/// </summary>
public partial class SignatureMenu
    : FluentComponentBase
{
    /// <summary>
    /// Value indicating whether the multi-settings popover is open.
    /// </summary>
    private bool _isMultiSettingsPopoverOpen;

    /// <summary>
    /// Value indicating whether the pen is active.
    /// </summary>
    private bool _isPenActive = true;

    /// <summary>
    /// Renders the label for a button.
    /// </summary>
    private readonly RenderFragment<string> _renderLabel = label => builder =>
    {
        builder.AddContent(0, label);
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureMenu"/> class.
    /// </summary>
    public SignatureMenu()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the menu options.
    /// </summary>
    [Parameter]
    public MenuOptions Options { get; set; } = new MenuOptions();

    /// <summary>
    /// Gets or sets a value indicating whether the device is mobile.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the labels for the menu buttons.
    /// </summary>
    [Parameter]
    public SignatureLabels Labels { get; set; } = SignatureLabels.Default;

    /// <summary>
    /// Gets or sets the callback for when the undo button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnUndoClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the redo button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnRedoClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the clear button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClearClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the eraser button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEraserClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the pen button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPenClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnExportClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the pen settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnPenSettingsClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the eraser settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnEraserSettingsClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the grid settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnGridSettingsClicked { get; set; }

    /// <summary>
    /// Gets or sets the callback for when the watermark settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnWatermarkSettingsClicked { get; set; }

    /// <summary>
    /// Occurs when the export button is clicked.
    /// </summary>
    /// <returns></returns>
    private async Task OnExportClickedAsync()
    {
        if (OnExportClicked.HasDelegate)
        {
            await OnExportClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the pen button is clicked.
    /// </summary>
    /// <returns></returns>
    private async Task OnPenOrEraserClickedAsync()
    {
        _isPenActive = !_isPenActive;

        if (_isPenActive && OnPenClicked.HasDelegate)
        {
            await OnPenClicked.InvokeAsync();
        }
        else if (!_isPenActive && OnEraserClicked.HasDelegate)
        {
            await OnEraserClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the undo button is clicked.
    /// </summary>
    /// <returns></returns>
    private async Task OnUndoClickedAsync()
    {
        if (OnUndoClicked.HasDelegate)
        {
            await OnUndoClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the redo button is clicked.
    /// </summary>
    /// <returns></returns>
    private async Task OnRedoClickedAsync()
    {
        if (OnRedoClicked.HasDelegate)
        {
            await OnRedoClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the settings button is clicked.
    /// </summary>
    private void OnSettingsClicked()
    {
        _isMultiSettingsPopoverOpen = !_isMultiSettingsPopoverOpen;
    }

    /// <summary>
    /// Handles the event triggered when the grid settings button is clicked.
    /// </summary>
    /// <remarks>This method closes the multi-settings popover and invokes the <see
    /// cref="OnGridSettingsClicked"/> delegate if it has been assigned. Ensure that the delegate is set if additional
    /// actions are required when the grid settings button is clicked.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task OnGridSettingsClickedAsync()
    {
        _isMultiSettingsPopoverOpen = false;

        if (OnGridSettingsClicked.HasDelegate)
        {
            await OnGridSettingsClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Handles the event triggered when the watermark settings option is clicked.
    /// </summary>
    /// <remarks>This method closes the multi-settings popover and invokes the <see
    /// cref="OnWatermarkSettingsClicked"/> callback  if it has been assigned. Ensure that the callback is properly set
    /// to handle the event.</remarks>
    /// <returns></returns>
    private async Task OnWatermarkSettingsClickedAsync()
    {
        _isMultiSettingsPopoverOpen = false;

        if (OnWatermarkSettingsClicked.HasDelegate)
        {
            await OnWatermarkSettingsClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Handles the event triggered when the pen settings button is clicked.
    /// </summary>
    /// <remarks>This method closes the multi-settings popover and invokes the <see
    /// cref="OnPenSettingsClicked"/> callback  if it has been assigned. Ensure that the callback is properly set to
    /// handle the event.</remarks>
    private void OnPenSettingsClickedAsync()
    {
        _isMultiSettingsPopoverOpen = false;

        if (OnPenSettingsClicked.HasDelegate)
        {
            OnPenSettingsClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Handles the event triggered when the eraser settings button is clicked.
    /// </summary>
    /// <remarks>This method closes the multi-settings popover and invokes the <see
    /// cref="OnEraserSettingsClicked"/> callback if it has been assigned. Ensure that the callback is properly set to
    /// handle the event.</remarks>
    private void OnEraserSettingsClickedAsync()
    {
        _isMultiSettingsPopoverOpen = false;

        if (OnEraserSettingsClicked.HasDelegate)
        {
            OnEraserSettingsClicked.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when the clear button is clicked.
    /// </summary>
    /// <returns></returns>
    private async Task OnClearClickedAsync()
    {
        if (OnClearClicked.HasDelegate)
        {
            await OnClearClicked.InvokeAsync();
        }
    }
}
