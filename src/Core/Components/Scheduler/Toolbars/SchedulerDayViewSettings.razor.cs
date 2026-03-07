using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides configuration settings for the day view of a scheduler component.
/// </summary>
/// <remarks>Use this class to customize the behavior and appearance of the scheduler's day view. Settings defined
/// here affect how individual days are rendered and interacted with in the parent scheduler.</remarks>
/// <typeparam name="TItem">The type of the data item displayed in the scheduler.</typeparam>
public partial class SchedulerDayViewSettings<TItem> : IDisposable
{
    /// <summary>
    /// Indicates whether the day settings panel is open.
    /// </summary>
    private bool _openDaySettings;

    /// <summary>
    /// Represents the number of subdivisions for each hour in the day view.
    /// </summary>
    private string? _subdivisions;

    /// <summary>
    /// Represents the height of each time slot in the day view.
    /// </summary>
    private string? _slotHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulerDayViewSettings{TItem}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration used to initialize the component.</param>
    public SchedulerDayViewSettings(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

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
        Parent!.SetDaySettingsMenu(null);

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

        Parent.SetDaySettingsMenu(this);
        _subdivisions = Parent.DaySubdivisions.ToString(Parent.Culture);
        _slotHeight = Parent.DaySlotHeight.ToString(Parent.Culture);
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
    /// Handles changes to the subdivisions value and updates the parent component accordingly.
    /// </summary>
    /// <remarks>This method should be called when the subdivisions value changes to ensure that the parent
    /// component reflects the updated state. If the parent component is not set, no action is taken.</remarks>
    private void OnSubdivisionsValueChanged()
    {
        if (!string.IsNullOrEmpty(_subdivisions))
        {
            return;
        }

        if (int.TryParse(_subdivisions, NumberStyles.Integer, Parent.Culture, out var subdivisions))
        {
            Parent?.SetDaySubdivisions(subdivisions);
        }
    }

    /// <summary>
    /// Handles changes to the slot height value and updates the parent component accordingly.
    /// </summary>
    /// <remarks>This method should be called whenever the slot height value changes to ensure the parent
    /// component reflects the updated height. If the parent component is not set, no action is taken.</remarks>
    private void OnSlotHeightValueChanged()
    {
        if (!string.IsNullOrEmpty(_slotHeight))
        {
            return;
        }

        if (int.TryParse(_slotHeight, NumberStyles.Integer, Parent.Culture, out var slotHeight))
        {
            Parent?.SetDaySubdivisions(slotHeight);
        }
    }
}
