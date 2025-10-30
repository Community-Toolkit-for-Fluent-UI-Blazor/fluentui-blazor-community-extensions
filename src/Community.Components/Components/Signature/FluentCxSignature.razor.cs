using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a signature component that allows users to draw and manage signatures with various options and features.
/// </summary>
public partial class FluentCxSignature
    : ObserverItem
{
    /// <summary>
    /// Rerpresents the signature component.
    /// </summary>
    private Signature? _signature;

    /// <summary>
    /// Gets or sets the dialog service for displaying dialogs and panels.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the signature options.
    /// </summary>
    [Parameter]
    public SignatureOptions Options { get; set; } = new SignatureOptions();

    /// <summary>
    /// Gets or sets the menu options.
    /// </summary>
    [Parameter]
    public MenuOptions MenuOptions { get; set; } = new MenuOptions();

    /// <summary>
    /// Gets or sets the labels for the signature component.
    /// </summary>
    [Parameter]
    public SignatureLabels Labels { get; set; } = SignatureLabels.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the component is being rendered on a mobile device.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the export operation is completed.
    /// </summary>
    [Parameter]
    public EventCallback<SignatureExportEventArgs> ExportCompleted { get; set; }

    /// <summary>
    /// Gets the css for the signature area.
    /// </summary>
    private string? SignatureStyle => new StyleBuilder()
        .AddStyle("height", "calc(100% - 40px)", MenuOptions.IsVisible)
        .Build();

    /// <summary>
    /// Displays the export settings panel to the user and allows them to configure and confirm export options.
    /// </summary>
    /// <remarks>This method asynchronously opens a dialog panel for exporting signatures. The panel includes
    /// options  for configuring export settings, as well as primary and secondary actions for confirming or canceling 
    /// the operation. The dialog is aligned to the right side of the screen.</remarks>
    /// <returns>Returns a task which displays the export settings panel when completed.</returns>
    private async Task OnExportClickedAsync()
    {
        var dialog = await DialogService.ShowPanelAsync<SignatureExportPanel>((Labels, Options.Export), new DialogParameters<Tuple<SignatureLabels, SignatureExportOptions>>()
        {
            Content = new(Labels, Options.Export),
            Alignment = HorizontalAlignment.Right,
            Title = Labels.ExportSettings,
            PrimaryAction = Labels.Export,
            SecondaryAction = Labels.Cancel,
        });

        var result = await dialog.Result;

        if (result.Cancelled)
        {
            Options.Export.Reset();
            await InvokeAsync(StateHasChanged);
        }
        else if (_signature is not null)
        {
            var svg = await _signature.ExportSvgAsync();
            (var filename, var contentType, var data) = SignatureExporter.Export(Options.Export, svg);

            if (ExportCompleted.HasDelegate)
            {
                await ExportCompleted.InvokeAsync(new(filename, contentType, data));
            }
        }
    }

    /// <summary>
    /// Displays the pen settings panel to the user and allows them to configure and confirm pen options.
    /// </summary>
    /// <remarks>This method asynchronously opens a dialog panel for configuring pen options,
    /// as well as primary and secondary actions for confirming or canceling 
    /// the operation. The dialog is aligned to the right side of the screen.</remarks>
    /// <returns>Returns a task which displays the pen settings panel when completed.</returns>
    private async Task OnPenSettingsClickedAsync()
    {
        var dialog = await DialogService.ShowPanelAsync<SignaturePenPanel>((Labels, Options.Pen), new DialogParameters<Tuple<SignatureLabels, SignaturePenOptions>>()
        {
            Content = new(Labels, Options.Pen),
            Alignment = HorizontalAlignment.Right,
            Title = Labels.PenSettings,
            PrimaryAction = Labels.Apply,
            SecondaryAction = Labels.Cancel,
        });

        var result = await dialog.Result;

        if (result.Cancelled)
        {
            Options.Pen.Reset();
            await InvokeAsync(StateHasChanged);
        }
        else if (_signature is not null)
        {
            await _signature.RefreshInkAsync();
        }
    }

    /// <summary>
    /// Displays the eraser settings panel to the user and allows them to configure
    ///  and confirm eraser options.
    /// </summary>
    /// <remarks>This method asynchronously opens a dialog panel for configuring eraser options,
    /// as well as primary and secondary actions for confirming or canceling 
    /// the operation. The dialog is aligned to the right side of the screen.</remarks>
    /// <returns>Returns a task which displays the pen settings panel when completed.</returns>
    private async Task OnEraserSettingsClickedAsync()
    {
        var dialog = await DialogService.ShowPanelAsync<SignatureEraserPanel>((Labels, Options.Eraser), new DialogParameters<Tuple<SignatureLabels, SignatureEraserOptions>>()
        {
            Content = new(Labels, Options.Eraser),
            Alignment = HorizontalAlignment.Right,
            Title = Labels.EraserSettings,
            PrimaryAction = Labels.Apply,
            SecondaryAction = Labels.Cancel,
        });

        var result = await dialog.Result;

        if (result.Cancelled)
        {
            Options.Eraser.Reset();
            await InvokeAsync(StateHasChanged);
        }
        else if (_signature is not null)
        {
            await _signature.RefreshInkAsync();
        }
    }

    /// <summary>
    /// Displays the grid settings panel to the user and allows them to configure
    ///  and confirm grid options.
    /// </summary>
    /// <remarks>This method asynchronously opens a dialog panel for configuring grid options,
    /// as well as primary and secondary actions for confirming or canceling 
    /// the operation. The dialog is aligned to the right side of the screen.</remarks>
    /// <returns>Returns a task which displays the grid settings panel when completed.</returns>
    private async Task OnGridSettingsClickedAsync()
    {
        var dialog = await DialogService.ShowPanelAsync<SignatureGridPanel>((Labels, Options.Grid), new DialogParameters<Tuple<SignatureLabels, SignatureGridOptions>>()
        {
            Content = new(Labels, Options.Grid),
            Alignment = HorizontalAlignment.Right,
            Title = Labels.GridSettings,
            PrimaryAction = Labels.Apply,
            SecondaryAction = Labels.Cancel,
        });

        var result = await dialog.Result;

        if (result.Cancelled)
        {
            Options.Grid.Reset();
            await InvokeAsync(StateHasChanged);
        }
        else if (_signature is not null)
        {
            await _signature.RefreshGridAsync();
        }
    }

    /// <summary>
    /// Displays the watermark settings panel to the user and allows them to configure
    ///  and confirm watermark options.
    /// </summary>
    /// <remarks>This method asynchronously opens a dialog panel for configuring watermark options,
    /// as well as primary and secondary actions for confirming or canceling 
    /// the operation. The dialog is aligned to the right side of the screen.</remarks>
    /// <returns>Returns a task which displays the watermark settings panel when completed.</returns>
    private async Task OnWatermarkSettingsClickedAsync()
    {
        var dialog = await DialogService.ShowPanelAsync<SignatureWatermarkPanel>((Labels, Options.Watermark), new DialogParameters<Tuple<SignatureLabels, SignatureWatermarkOptions>>()
        {
            Content = new(Labels, Options.Watermark),
            Alignment = HorizontalAlignment.Right,
            Title = Labels.WatermarkSettings,
            PrimaryAction = Labels.Apply,
            SecondaryAction = Labels.Cancel,
        });

        var result = await dialog.Result;

        if (result.Cancelled)
        {
            Options.Watermark.Reset();
            await InvokeAsync(StateHasChanged);
        }
        else if (_signature is not null)
        {
            await _signature.RefreshOverlayAsync();
        }
    }

    /// <summary>
    /// Reverts the most recent action performed on the signature, if available.
    /// </summary>
    /// <remarks>This method performs an undo operation on the signature if a signature instance is present. 
    /// If no signature is available, the method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous undo operation.</returns>
    private async Task OnUndoClickedAsync()
    {
        if (_signature is not null)
        {
            await _signature.UndoAsync();
        }
    }

    /// <summary>
    /// Handles the redo action by invoking the redo operation on the associated signature, if available.
    /// </summary>
    /// <remarks>This method performs the redo operation asynchronously. If no signature is associated, the
    /// method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnRedoClickedAsync()
    {
        if (_signature is not null)
        {
            await _signature.RedoAsync();
        }
    }

    /// <summary>
    /// Handles the event triggered when the pen is clicked, ensuring the signature control exits erase mode.
    /// </summary>
    /// <remarks>This method sets the signature control to non-erase mode if the signature control is
    /// initialized.</remarks>
    /// <returns></returns>
    private async Task OnPenClickedAsync()
    {
        if (_signature is not null)
        {
            await _signature.SetEraseModeAsync(false);
        }
    }

    /// <summary>
    /// Enables erase mode for the signature component asynchronously.
    /// </summary>
    /// <remarks>This method sets the signature component to erase mode, allowing the user to erase content. 
    /// If the signature component is not initialized, the method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnEraserClickedAsync()
    {
        if (_signature is not null)
        {
            await _signature.SetEraseModeAsync(true);
        }
    }

    /// <summary>
    /// Clears the signature if one is currently present.
    /// </summary>
    /// <remarks>This method asynchronously clears the signature data by invoking the <see cref="Signature.ClearAsync"/>
    /// method  on the signature object. If no signature is present, the method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnClearClickedAsync()
    {
        if (_signature is not null)
        {
            await _signature.ClearAsync();
        }
    }

    /// <inheritdoc />
    protected internal override async Task OnResizeAsync(ResizeEventArgs e)
    {
        if (_signature is not null)
        {
            await _signature.OnResizedAsync((int)Math.Round(e.Width), (int)Math.Round(e.Height));
        }

        await base.OnResizeAsync(e);
    }
}
