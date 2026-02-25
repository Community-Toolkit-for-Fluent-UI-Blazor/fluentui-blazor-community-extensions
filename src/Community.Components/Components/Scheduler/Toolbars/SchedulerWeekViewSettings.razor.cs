using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration settings for the day view of a scheduler component.
/// </summary>
/// <remarks>Use this class to customize the behavior and appearance of the scheduler's day view. Settings defined
/// here affect how individual days are rendered and interacted with in the parent scheduler.</remarks>
/// <typeparam name="TItem">The type of the data item displayed in the scheduler.</typeparam>
public partial class SchedulerWeekViewSettings<TItem> : IDisposable
{
    /// <summary>
    /// Indicates whether the week settings panel is open.
    /// </summary>
    private bool _openWeekSettings;

    /// <summary>
    /// Represents the number of subdivisions for each hour in the week view.
    /// </summary>
    private int _weekSubdivisions;

    /// <summary>
    /// Represents the height of each time slot in the week view.
    /// </summary>
    private int _weekSlotHeight;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxScheduler{TItem}"/> instance provided as a cascading parameter.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent <see cref="FluentCxScheduler{TItem}"/>. It enables child components to access shared
    /// scheduling functionality from the parent scheduler.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem> Parent { get; set; } = null!;

    /// <inheritdoc />
    public void Dispose()
    {
        Parent!.SetWeekSettingsMenu(null);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerDayViewSettings<>)} must be used within a {nameof(FluentCxScheduler<>)} component.");
        }

        Parent.SetWeekSettingsMenu(this);
        _weekSubdivisions = Parent.WeekSubdivisions;
        _weekSlotHeight = Parent.WeekSlotHeight;
    }

    /// <summary>
    /// Forces the component to re-render by notifying the framework that its state has changed.
    /// </summary>
    /// <remarks>Call this method when the component's state has been updated outside of the normal
    /// data-binding or event flow, and a UI refresh is required. This method should be used judiciously, as excessive
    /// calls may impact performance.</remarks>
    internal void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the current value and triggers a refresh of the parent component.
    /// </summary>
    private void OnValueChanged()
    {
        if (Parent is not null)
        {
            Parent.SetWeekSlotHeight(_weekSlotHeight);
            Parent.SetWeekSubdivisions(_weekSubdivisions);
        }
    }
}
