using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the viewer of a <see cref="SleekDialItem"/> collection.
/// </summary>
public partial class SleekDialView
    : FluentComponentBase
{
    /// <summary>
    /// Represents the <see cref="FluentCxSleekDial"/> component.
    /// </summary>
    [CascadingParameter]
    internal FluentCxSleekDial Parent { get; set; } = default!;

    /// <summary>
    /// Refreshes the component state.
    /// </summary>
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType()} must be used inside a {nameof(FluentCxSleekDial)} component.");
        }

        Parent.Viewer = this;
    }
}
