using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the business hours configuration for a scheduler component, enabling customization of working and
/// non-working time periods within the scheduling interface.
/// </summary>
/// <remarks>This component must be used within a <see cref="FluentCxScheduler{TItem}"/>. It allows the parent
/// scheduler to display or hide non-working hours based on the configured business hours. Disposing this component will
/// remove its business hours configuration from the parent scheduler.</remarks>
/// <typeparam name="TItem">The type of the data item associated with the scheduler entries.</typeparam>
public partial class SchedulerBusinessHours<TItem> : IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxScheduler{TItem}"/> instance that provides cascading context to this
    /// component.
    /// </summary>
    /// <remarks>This property is set automatically by the Blazor framework when the parent component supplies
    /// a cascading value of type <see cref="FluentCxScheduler{TItem}"/>. It should not be set manually.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem>? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerBusinessHours<>)} must be used within a {nameof(FluentCxScheduler<>)} component.");
        }

        Parent.SetBusinessHour(this);
    }

    /// <summary>
    /// Handles the toggle action for displaying non-working business hours asynchronously.
    /// </summary>
    /// <remarks>This method updates the parent component's display of non-working hours and triggers a UI
    /// refresh. It should be called in response to user interactions that change the visibility of business
    /// hours.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private void OnBusinessHoursToggle()
    {
        if (Parent is not null)
        {
            Parent.ToggleShowNonWorkingHours();
            InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.SetBusinessHour(null);

        GC.SuppressFinalize(this);
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
}
