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

    internal async Task OnViewSelectedAsync(SchedulerView value)
    {
        await Parent!.ChangeViewAsync(value);
        await InvokeAsync(StateHasChanged);
    }

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
