using System.Diagnostics.CodeAnalysis;
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
    /// Represents the collection of strokes that make up the signature.
    /// </summary>
    private readonly List<SignatureStroke> _strokes = [];

    /// <summary>
    /// Represents the stack of undone strokes for redo functionality.
    /// </summary>
    private readonly Stack<SignatureStroke> _redoStrokes = [];

    /// <summary>
    /// Represents the reference to the signature canvas element in the DOM.
    /// </summary>
    private ElementReference _previewCanvas;

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
    /// Represents the current tool selected for drawing on the signature canvas.
    /// </summary>
    private SignatureTool _currentTool;

    /// <summary>
    /// Represents the render fragment for the label.
    /// </summary>
    private readonly RenderFragment<string> _renderLabel;

    /// <summary>
    /// Represents a value indicating whether the grid is shown.
    /// </summary>
    private bool _showGrid;

    /// <summary>
    /// Represents the width of the stroke.
    /// </summary>
    private float _strokeWidth;

    /// <summary>
    /// Represents the color of the stroke.
    /// </summary>
    private string _penColor = string.Empty;

    /// <summary>
    /// Represents the opacity of the stroke.
    /// </summary>
    private float _penOpacity;

    /// <summary>
    /// Represents the style of the stroke.
    /// </summary>
    private SignatureLineStyle _strokeStyle;

    /// <summary>
    /// Represents a value indicating if the line separator is visible.
    /// </summary>
    private bool _showSeparatorLine;

    /// <summary>
    /// Represents a value indicating if the signature use smoothing.
    /// </summary>
    private bool _useSmooth;

    /// <summary>
    /// Represents a value indicating if we use the pressure of the pointer.
    /// </summary>
    private bool _usePointerPressure;

    /// <summary>
    /// Represents a value indicating if the shadow is used.
    /// </summary>
    private bool _useShadow;

    /// <summary>
    /// Represents the opacity of the shadow.
    /// </summary>
    private float _shadowOpacity;

    /// <summary>
    /// Represents the color of the shadow.
    /// </summary>
    private string _shadowColor = string.Empty;

    /// <summary>
    /// Represents the format of the export.
    /// </summary>
    private SignatureExportFormat _exportFormat;

    /// <summary>
    /// Represents the available stroke styles.
    /// </summary>
    private static readonly SignatureLineStyle[] _strokeStyles = Enum.GetValues<SignatureLineStyle>();

    #endregion Fields

    #region Properties

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
    /// Gets or sets the intrinsic width of the signature component.
    /// </summary>
    [Parameter]
    public int IntrinsicWidth { get; set; } = 800;

    /// <summary>
    /// Gets or sets the intrinsic height of the signature component.
    /// </summary>
    [Parameter]
    public int IntrinsicHeight { get; set; } = 300;

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
        Width = IntrinsicWidth,
        Height = IntrinsicHeight,
        StrokeWidth = SignatureSettings.StrokeWidth,
        PenColor = SignatureSettings.PenColor,
        PenOpacity = SignatureSettings.PenOpacity,
        StrokeStyle = SignatureSettings.StrokeStyle,
        Smooth = SignatureSettings.Smooth,
        UseShadow = SignatureSettings.UseShadow,
        UsePointerPressure = SignatureSettings.UsePointerPressure,
        Background = SignatureSettings.BackgroundColor,
        ShowSeparatorLine = SignatureSettings.ShowSeparatorLine,
        SeparatorY = SignatureSettings.SeparatorY,
        ShowGrid = SignatureSettings.ShowGrid,
        GridType = SignatureSettings.GridType,
        GridSpacing = SignatureSettings.GridSpacing,
        GridColor = SignatureSettings.GridColor,
        GridOpacity = SignatureSettings.GridOpacity,
        WatermarkText = WatermarkSettings.Text,
        WatermarkOpacity = WatermarkSettings.Opacity
    };

    /// <summary>
    /// Gets or sets the <see cref="FluentDialog"/> instance.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets a value indicating whether the component is currently displayed within a dialog.
    /// </summary>
    private bool IsInDialog => Dialog is not null;

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
    private string SeparatorYPx => $"{(int)(SignatureSettings.SeparatorY * IntrinsicHeight)}px";

    /// <summary>
    /// Gets the internal style for the canvas element.
    /// </summary>
    private string? InternalCanvasStyle => new StyleBuilder()
        .AddStyle($"width", $"{Width}")
        .AddStyle($"height", $"{Height}")
        .AddStyle($"touch-action", "none")
        .Build();

    #endregion Properties

    #region Methods

    /// <summary>
    /// Occurs when the grid visibility is toggled.
    /// </summary>
    /// <returns>Returns a task which shows or hides the grid.</returns>
    private async Task OnSwapGridAsync()
    {
        _showGrid = !_showGrid;
        SignatureSettings.OnShowGridChanged(_showGrid);
        await SyncOptionsAsync(_previewCanvas);
    }

    /// <summary>
    /// Occurs when a value has changed.
    /// </summary>
    private async Task OnValueChanged()
    {
        SignatureSettings.UpdateInternalValues(
            _strokeWidth,
            _penColor,
            _penOpacity,
            _strokeStyle,
            _showSeparatorLine,
            _useSmooth,
            _usePointerPressure,
            _useShadow,
            _shadowOpacity,
            _shadowColor,
            _showGrid);

        await SyncOptionsAsync(_previewCanvas);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    private async Task OnChangeToolAsync(SignatureTool tool)
    {
        if (_currentTool != tool)
        {
            _currentTool = tool;

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
    /// Handles the maximize event asynchronously.
    /// </summary>
    /// <remarks>This method completes immediately and does not perform any operations. It can be overridden
    /// in derived classes to provide custom behavior when a maximize event occurs.</remarks>
    /// <returns></returns>
    private Task OnMaximizeAsync()
    {
        return Task.CompletedTask;
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
            await _module.InvokeVoidAsync("fluentCxSignature.setOptions", elementReference, Options, GetBackgroundImage(), GetWatermarkImage());
        }
    }

    /// <summary>
    /// Retrieves the label associated with the specified <see cref="SignatureLineStyle"/> value.
    /// </summary>
    /// <param name="value">The <see cref="SignatureLineStyle"/> value for which to retrieve the label.</param>
    /// <returns>A string representing the label corresponding to the specified <paramref name="value"/>. If the value does not
    /// match a predefined label, the string representation of the value is returned.</returns>
    private string GetLabelFromValue(SignatureLineStyle value)
    {
        return value switch
        {
            SignatureLineStyle.Solid => Labels.SolidLine,
            SignatureLineStyle.Dashed => Labels.DashedLine,
            SignatureLineStyle.Dotted => Labels.DottedLine,
            _ => value.ToString() ?? string.Empty,
        };
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
        _strokes.Clear();
        _redoStrokes.Clear();
        CurrentValue = null;
        EditContext?.NotifyFieldChanged(FieldIdentifier);

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.clear", _previewCanvas, Options, GetBackgroundImage(), GetWatermarkImage());
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
        if (_strokes.Count == 0)
        {
            return;
        }

        var s = _strokes[^1];
        _strokes.RemoveAt(_strokes.Count - 1);
        _redoStrokes.Push(s);
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
        if (_redoStrokes.Count == 0)
        {
            return;
        }

        var s = _redoStrokes.Pop();
        _strokes.Add(s);
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
            await _module.InvokeVoidAsync("fluentCxSignature.render", _previewCanvas, _strokes, Options, GetBackgroundImage(), GetWatermarkImage());
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
        return SignatureExporter.Export(IntrinsicWidth, IntrinsicHeight, _strokes, ExportSettings, SignatureSettings, WatermarkSettings);
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
            var options = Options;
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/FluentCxSignature.razor.js");
            await _module.InvokeVoidAsync("fluentCxSignature.initialize", _previewCanvas, _signatureDotNetRef, options, GetBackgroundImage(), GetWatermarkImage());
            await _module.InvokeVoidAsync("fluentCxSignature.render", _previewCanvas, _strokes, options, GetBackgroundImage(), GetWatermarkImage());
        }
        else if (_invalidateRender)
        {
            _invalidateRender = false;

            if (_module is not null)
            {
                var options = Options;
                await _module.InvokeVoidAsync("fluentCxSignature.render", _previewCanvas, _strokes, options, GetBackgroundImage(), GetWatermarkImage());
                await OnChangeToolAsync(_currentTool);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        var settings = SignatureSettings;

        _strokeWidth = settings.StrokeWidth;
        _penColor = settings.PenColor;
        _penOpacity = settings.PenOpacity;
        _strokeStyle = settings.StrokeStyle;
        _showSeparatorLine = settings.ShowSeparatorLine;
        _useSmooth = settings.Smooth;
        _usePointerPressure = settings.UsePointerPressure;
        _useShadow = settings.UseShadow;
        _shadowOpacity = settings.ShadowOpacity;
        _shadowColor = settings.ShadowColor;
        _showGrid = settings.ShowGrid;
    }

    /// <summary>
    /// Handles the completion of a stroke in the signature input, adding it to the collection of strokes.
    /// </summary>
    /// <param name="value">The completed stroke to be added to the signature.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [JSInvokable("OnStrokeCompleted")]
    public async Task OnStrokeCompletedAsync(SignatureStroke value)
    {
        _redoStrokes.Clear();
        _strokes.Add(value);
        await RefreshCanvasAsync();
        UpdateCurrentValueDefault();
    }

    #endregion Methods
}
