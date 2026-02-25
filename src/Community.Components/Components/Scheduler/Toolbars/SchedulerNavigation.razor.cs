using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides navigation controls for a scheduler component, enabling users to move between different time periods or
/// views within the scheduler.
/// </summary>
/// <remarks>This component must be used within a <see cref="FluentCxScheduler{TItem}"/>. Attempting to use it
/// outside of a scheduler context will result in an exception during initialization.</remarks>
/// <typeparam name="TItem">The type of the data item displayed or managed by the scheduler.</typeparam>
public partial class SchedulerNavigation<TItem>
{
    /// <summary>
    /// Gets or sets the parent scheduler component that provides cascading context for this instance.
    /// </summary>
    [CascadingParameter]
    private FluentCxScheduler<TItem>? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerNavigation<>)} must be used within a {nameof(FluentCxScheduler<>)}.");
        }
    }

    /// <summary>
    /// Moves to the previous day, or week or month.
    /// </summary>
    private async Task OnPreviousAsync()
    {
        await Parent!.MoveToPreviousAsync();
    }

    /// <summary>
    /// Moves to the next day, or week or month.
    /// </summary>
    private async Task OnNextAsync()
    {
        await Parent!.MoveToNextAsync();
    }

    /// <summary>
    /// Moves to today's date.
    /// </summary>
    private async Task OnTodayAsync()
    {
        await Parent!.MoveToTodayAsync();
    }
}
