using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a customizable player component for rendering and controlling animations.
/// </summary>
/// <remarks>The <see cref="FluentCxLootiePlayer"/> component allows developers to render animations using a specified
/// renderer and provides options to control playback behavior, such as looping, autoplay, and playback speed. The
/// component also supports customizable dimensions and accepts a path to the animation file.</remarks>
public partial class FluentCxLootiePlayer
    : FluentComponentBase
{
    /// <summary>
    /// Represents a JavaScript module reference that can be used to invoke JavaScript functions.
    /// </summary>
    /// <remarks>This field holds a reference to a JavaScript module loaded via an interop mechanism, such as
    /// Blazor's JavaScript interop. The value is <see langword="null"/> if the module has not been initialized or has
    /// been disposed.</remarks>
    private IJSObjectReference? _module;

    /// <summary>
    /// Indicates whether a property value has been modified.   
    /// </summary>
    private bool _hasPropertyChanged;

    /// <summary>
    /// Represents a reference to a .NET object of type <see cref="FluentCxLootiePlayer"/>  that can be passed to
    /// JavaScript interop in Blazor.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxLootiePlayer> _dotNetRef;

    /// <summary>
    /// Represents the controls for the Lootie animation player.
    /// </summary>
    private LootieControls? _controls;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxLootiePlayer"/> class.
    /// </summary>
    /// <remarks>The constructor generates a unique identifier for the player by assigning a new value to the
    /// <see cref="FluentComponentBase.Id"/> property.</remarks>
    public FluentCxLootiePlayer()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the source to be used for the animation.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the animation should loop.
    /// </summary>
    [Parameter]
    public bool Loop { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the animation should start playing automatically.
    /// </summary>
    [Parameter]
    public bool Autoplay { get; set; }

    /// <summary>
    /// Gets or sets the speed of the animation playback. The default value is 1, which represents normal speed.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1;

    /// <summary>
    /// Gets or sets the width of the player. This can be specified in any valid CSS unit (e.g., "100px", "50%", "10rem").
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the player. This can be specified in any valid CSS unit (e.g., "100px", "50%", "10rem").
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the renderer to be used for rendering the animation. The default value is <see cref="LootieRenderer.Svg"/>.
    /// </summary>
    [Parameter]
    public LootieRenderer Renderer { get; set; } = LootieRenderer.Svg;

    /// <summary>
    /// Gets or sets a callback that is invoked when the animation completes.
    /// </summary>
    [Parameter]
    public EventCallback OnComplete { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the animation loops.
    /// </summary>
    [Parameter]
    public EventCallback OnLoop { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the animation enters a new frame.
    /// </summary>
    [Parameter]
    public EventCallback OnEnterFrame { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript runtime instance used for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the computed CSS classes for the component.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("fluentcx-lootie-player")
        .Build();

    /// <summary>
    /// Gets the computed inline styles for the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--lootie-player-width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("--lootie-player-height", Height, !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Loads the animation asynchronously.
    /// </summary>
    /// <returns>Returns a task which loads the animation when completed.</returns>
    private async Task LoadAnimationAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.load", Id, _dotNetRef, Source, Loop, Autoplay, Speed, Renderer);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Updates the animation if a property has changed.
    /// </summary>
    /// <returns></returns>
    private async Task OnUpdateAnimationAsync()
    {
        if (_hasPropertyChanged)
        {
            _hasPropertyChanged = false;
            await LoadAnimationAsync();
        }
    }

    /// <summary>
    /// Adds the specified controls to the player.
    /// </summary>
    /// <param name="controls">Controls of the player.</param>
    internal void Add(LootieControls controls)
    {
        _controls = controls;
    }

    /// <summary>
    /// Removes the controls from the player.
    /// </summary>
    /// <param name="controls">Controls of the player to remove.</param>
    internal void Remove(LootieControls controls)
    {
        if (_controls == controls)
        {
            _controls = null;
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Lootie/FluentCxLootiePlayer.razor.js");
            await LoadAnimationAsync();
        }
    }

    /// <summary>
    /// Plays the animation.
    /// </summary>
    /// <returns>Returns a task wich plays the animation.</returns>
    public async ValueTask PlayAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.play", Id);
        }
    }

    /// <summary>
    /// Pauses the operation associated with the current instance asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask PauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.pause", Id);
        }
    }

    /// <summary>
    /// Stops the operation associated with the current instance asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask StopAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.stop", Id);
        }
    }

    /// <summary>
    /// Asynchronously sets the speed of the object identified by the current instance.
    /// </summary>
    /// <param name="speed">The new speed value to set. Must be a valid double representing the desired speed.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask SetSpeedAsync(double speed)
    {
        if (_module is not null)
        {
            Speed = speed;
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.setSpeed", Id, speed);
        }
    }

    /// <summary>
    /// Asynchronously sets the direction of the specified element.
    /// </summary>
    /// <param name="direction">The new direction to apply to the element.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask SetDirectionAsync(LootieDirection direction)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.setDirection", Id, direction);
        }
    }

    /// <summary>
    /// Plays a range of segments within the animation.
    /// </summary>
    /// <param name="startSegment">The index of the segment at which playback should start. Must be a non-negative integer.</param>
    /// <param name="endSegment">The index of the segment at which playback should end. Must be greater than or equal to <paramref
    /// name="startSegment"/>.</param>
    /// <param name="forceFlag">A value indicating whether to force playback, overriding any current playback state. Defaults to <see
    /// langword="false"/>.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask PlaySegments(int startSegment, int endSegment, bool forceFlag = false)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("fluentcxLootiePlayer.playSegments", Id, startSegment, endSegment, forceFlag);
        }
    }

    /// <summary>
    /// Toggles the loop state of the current instance.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async ValueTask ToggleLoopAsync(bool isLooping)
    {
        Loop = isLooping;
        _hasPropertyChanged = true;
        await OnUpdateAnimationAsync();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentcxLootiePlayer.dispose", Id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        { }

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasPropertyChanged = parameters.HasValueChanged(nameof(Source), Source)
            || parameters.HasValueChanged(nameof(Loop), Loop)
            || parameters.HasValueChanged(nameof(Autoplay), Autoplay)
            || parameters.HasValueChanged(nameof(Speed), Speed)
            || parameters.HasValueChanged(nameof(Renderer), Renderer);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        await OnUpdateAnimationAsync();
    }

    /// <summary>
    /// Occurs when the animation completes.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnComplete"/> callback.</returns>
    [JSInvokable("onComplete")]
    public async Task NotifyCompleteAsync()
    {
        if (OnComplete.HasDelegate)
        {
            await OnComplete.InvokeAsync();
        }

        if (_controls is not null)
        {
            await _controls.OnAnimationCompletedAsync();
        }
    }

    /// <summary>
    /// Occurs when a loop completes.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnLoop"/> callback.</returns>
    [JSInvokable("onLoop")]
    public async Task NotifyLoopAsync()
    {
        if (OnLoop.HasDelegate)
        {
            await OnLoop.InvokeAsync();
        }
    }

    /// <summary>
    /// Occurs when entering a new frame.
    /// </summary>
    /// <returns>Returns a task which invokes the <see cref="OnEnterFrame"/> callback.</returns>
    [JSInvokable("onFrame")]
    public async Task NotifyEnterFrameAsync()
    {
        if (OnEnterFrame.HasDelegate)
        {
            await OnEnterFrame.InvokeAsync();
        }
    }
}
