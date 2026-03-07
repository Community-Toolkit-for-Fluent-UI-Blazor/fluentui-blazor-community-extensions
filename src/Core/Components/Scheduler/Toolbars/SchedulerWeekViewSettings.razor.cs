using System.Globalization;
using FluentUI.Blazor.Community.Components.Localization;
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
public partial class SchedulerWeekViewSettings<TItem> : IDisposable
{
    /// <summary>
    /// Indicates whether the week settings panel is open.
    /// </summary>
    private bool _openWeekSettings;

    /// <summary>
    /// Represents the number of subdivisions for each hour in the week view.
    /// </summary>
    private string? _weekSubdivisions;

    /// <summary>
    /// Represents the height of each time slot in the week view.
    /// </summary>
    private string? _weekSlotHeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="SchedulerWeekViewSettings{TItem}"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the week view scheduler. Cannot be null.</param>
    public SchedulerWeekViewSettings(LibraryConfiguration configuration)
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
        Parent!.SetWeekSettingsMenu(null);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException(
                string.Format(CultureInfo.InvariantCulture,
                    Localizer[LanguageResource.CX_Scheduler_WeekSettingsException],
                nameof(SchedulerWeekViewSettings<>),
                nameof(FluentCxScheduler<>)));
        }

        Parent.SetWeekSettingsMenu(this);
        _weekSubdivisions = Parent.WeekSubdivisions.ToString(Parent.Culture);
        _weekSlotHeight = Parent.WeekSlotHeight.ToString(Parent.Culture);
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
    /// Handles changes to the week subdivisions value and updates the parent component accordingly.
    /// </summary>
    /// <remarks>If the new value is null, empty, or cannot be parsed as an integer, the parent component is
    /// set to use 4 subdivisions by default. Otherwise, the parsed integer value is used. This method ensures the
    /// parent component always receives a valid subdivisions value.</remarks>
    private void OnSubdivisionsValueChanged()
    {
        if (Parent is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(_weekSubdivisions))
        {
            Parent.SetWeekSubdivisions(4);
        }
        else if (int.TryParse(_weekSubdivisions, NumberStyles.Integer, Parent.Culture, out var subdivisions))
        {
            Parent.SetWeekSubdivisions(subdivisions);
        }
        else
        {
            Parent.SetWeekSubdivisions(4);
        }
    }

    private void OnWeekSlotHeightValueChanged()
    {
        if (Parent is null)
        {
            return;
        }

        if (string.IsNullOrEmpty(_weekSlotHeight))
        {
            Parent.SetWeekSlotHeight(50);
        }
        else if (int.TryParse(_weekSlotHeight, NumberStyles.Integer, Parent.Culture, out var slotHeight))
        {
            Parent.SetWeekSlotHeight(slotHeight);
        }
        else
        {
            Parent.SetWeekSlotHeight(50);
        }
    }
}
