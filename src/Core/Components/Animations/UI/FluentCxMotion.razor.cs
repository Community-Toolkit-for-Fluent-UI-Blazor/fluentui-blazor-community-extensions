using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that provides motion or animation effects for its child content.
/// </summary>
/// <remarks>Use this component to wrap other elements or markup that should participate in motion or animation
/// behaviors. The specific motion effects applied may depend on additional parameters or usage context. Typically used
/// in Razor markup to enhance the visual presentation of child elements.</remarks>
public partial class FluentCxMotion
    : FluentComponentBase
{
    /// <summary>
    /// Represents the collection of motion groups associated with the current instance.
    /// </summary>
    private readonly Dictionary<string, MotionGroup> _groups = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Represents a reference to the current instance of the <see cref="FluentCxMotion"/> component for JavaScript interop purposes.
    /// </summary>
    private DotNetObjectReference<FluentCxMotion>? _dotNetRef;

    /// <summary>
    /// Represents a flag indicating whether the frames per second (FPS) setting has changed and requires validation or reinitialization.
    /// </summary>
    private bool _hasFpsChanged;

    /// <summary>
    /// Represents the relative path to the JavaScript module used for FluentCx motion animations in the UI.
    /// </summary>
    /// <remarks>This constant is used to reference the JavaScript file required for animation features within
    /// FluentCx components. The path is constructed using the root JavaScript directory defined in
    /// FluentCxConstants.</remarks>
    private const string JavaScriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Animations/UI/FluentCxMotion.razor.js";

    /// <summary>
    /// Holds a reference to the JavaScript module used for interop operations.
    /// </summary>
    /// <remarks>This field is typically assigned when loading JavaScript modules via Blazor's JavaScript
    /// interop. It may be null if the module has not been loaded or has been disposed.</remarks>
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxMotion"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public FluentCxMotion(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>Use this property to specify child elements or markup that should appear within the
    /// component. Typically set in Razor markup using child content syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the target frames per second (FPS) for rendering or updates.
    /// </summary>
    /// <remarks>Set this property to control the frequency of updates or redraws. If not set, a default FPS
    /// value may be used depending on the component's implementation.</remarks>
    [Parameter]
    public int? CustomFps { get; set; }

    /// <summary>
    /// Gets or sets the motion frames per second (FPS) mode for the component.
    /// </summary>
    /// <remarks>Use this property to control the animation frame rate. The default value is <see
    /// cref="MotionFps.Auto"/>, which selects an appropriate frame rate based on the environment.</remarks>
    [Parameter]
    public MotionFps MotionFps { get; set; } = MotionFps.Auto;

    /// <summary>
    /// Validates that the combination of motion frames per second (FPS) settings is consistent and supported.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the FPS mode is not set to Custom but a custom FPS value is provided, or if the FPS mode is set to
    /// Custom but the custom FPS value is missing or not greater than zero.</exception>
    private void ValidateFps()
    {
        if (MotionFps != MotionFps.Custom && CustomFps.HasValue)
        {
            throw new InvalidOperationException("CustomFps is defined but Fps is not Custom.");
        }

        if (MotionFps == MotionFps.Custom && (!CustomFps.HasValue || CustomFps <= 0))
        {
            throw new InvalidOperationException("Fps is Custom but CustomFps is invalid.");
        }
    }

    /// <summary>
    /// Resolves the effective frames per second (FPS) value based on the current motion FPS setting.
    /// </summary>
    /// <remarks>Use this method to obtain the actual FPS value to be applied, depending on whether the motion
    /// FPS is set to automatic, a predefined value, or a custom value.</remarks>
    /// <returns>An integer representing the resolved FPS value. Returns 0 for automatic mode, a specific FPS value for
    /// predefined settings, or the custom FPS value if specified.</returns>
    private int ResolveFpsValue() => MotionFps switch
    {
        MotionFps.Auto => -1,
        MotionFps.Fps30 => 30,
        MotionFps.Fps50 => 50,
        MotionFps.Fps60 => 60,
        MotionFps.Fps75 => 75,
        MotionFps.Custom => CustomFps!.Value,
        _ => 60
    };

    /// <summary>
    /// Handles an animation frame update by advancing all registered groups by the specified time interval.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop to synchronize animation
    /// updates with the browser's rendering loop.</remarks>
    /// <param name="deltaMs">The elapsed time, in milliseconds, since the last animation frame. Must be a non-negative value.</param>
    [JSInvokable]
    public void OnAnimationFrame(double deltaMs)
    {
        var delta = TimeSpan.FromMilliseconds(deltaMs);

        foreach (var group in _groups.Values)
        {
            group.OnTick(delta);
        }
    }

    /// <summary>
    /// Registers the specified motion group with the current collection.
    /// </summary>
    /// <param name="group">The motion group to add to the collection. Cannot be null.</param>
    internal void Register(MotionGroup group) => _groups.Add(group.Id!, group);

    /// <summary>
    /// Removes the specified motion group from the internal collection, unregistering it from further processing.
    /// </summary>
    /// <param name="group">The motion group to remove from the collection. Cannot be null.</param>
    internal void Unregister(MotionGroup group) => _groups.Remove(group.Id!);

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();

        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.Stop", Id);
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JS runtime is already disconnected, so we can ignore this exception.
        }

        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        _dotNetRef = DotNetObjectReference.Create(this);
        var fps = ResolveFpsValue();

        _jsModule = await JSModule.ImportJavaScriptModuleAsync(JavaScriptModulePath);
        await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.Start", Id, _dotNetRef, fps);

        foreach(var item in _groups.Values)
        {
            await RegisterResizeObserverAsync(item.Id);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasFpsChanged = parameters.TryGetValue(nameof(MotionFps), out MotionFps newMotionFps) && newMotionFps != MotionFps
            || parameters.TryGetValue(nameof(CustomFps), out int? newCustomFps) && newCustomFps != CustomFps;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasFpsChanged)
        {
            if (_jsModule is not null)
            {
                ValidateFps();
                var fps = ResolveFpsValue();
                await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.UpdateFps", Id, fps);
            }

            _hasFpsChanged = false;
        }
    }

    /// <summary>
    /// Asynchronously sets the inline style of the specified element by its identifier.
    /// </summary>
    /// <remarks>This method uses JavaScript interop to update the element's style. If the JavaScript module
    /// is not loaded, the operation is ignored.</remarks>
    /// <param name="id">The identifier of the element whose style will be set. Can be null to target the root element.</param>
    /// <param name="style">The CSS style string to apply to the element. Must be a valid CSS style declaration.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task SetStyleAsync(string? id, string style)
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.SetStyle", id, style);
        }
    }

    /// <summary>
    /// Asynchronously sets up a resize observer for the specified element by its identifier.
    /// </summary>
    /// <param name="id">Identifier of the element to observe for resize events.</param>
    /// <returns>Returns a task that represents the asynchronous operation of setting up the resize observer.</returns>
    internal async Task RegisterResizeObserverAsync(string? id)
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.RegisterResizeObserver", Id, id);
        }
    }

    /// <summary>
    /// Unregisters a resize observer for the specified element identifier.
    /// </summary>
    /// <param name="id">The identifier of the element for which to unregister the resize observer. Can be null to indicate no specific
    /// element.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task UnregisterResizeObserverAsync(string? id)
    {
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("FluentUI.Blazor.Community.Motion.UnregisterResizeObserver", Id, id);
        }
    }

    /// <summary>
    /// Handles updates to the size of a motion group when its dimensions change.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop when the size of a motion
    /// group element changes. If the specified group is not found, the method has no effect.</remarks>
    /// <param name="id">The identifier of the motion group whose size has changed. Can be null or empty, in which case the method does
    /// nothing.</param>
    /// <param name="width">The new width of the motion group, in pixels.</param>
    /// <param name="height">The new height of the motion group, in pixels.</param>
    [JSInvokable]
    public void OnMotionGroupSizeChanged(string? id, double width, double height)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        if (!_groups.TryGetValue(id, out var value))
        {
            return;
        }

        value.SetDimensions(width, height);
    }
}
