using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the agenda menu component used within a scheduler to manage agenda-related settings and actions for items
/// of type <typeparamref name="TItem"/>.
/// </summary>
/// <remarks>This component must be used as a child of a <see cref="FluentCxScheduler{TItem}"/> component.
/// Attempting to use it outside of a scheduler context will result in an exception during initialization.</remarks>
/// <typeparam name="TItem">The type of the items managed by the scheduler and associated with the agenda menu.</typeparam>
public partial class SchedulerAgendaMenu<TItem> : IDisposable
{
    /// <summary>
    /// Flag indicating whether the agenda settings panel is open.
    /// </summary>
    private bool _openAgendaSettings;

    /// <summary>
    /// Indicates whether to hide days without scheduled items in the agenda view.
    /// </summary>
    private bool _hideEmptyDays;

    /// <summary>
    /// Number of days to display in the agenda view.
    /// </summary>
    private int _numberOfDays;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxScheduler{TItem}"/> instance provided as a cascading parameter.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent <see cref="FluentCxScheduler{TItem}"/>. It enables child components to access shared
    /// scheduling context from their parent.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem>? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        _openAgendaSettings = false;
        Parent!.SetAgendaMenu(null);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerAgendaMenu<>)} must be used within a {nameof(FluentCxScheduler<>)} component.");
        }

        Parent.SetAgendaMenu(this);
        _hideEmptyDays = Parent.HideEmptyDays;
        _numberOfDays = Parent.NumberOfDays;
    }

    /// <summary>
    /// Forces the component to re-render by notifying the framework that its state has changed.
    /// </summary>
    /// <remarks>Call this method when the component's state has been updated outside of the normal
    /// data-binding or event flow, and a UI refresh is required. This method should be used sparingly, as frequent
    /// calls may impact performance.</remarks>
    internal void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Handles changes to the value and updates the parent component's empty days visibility setting asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnValueChanged()
    {
        Parent!.SetAgendaDays(_numberOfDays);
        Parent!.SetHideEmptyDays(_hideEmptyDays);
    }
}
