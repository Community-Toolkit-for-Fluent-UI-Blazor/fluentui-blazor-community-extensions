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
    /// 
    /// </summary>
    private ElementReference _previewCanvas;

    /// <summary>
    /// 
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// 
    /// </summary>
    private readonly DotNetObjectReference<FluentCxSignature> _signatureDotNetRef;

    /// <summary>
    /// 
    /// </summary>
    private bool _invalidateRender;

    /// <summary>
    /// 
    /// </summary>
    private readonly RenderFragment _renderSignature;

    /// <summary>
    /// 
    /// </summary>
    private readonly RenderFragment<Orientation> _renderToolbar;

    /// <summary>
    /// 
    /// </summary>
    private bool _isSettingsOpen;

    /// <summary>
    /// Represents the current tool selected for drawing on the signature canvas.
    /// </summary>
    private SignatureTool _currentTool;

    /// <summary>
    /// 
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
    /// Gets or sets the width of the signature component.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 300;

    /// <summary>
    /// Gets or sets the height of the signature component.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 150;

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
        Width = Width,
        Height = Height,
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
        .AddStyle($"width", $"{Width}px")
        .AddStyle($"height", $"{Height}px")
        .AddStyle($"background", SignatureSettings.BackgroundImage is null ? SignatureSettings.BackgroundColor : "transparent")
        .Build();

    /// <summary>
    /// Gets the separator Y position in pixels.
    /// </summary>
    private string SeparatorYPx => $"{(int)(SignatureSettings.SeparatorY * Height)}px";

    #endregion Properties

    #region Methods

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

    private async Task OnExportAsync()
    {
        var (bytes, mime, filename) = Export();

        if (OnExport.HasDelegate)
        {
            await OnExport.InvokeAsync(new SignatureExportResult(filename, bytes, mime, ExportSettings.Format));
        }
    }

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

    private async Task SyncOptionsAsync(ElementReference elementReference)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.setOptions", elementReference, Options, GetBackgroundImage(), GetWatermarkImage());
        }
    }

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

    private async Task RefreshCanvasAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentCxSignature.render", _previewCanvas, _strokes, Options, GetBackgroundImage(), GetWatermarkImage());
        }
    }

    private void UpdateCurrentValueDefault()
    {
        CurrentValue = Export().bytes;
        EditContext?.NotifyFieldChanged(FieldIdentifier);
    }

    private (byte[] bytes, string mime, string filename) Export()
    {
        return SignatureExporter.Export(Width, Height, _strokes, ExportSettings, SignatureSettings, WatermarkSettings);
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
