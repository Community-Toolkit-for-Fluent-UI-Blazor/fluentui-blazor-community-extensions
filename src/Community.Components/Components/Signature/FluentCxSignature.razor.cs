using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Fluent UI Signature component.
/// </summary>
public partial class FluentCxSignature
    : FluentInputBase<byte[]>
{
    #region Fields

    /// <summary>
    /// Represents all available line styles.
    /// </summary>
    private static readonly SignatureLineStyle[] _allLineStyles = Enum.GetValues<SignatureLineStyle>();

    /// <summary>
    /// Represents all available export formats.
    /// </summary>
    private static readonly SignatureExportFormat[] _exportFormats = Enum.GetValues<SignatureExportFormat>();

    /// <summary>
    /// Represents the reference to the signature canvas element in the DOM.
    /// </summary>
    private ElementReference _previewCanvas;

    /// <summary>
    /// Represents the reference to the grid canvas element in the DOM.
    /// </summary>
    private ElementReference _gridCanvas;

    /// <summary>
    /// Represents the reference to the pen preview canvas element in the DOM.
    /// </summary>
    private ElementReference _previewPenCanvas;

    /// <summary>
    /// Represents the JavaScript module for interop calls.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the .NET object reference for JavaScript interop.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxSignature> _signatureDotNetRef;

    /// <summary>
    /// Value indicating whether the component needs to re-render.
    /// </summary>
    private bool _invalidateRender;

    /// <summary>
    /// Represents the render fragment for the signature canvas.
    /// </summary>
    private readonly RenderFragment _renderSignature;

    /// <summary>
    /// Represents the render fragment for the toolbar.
    /// </summary>
    private readonly RenderFragment<Orientation> _renderToolbar;

    /// <summary>
    /// Value indicating whether the settings panel is open.
    /// </summary>
    private bool _isSettingsOpen;

    /// <summary>
    /// Represents the render fragment for the label.
    /// </summary>
    private readonly RenderFragment<string> _renderLabel;

    /// <summary>
    /// Represents the intrinsic width of the signature component.
    /// </summary>
    private int _intrinsicWidth;

    /// <summary>
    /// Represents the intrinsic height of the signature component.
    /// </summary>
    private int _intrinsicHeight;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets or sets the state of the signature.
    /// </summary>
    [Inject]
    private SignatureState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the export settings for the signature.
    /// </summary>
    [Parameter]
    public SignatureExportSettings ExportSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the signature settings.
    /// </summary>
    [Parameter]
    public SignatureSettings SignatureSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the watermark settings for the signature.
    /// </summary>
    [Parameter]
    public SignatureWatermarkSettings WatermarkSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the width of the signature component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the height of the signature component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the callback function to be invoked when the signature is cleared.
    /// </summary>
    [Parameter]
    public EventCallback OnClear { get; set; }

    /// <summary>
    /// Gets or sets the callback function to be invoked when the signature is exported.
    /// </summary>
    /// <remarks>
    /// Use this callback to handle the exported signature data, which includes the image bytes,
    /// and for downloading or processing the signature as needed in the browser.
    /// </remarks>
    [Parameter]
    public EventCallback<SignatureExportResult> OnExport { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets the configuration options for rendering and customizing the signature.
    /// </summary>
    /// <remarks>The options are derived from the current signature and watermark settings, allowing for 
    /// fine-grained control over the appearance and behavior of the signature. These settings include  properties such
    /// as stroke width, pen color, grid type, and watermark text.</remarks>
    private SignatureOptions Options => new()
    {
        Width = _intrinsicWidth,
        Height = _intrinsicHeight,
        StrokeWidth = State.StrokeWidth,
        PenColor = State.PenColor,
        PenOpacity = State.PenOpacity,
        StrokeStyle = State.StrokeStyle,
        Smooth = State.UseSmooth,
        UseShadow = State.UseShadow,
        UsePointerPressure = State.UsePointerPressure,
        Background = SignatureSettings.BackgroundColor,
        ShowSeparatorLine = State.ShowSeparatorLine,
        SeparatorY = SignatureSettings.SeparatorY,
        ShowGrid = State.ShowGrid,
        GridType = SignatureSettings.GridType,
        GridSpacing = SignatureSettings.GridSpacing,
        GridColor = SignatureSettings.GridColor,
        GridOpacity = SignatureSettings.GridOpacity,
        WatermarkText = WatermarkSettings.Text,
        WatermarkOpacity = WatermarkSettings.Opacity,
        ShadowColor = State.ShadowColor,
        ShadowOpacity = State.ShadowOpacity
    };

    /// <summary>
    /// Gets or sets the capabilities of the signature component.
    /// </summary>
    [Parameter]
    public SignatureCapabilities Capabilities { get; set; } = new();

    /// <summary>
    /// Gets or sets the position of the signature tool.
    /// </summary>
    [Parameter]
    public SignatureToolPosition ToolPosition { get; set; } = SignatureToolPosition.Top;

    /// <summary>
    /// Gets or sets a value indicating whether the component is being rendered on a mobile device.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the labels used in the signature component for localization.
    /// </summary>
    [Parameter]
    public SignatureLabels Labels { get; set; } = SignatureLabels.Default;

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("fluent-cx-signature")
        .Build();

    /// <summary>
    /// Gets the internal style for the preview component.
    /// </summary>
    private string? InternalPreviewStyle => new StyleBuilder()
        .AddStyle($"width", $"{Width}")
        .AddStyle($"height", $"{Height}")
        .AddStyle($"background", SignatureSettings.BackgroundImage is null ? SignatureSettings.BackgroundColor : "transparent")
        .Build();

    /// <summary>
    /// Gets the separator Y position in pixels.
    /// </summary>
    private string SeparatorYPx => $"calc(100% * {SignatureSettings.SeparatorY.ToString(CultureInfo.InvariantCulture)})";

    /// <summary>
    /// Gets the internal style for the canvas element.
    /// </summary>
    private string? InternalCanvasStyle => new StyleBuilder()
        .AddStyle("width", $"{Width}")
        .AddStyle("height", $"{Height}")
        .AddStyle("touch-action", "none")
        .AddStyle("position", "absolute")
        .Build();

    #endregion Properties

    #region Methods

    /// <summary>
    /// Occurs when the grid type has changed.
    /// </summary>
    /// <param name="value">The new grid type.</param>
    /// <returns>Returns a task which shows or hides the grid.</returns>
    private async Task OnChangeGridAsync(SignatureGridType value)
    {
        State.GridType = value;
        SignatureSettings.UpdateInternalValues(State);
        await SyncOptionsAsync(_previewCanvas);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    private async Task OnChangeToolAsync(SignatureTool tool)
    {
        if (State.CurrentTool != tool)
        {
            State.CurrentTool = tool;

            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentCxSignature.setTool", _previewCanvas, tool);
            }
        }
    }

    /// <summary>
    /// Handles the export operation by generating the export data and invoking the <see cref="OnExport"/> callback, if
    /// subscribed.
    /// </summary>
    /// <remarks>This method generates the export data, including the file content, MIME type, and filename,
    /// and then triggers the  <see cref="OnExport"/> event with the generated export result. The export format is
    /// determined by the current  <see cref="ExportSettings.Format"/>.</remarks>
    /// <returns></returns>
    private async Task OnExportAsync()
    {
        var (bytes, mime, filename) = Export();

        if (OnExport.HasDelegate)
        {
            await OnExport.InvokeAsync(new SignatureExportResult(filename, bytes, mime, ExportSettings.Format));
        }
    }

    /// <summary>
    /// Gets the background image as a base64 string.
    /// </summary>
    /// <returns>Returns the base64 string of the background image if exists, <see langword="null" /> otherwise.</returns>
    private string? GetBackgroundImage()
    {
        return SignatureSettings.BackgroundImage is null ? null : Convert.ToBase64String(SignatureSettings.BackgroundImage);
    }

    /// <summary>
    /// Gets the watermark image as a base64 string.
    /// </summary>
    /// <returns>Returns the base64 string of the watermark image if exists, <see langword="null" /> otherwise.</returns>
    private string? GetWatermarkImage()
    {
        return WatermarkSettings.Image is null ? null : Convert.ToBase64String(WatermarkSettings.Image);
    }

    /// <summary>
    /// Synchronizes the options for the specified element asynchronously.
    /// </summary>
    /// <remarks>This method updates the options for the specified element by invoking a JavaScript function. 
    /// Ensure that the module is initialized before calling this method.</remarks>
    /// <param name="elementReference">A reference to the HTML element whose options are being synchronized.</param>
    /// <returns></returns>
    private async Task SyncOptionsAsync(ElementReference elementReference)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.setOptions", _gridCanvas, elementReference, Options, GetBackgroundImage(), GetWatermarkImage());
            await _module.InvokeVoidAsync("fluentCxSignature.setAutoIntrinsicSize", _gridCanvas, _previewCanvas);
        }
    }

    /// <summary>
    /// Clears all strokes and resets the signature canvas to its initial state.
    /// </summary>
    /// <remarks>This method clears the internal stroke collections, resets the current value, and notifies
    /// the  associated <see cref="EditContext"/> of the field change. If a JavaScript module is provided,  it invokes
    /// the corresponding JavaScript function to clear the canvas. Finally, it triggers the  <see cref="OnClear"/> event
    /// if it has subscribers.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task OnClearAsync()
    {
        State.Clear();
        CurrentValue = null;
        EditContext?.NotifyFieldChanged(FieldIdentifier);

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.clear", _gridCanvas, _previewCanvas, Options, GetBackgroundImage(), GetWatermarkImage());
        }

        if (OnClear.HasDelegate)
        {
            await OnClear.InvokeAsync();
        }
    }

    /// <summary>
    /// Reverts the most recent stroke, moving it to the redo stack.
    /// </summary>
    /// <remarks>This method removes the last stroke from the collection of strokes and adds it to the redo
    /// stack. If there are no strokes to undo, the method exits without performing any action. After undoing, the
    /// canvas is refreshed and the current value is updated to its default state.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnUndoAsync()
    {
        var strokes = State.Strokes;

        if (strokes.Count == 0)
        {
            return;
        }

        var s = strokes[^1];
        strokes.RemoveAt(strokes.Count - 1);
        State.RedoStrokes.Push(s);
        await RefreshCanvasAsync();
        UpdateCurrentValueDefault();
    }

    /// <summary>
    /// Performs the redo operation by restoring the most recently undone stroke to the canvas.
    /// </summary>
    /// <remarks>This method moves a stroke from the redo stack back to the main collection of strokes  and
    /// refreshes the canvas to reflect the change. If there are no strokes available to redo,  the method exits without
    /// making any changes.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnRedoAsync()
    {
        var redoStrokes = State.RedoStrokes;

        if (redoStrokes.Count == 0)
        {
            return;
        }

        var s = redoStrokes.Pop();
        State.Strokes.Add(s);
        await RefreshCanvasAsync();
        UpdateCurrentValueDefault();
    }

    /// <summary>
    /// Refreshes the canvas by rendering the current strokes, options, background image, and watermark.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to update the canvas with the latest state.  Ensure
    /// that the module is initialized before calling this method.</remarks>
    /// <returns></returns>
    private async Task RefreshCanvasAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.render", _gridCanvas, _previewCanvas, State.Strokes, Options, GetBackgroundImage(), GetWatermarkImage());
        }
    }

    /// <summary>
    /// Updates the current value to the default value derived from the export operation and notifies the edit context
    /// of the field change.
    /// </summary>
    /// <remarks>This method sets the <see cref="CurrentValue"/> property to the byte data returned  by the
    /// <c>Export</c> method and triggers a field change notification in the associated  <see cref="EditContext"/>, if
    /// one is defined.</remarks>
    private void UpdateCurrentValueDefault()
    {
        CurrentValue = Export().bytes;
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    /// <summary>
    /// Exports the signature as a byte array along with its MIME type and suggested filename.
    /// </summary>
    /// <remarks>The exported signature includes the strokes, settings, and watermark as configured.  This
    /// method is typically used to generate a file-ready representation of the signature.</remarks>
    /// <returns>A tuple containing the following: <list type="bullet"> <item> <description><see cref="System.Byte"/>[]: The
    /// binary data representing the exported signature.</description> </item> <item> <description><see
    /// cref="System.String"/>: The MIME type of the exported file (e.g., "image/png").</description> </item> <item>
    /// <description><see cref="System.String"/>: The suggested filename for the exported file.</description> </item>
    /// </list></returns>
    private (byte[] bytes, string mime, string filename) Export()
    {
        return SignatureExporter.Export(_intrinsicWidth, _intrinsicHeight, State.Strokes, ExportSettings, SignatureSettings, WatermarkSettings);
    }

    /// <summary>
    /// Updates the preview line on the canvas asynchronously based on the current pen settings.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to render the preview line using the specified pen
    /// properties,  such as stroke width, opacity, color, and style. The method does nothing if the module is not
    /// initialized.</remarks>
    /// <returns>Returns a task which updates the preview of the line.</returns>
    private async Task UpdatePreviewAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.drawPreviewLine", _previewPenCanvas, new
            {
                thickness = State.StrokeWidth,
                opacity = State.PenOpacity,
                color = State.PenColor,
                smooth = State.UseSmooth,
                shadow = State.UseShadow,
                shadowOpacity = State.ShadowOpacity,
                shadowColor = State.ShadowColor,
                style = State.StrokeStyle
            });
        }
    }

    /// <summary>
    /// Updates the preview asynchronously after a change has occurred.
    /// </summary>
    /// <returns>Returns a task which updates the preview of the line.</returns>
    private async Task OnAfterChangeAsync()
    {
        SignatureSettings.UpdateInternalValues(State);
        ExportSettings.UpdateInternalValues(State);
        await UpdatePreviewAsync();
        _invalidateRender = true;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Occurs when the active ID changes.
    /// </summary>
    /// <param name="id">Id which has changed.</param>
    /// <returns>Returns a task which updates the pen.</returns>
    private async Task OnActiveIdChangedAsync(string id)
    {
        if (string.Equals(id, $"accordion-item-{Id}"))
        {
            await OnAfterChangeAsync();
        }
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out byte[] result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        result = [];
        validationErrorMessage = null;

        return true;
    }

    /// <summary>
    /// Invalidates the render.
    /// </summary>
    internal void InvalidateRender()
    {
        _invalidateRender = true;
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/FluentCxSignature.razor.js");
            await _module.InvokeVoidAsync("fluentCxSignature.initialize", _gridCanvas, _previewCanvas, _signatureDotNetRef, Options, GetBackgroundImage(), GetWatermarkImage());
            await _module.InvokeVoidAsync("fluentCxSignature.setAutoIntrinsicSize", _gridCanvas, _previewCanvas);
            await _module.InvokeVoidAsync("fluentCxSignature.render", _gridCanvas, _previewCanvas, State.Strokes, Options, GetBackgroundImage(), GetWatermarkImage());
        }

        if (_invalidateRender)
        {
            _invalidateRender = false;

            if (_module is not null)
            {
                var options = Options;
                await _module.InvokeVoidAsync("fluentCxSignature.render", _gridCanvas, _previewCanvas, State.Strokes, options, GetBackgroundImage(), GetWatermarkImage());
                await OnChangeToolAsync(State.CurrentTool);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        State.Update(SignatureSettings);
        State.Update(ExportSettings);
    }

    /// <summary>
    /// Handles the completion of a stroke in the signature input, adding it to the collection of strokes.
    /// </summary>
    /// <param name="value">The completed stroke to be added to the signature.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable("OnStrokeCompleted")]
    public async Task OnStrokeCompletedAsync(SignatureStroke value)
    {
        State.RedoStrokes.Clear();
        State.Strokes.Add(value);
        await RefreshCanvasAsync();
        UpdateCurrentValueDefault();
    }

    /// <summary>
    /// Handles the completion of an auto-size operation by updating the intrinsic dimensions  and synchronizing the
    /// canvas options.
    /// </summary>
    /// <param name="width">The new intrinsic width of the canvas, in pixels.</param>
    /// <param name="height">The new intrinsic height of the canvas, in pixels.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable("OnIntrinsicSizeSet")]
    public async Task OnIntrinsicSizeSetAsync(int width, int height)
    {
        _intrinsicWidth = width;
        _intrinsicHeight = height;

        await SyncOptionsAsync(_previewCanvas);
        await RefreshCanvasAsync();
    }

    #endregion Methods
}
