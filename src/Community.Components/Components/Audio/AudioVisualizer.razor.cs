using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
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
    private const string JavaScriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Audio/AudioVisualizer.razor.js";

    /// <summary>
    /// Represents the JavaScript module for the audio visualizer component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Value indicating whether the visualizer mode has changed.
    /// </summary>
    private bool _hasModeChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioVisualizer"/> class.
    /// </summary>
    public AudioVisualizer()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the visualizer mode.
    /// </summary>
    [Parameter]
    public VisualizerMode Mode { get; set; } = VisualizerMode.Waveform;

    /// <summary>
    /// Gets or sets the cover source URL.
    /// </summary>
    [Parameter]
    public string? Cover { get; set; }

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
    [Parameter, EditorRequired]
    public string? Anchor { get; set; }

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
            await _module.InvokeVoidAsync("fluentCxAudioVisualizer.initialize", Id, Anchor, Mode, Cover);
            await _module.InvokeVoidAsync("fluentCxAudioVisualizer.setMode", Id, Mode);
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (string.IsNullOrWhiteSpace(Anchor))
        {
            throw new InvalidOperationException($"{nameof(Anchor)} parameter is required.");
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_hasModeChanged && _module is not null)
        {
            _hasModeChanged = false;
            await _module.InvokeVoidAsync("fluentCxAudioVisualizer.setMode", Id, Mode);
        }

        await base.OnParametersSetAsync();
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasModeChanged = parameters.HasValueChanged(nameof(Mode), Mode);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentCxAudioVisualizer.dispose", Id);
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
