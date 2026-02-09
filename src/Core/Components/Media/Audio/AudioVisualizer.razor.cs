using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an audio visualizer component that can display audio data in various visual formats.
/// </summary>
public partial class AudioVisualizer
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represets the JavaScript file for the audio visualizer component.
    /// </summary>
    private const string JavaScriptFile = FluentCxConstants.JAVASCRIPT_ROOT + "Media/Audio/AudioVisualizer.razor.js";

    /// <summary>
    /// Represents the JavaScript module for the audio visualizer component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Value indicating whether the visualizer mode has changed.
    /// </summary>
    private bool _hasChanged;

    /// <summary>
    /// Value indicating whether the size of the visualizer has changed.
    /// </summary>
    private bool _hasSizeChanged;

    /// <summary>
    /// 
    /// </summary>
    private bool _hasAnchorChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioVisualizer"/> class.
    /// </summary>
    public AudioVisualizer(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the visualizer mode.
    /// </summary>
    [Parameter]
    public VisualizerMode Mode { get; set; } = VisualizerMode.Waveform;

    /// <summary>
    /// Gets or sets the color of the particles.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the width of the visualizer.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 600;

    /// <summary>
    /// Gets or sets the height of the visualizer.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 280;

    /// <summary>
    /// Gets or sets the anchor audio element identifier.
    /// </summary>
    [Parameter]
    public string? Anchor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the visualizer is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module ??= await JS.InvokeAsync<IJSObjectReference>("import", JavaScriptFile);
        }

        if (_hasAnchorChanged && !string.IsNullOrEmpty(Anchor) && _module is not null)
        {
            _hasAnchorChanged = false;
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.AudioVisualizer.Initialize", Id, Anchor, Mode, Color);
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.AudioVisualizer.SetMode", Id, Mode);
        }

        if (_hasChanged &&
            _module is not null &&
            IsVisible)
        {
            _hasChanged = false;
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.AudioVisualizer.SetMode", Id, Mode);
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasSizeChanged && _module is not null)
        {
            _hasSizeChanged = false;
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.AudioVisualizer.ResizeCanvas", Id, Width, Height);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasChanged = parameters.HasValueChanged(nameof(Mode), Mode) ||
                      parameters.HasValueChanged(nameof(IsVisible), IsVisible);

        _hasSizeChanged = parameters.HasValueChanged(nameof(Width), Width) ||
                         parameters.HasValueChanged(nameof(Height), Height);

        _hasAnchorChanged = parameters.HasValueChanged(nameof(Anchor), Anchor);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.AudioVisualizer.Dispose", Id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Ignore the exception that occurs when the JS runtime is already disposed.
        }

        GC.SuppressFinalize(this);
    }
}
