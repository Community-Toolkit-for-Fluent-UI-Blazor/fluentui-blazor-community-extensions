using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Animations;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a container for animating child elements using various layout strategies.
/// </summary>
public partial class FluentCxAnimation
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Reprensents a value indicating whether the component has been rendered.
    /// </summary>
    private bool _isRendered;

    /// <summary>
    /// Represents a value indicating whether the MaxDisplayedItems parameter has changed.
    /// </summary>
    private bool _hasMaxDisplayedItemsChanged;

    /// <summary>
    /// Represents the layout strategy used for animating elements.
    /// </summary>
    private ILayoutStrategy? _layout;

    /// <summary>
    /// Represents the javascript module file path for animation handling.
    /// </summary>
    private readonly string JavaScriptModulePath = "./_content/FluentUI.Blazor.Community.Components/Components/Animations/FluentCxAnimation.razor.js";

    /// <summary>
    /// Represents the module reference for JavaScript interop.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents a flag indicating if the loop parameter has changed.
    /// </summary>
    private bool _hasLoopChanged;

    /// <summary>
    /// Represents a flag indicating if the width or height parameters have changed.
    /// </summary>
    private bool _hasSizeChanged;

    /// <summary>
    /// Represents the dotnet object reference for JavaScript interop.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxAnimation> _dotNetRef;

    /// <summary>
    /// Represents the animation engine instance.
    /// </summary>
    private readonly AnimationEngine _animationEngine = new();

    /// <summary>
    /// Represents whether the animation should run immediately without any transition duration.
    /// </summary>
    private bool _immediate;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxAnimation"/> class.
    /// </summary>
    public FluentCxAnimation()
    {
        Id = Identifier.NewId();
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the javaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the layout content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment? Layout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the animation should loop continuously.
    /// </summary>
    [Parameter]
    public bool Loop { get; set; }

    /// <summary>
    /// Gets or sets the width of the animation container in pixels.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 400;

    /// <summary>
    /// Gets or sets the height of the animation container in pixels.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 400;

    /// <summary>
    /// Gets or sets the maximum number of items to display in the animation.
    /// </summary>
    [Parameter]
    public int MaxDisplayedItems { get; set; } = 10;

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation starts.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationStarted { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation stops.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationStopped { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation is paused.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationPaused { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation is resumed.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationResumed { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation loop is completed.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationLooped { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is invoked when the animation is completed.
    /// </summary>
    [Parameter]
    public EventCallback OnAnimationCompleted { get; set; }

    /// <summary>
    /// Gets or sets the internal css class for the animation container.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("fluentcx-animation")
        .Build();

    /// <summary>
    /// Gets or sets the inline style for the animation container, including width and height CSS variables.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--animation-width", $"{Width}px", Width > 0)
        .AddStyle("--animation-height", $"{Height}px", Height > 0)
        .Build();

    /// <summary>
    /// Invokes the specified <see cref="EventCallback"/> asynchronously if it has a delegate.
    /// </summary>
    /// <remarks>If the <paramref name="callback"/> does not have a delegate, the method completes without
    /// invoking anything.</remarks>
    /// <param name="callback">The <see cref="EventCallback"/> to invoke. Must have a delegate to be invoked.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private static async Task InvokeAsync(EventCallback callback)
    {
        if (callback.HasDelegate)
        {
            await callback.InvokeAsync();
        }
    }

    /// <summary>
    /// Adds an animated element to the animation engine for management and rendering.
    /// </summary>
    /// <param name="item">Item to add.</param>
    internal void AddElement(AnimationItem item)
    {
        _animationEngine.Register(item.AnimatedElement);
    }

    /// <summary>
    /// Removes an animated element from the animation engine.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    internal void RemoveElement(AnimationItem item)
    {
        _animationEngine.Unregister(item.AnimatedElement);
    }

    /// <summary>
    /// Adds an animation group to the animation container.
    /// </summary>
    /// <param name="group">Group to add.</param>
    internal void AddGroup(AnimationGroup group)
    {
        _animationEngine.RegisterGroup(group.AnimatedElementGroup);
    }

    /// <summary>
    /// Removes an animation group from the animation container.
    /// </summary>
    /// <param name="group">Group to remove.</param>
    internal void RemoveGroup(AnimationGroup group)
    {
        _animationEngine.UnregisterGroup(group.AnimatedElementGroup);
    }

    /// <summary>
    /// Sets the layout strategy for the animation engine.
    /// </summary>
    /// <remarks>The specified layout strategy is updated with the current dimensions of the  object and then
    /// applied to the animation engine. Ensure that the layout strategy  is compatible with the animation engine's
    /// requirements.</remarks>
    /// <param name="layoutBase">The layout strategy to be applied. This determines how the layout dimensions  are configured and animated.
    /// Cannot be <see langword="null"/>.</param>
    internal void SetLayout([DisallowNull] ILayoutStrategy layoutBase)
    {
        _layout = layoutBase;
        _layout.SetDimensions(Width, Height);
        _animationEngine.SetLayout(_layout);
    }

    /// <summary>
    /// Removes the current layout strategy from the animation engine.
    /// </summary>
    internal void RemoveLayout()
    {
        _layout = null;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _isRendered = true;
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavaScriptModulePath);
            await StartAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasLoopChanged)
        {
            _hasLoopChanged = false;

            if (_module != null)
            {
                await _module.InvokeVoidAsync("setLoop", Id, Loop);
            }
        }

        if (_hasSizeChanged)
        {
            _hasSizeChanged = false;
            _layout?.SetDimensions(Width, Height);
        }

        if (_hasMaxDisplayedItemsChanged)
        {
            _hasMaxDisplayedItemsChanged = false;
            _animationEngine.SetMaxDisplayedItems(MaxDisplayedItems);
        }
    }

    /// <summary>
    /// Occurs when the animation loop is completed.
    /// </summary>
    /// <remarks>This method reset the elasped time of the layout.</remarks>
    [JSInvokable("OnLoopCompleted")]
    public async Task OnLoopCompletedAsync()
    {
        _layout?.ApplyStartTime(DateTime.Now);
        await InvokeAsync(OnAnimationLooped);
    }

    /// <summary>
    /// Occurs when the animation is completed.
    /// </summary>
    [JSInvokable("OnAnimationCompleted")]
    public async Task OnAnimationCompletedAsync()
    {
        if (_layout is MorphingLayout ml)
        {
            if (ml.NextLayout())
            {
                _animationEngine.ApplyStartTime(DateTime.Now);
                await StartAsync();
            }
            else
            {
                await InvokeAsync(OnAnimationCompleted);
            }
        }
        else
        {
            await InvokeAsync(OnAnimationCompleted);
        }
    }

    /// <summary>
    /// Occurs on each animation frame to update the animated elements and return their current state.
    /// </summary>
    /// <returns>Returns the elements to be animated.</returns>
    [JSInvokable]
    public List<JsonAnimatedElement> OnAnimationFrame()
    {
        return _animationEngine.Update();
    }

    /// <summary>
    /// Pauses the animation.
    /// </summary>
    /// <returns>Returns a task which pauses the animation when completed.</returns>
    public async Task PauseAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("pauseAnimation", Id);
            await InvokeAsync(OnAnimationPaused);
        }
    }

    /// <summary>
    /// Resumes the animation associated with the current instance.
    /// </summary>
    /// <returns>Returns a task which resumes the animation when completed.</returns>
    public async Task ResumeAsync()
    {
        if (_module != null)
        {
            await _module.InvokeVoidAsync("resumeAnimation", Id);
            await InvokeAsync(OnAnimationResumed);
        }
    }

    /// <summary>
    /// Stops the animation associated with the current instance.
    /// </summary>
    /// <returns>Returns a task which stops the animation when completed.</returns>
    public async Task StopAsync()
    {
        if (_module != null)
        {
            await _module.InvokeVoidAsync("stopAnimation", Id);
            await InvokeAsync(OnAnimationStopped);
        }
    }

    /// <summary>
    /// Asynchronously sets the duration for the specified animation.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetDurationAsync(double duration)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(duration, nameof(duration));

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("setDuration", Id, duration);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasLoopChanged = parameters.HasValueChanged(nameof(Loop), Loop);
        _hasSizeChanged = parameters.HasValueChanged(nameof(Width), Width) ||
                          parameters.HasValueChanged(nameof(Height), Height);
        _hasMaxDisplayedItemsChanged = parameters.HasValueChanged(nameof(MaxDisplayedItems), MaxDisplayedItems);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            _dotNetRef.Dispose();

            if (_module is not null)
            {
                await _module.InvokeVoidAsync("dispose", Id);
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        { }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Sets whether the animation should run immediately without any transition duration.
    /// </summary>
    /// <param name="immediate">Value indicating that the animation is immediate.</param>
    /// <returns>Returns a task which sets the duration of the animation.</returns>
    internal async Task SetImmediateAsync(bool immediate)
    {
        _immediate = immediate;
        await StopAsync();
        await SetDurationAsync(immediate ? 0 : (_layout?.Duration.TotalMilliseconds ?? 500));

        if (_isRendered)
        {
            await StartAsync();
        }
    }

    /// <summary>
    /// Starts the animation by invoking the JavaScript function with the necessary parameters.
    /// </summary>
    /// <returns>Returns a task which starts the animation when completed.</returns>
    public async Task StartAsync()
    {
        _animationEngine.ApplyStartTime(DateTime.Now);

        if (_module is not null)
        {
            await _module.InvokeVoidAsync("startAnimation", Id, _dotNetRef, new
            {
                duration = _immediate ? 0 : (_layout?.Duration.TotalMilliseconds ?? 500),
                loop = Loop,
                speed = 1.0
            });
        }

        await InvokeAsync(OnAnimationStarted);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        SetLayout(new BindStackLayout());
    }
}
