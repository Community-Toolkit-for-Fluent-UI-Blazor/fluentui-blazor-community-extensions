using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the controls for a Lootie animation player.
/// </summary>
public partial class LootieControls
    : FluentComponentBase
{
    /// <summary>
    /// Indicates whether the animation is currently playing.
    /// </summary>
    private bool _isPlaying;

    /// <summary>
    /// Indicates whether the animation is set to loop.
    /// </summary>
    private bool _isLooping;

    /// <summary>
    /// Represents the label rendering fragment.
    /// </summary>
    private readonly RenderFragment<string> _renderLabel;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLootiePlayer"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxLootiePlayer? Parent { get; set; }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the controls are displayed in a mobile layout.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets or sets the labels used for localization in the controls.
    /// </summary>
    [Parameter]
    public LootieLabels Labels { get; set; } = LootieLabels.Default;

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType()} must be used within a {nameof(FluentCxLootiePlayer)}.");
        }

        _isLooping = Parent.Loop;
    }

    /// <summary>
    /// Invokes the specified asynchronous function and updates the playing state if necessary.
    /// </summary>
    /// <param name="func">Func to invoke.</param>
    /// <param name="isPlaying">Value indicating if the animation is playing.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    private async ValueTask InvokeAsync(Func<ValueTask> func, bool isPlaying)
    {
        if (!_isPlaying)
        {
            _isPlaying = isPlaying;
            await InvokeAsync(StateHasChanged);
        }

        await func();
    }

    /// <summary>
    /// Plays the current instance asynchronously, delegating the operation to the parent if one exists.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous play operation. If no parent exists, the task
    /// completes immediately.</returns>
    public async Task PlayAsync()
    {
        if (Parent is not null)
        {
            await InvokeAsync(Parent.PlayAsync, true);
        }
    }

    /// <summary>
    /// Pauses the current operation asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous pause operation.</returns>
    public async Task PauseAsync()
    {
        if (Parent is not null)
        {
            await InvokeAsync(Parent.PauseAsync, false);
        }
    }

    /// <summary>
    /// Asynchronously stops the current operation, delegating the stop request to the parent if one exists.    
    /// </summary>
    /// <returns></returns>
    public async Task StopAsync()
    {
        if (Parent is not null)
        {
            await InvokeAsync(Parent.StopAsync, false);
        }
    }

    /// <summary>
    /// Asynchronously sets the speed of the current object.
    /// </summary>
    /// <param name="speed">The desired speed to set, represented as a double. The value must be within the valid range supported by the
    /// parent object.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async Task SetSpeedAsync(double speed)
    {
        if (Parent is not null)
        {
            await Parent.SetSpeedAsync(speed);
        }
    }

    /// <summary>
    /// Asynchronously sets the direction for the current object.
    /// </summary>
    /// <param name="direction">The <see cref="LootieDirection"/> to set for the object.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public async Task SetDirectionAsync(LootieDirection direction)
    {
        if (Parent is not null)
        {
            await Parent.SetDirectionAsync(direction);
        }
    }

    /// <summary>
    /// Toggles the loop state of the parent asynchronously.
    /// </summary>
    /// <param name="isLooping">Value indicating if the animation loops.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task ToggleLoopAsync(bool isLooping)
    {
        _isLooping = isLooping;

        if (Parent is not null)
        {
            await Parent.ToggleLoopAsync(isLooping);
        }
    }
}
