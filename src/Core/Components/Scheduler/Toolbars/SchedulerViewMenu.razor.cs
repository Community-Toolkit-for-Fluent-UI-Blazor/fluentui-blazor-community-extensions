using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a menu component for selecting different views within a scheduler interface.
/// </summary>
/// <remarks>This component is intended to be used as a child of <see cref="FluentCxScheduler{TItem}"/>.
/// Attempting to use it outside of a scheduler context will result in an exception.</remarks>
/// <typeparam name="TItem">The type of the data item displayed or managed by the scheduler.</typeparam>
public partial class SchedulerViewMenu<TItem> : IDisposable
{
    /// <summary>
    /// Gets or sets the parent scheduler component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property provides access to the parent instance of the scheduler component, allowing
    /// child components to interact with or reference the parent as needed. It is typically set automatically by the
    /// Blazor framework when components are nested within a scheduler context.</remarks>
    [CascadingParameter]
    private FluentCxScheduler<TItem>? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(SchedulerViewMenu<>)} must be used within a {nameof(FluentCxScheduler<>)}.");
        }

        Parent.SetViewMenu(this);
    }

    /// <summary>
    /// Handles the selection of a scheduler view asynchronously and updates the component state.
    /// </summary>
    /// <remarks>This method changes the current view of the parent scheduler component and triggers a UI
    /// update. It should be called when a new view is selected by the user.</remarks>
    /// <param name="value">The scheduler view to select. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnViewSelectedAsync(SchedulerView value)
    {
        await Parent!.ChangeViewAsync(value);
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Triggers a re-render of the component by invoking the state change asynchronously.
    /// </summary>
    /// <remarks>Call this method to update the UI when component state changes outside of the normal Blazor
    /// lifecycle. This method should be used with caution, as excessive calls may impact performance.</remarks>
    internal void Refresh()
    {
        InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Parent?.SetViewMenu(null);

        GC.SuppressFinalize(this);
    }
}
