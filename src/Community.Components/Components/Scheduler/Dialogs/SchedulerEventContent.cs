using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the content associated with a scheduled event, including its labels and metadata.
/// </summary>
public sealed class SchedulerEventContent<TItem>
{
    /// <summary>
    /// Gets the set of labels associated with the scheduler instance.
    /// </summary>
    /// <remarks>The labels provide metadata that can be used to categorize or identify the scheduler for
    /// filtering, monitoring, or reporting purposes. The value is initialized to the default set of labels and is
    /// immutable after object construction.</remarks>
    public SchedulerLabels Labels { get; init; } = SchedulerLabels.Default;

    /// <summary>
    /// Gets the scheduler item associated with this instance.
    /// </summary>
    public SchedulerItem<TItem> Item { get; init; } = default!;

    /// <summary>
    /// Gets the template used to render the content of the scheduler event.
    /// </summary>
    public RenderFragment<TItem> Template { get; init; } = default!;

    /// <summary>
    /// Gets or sets the culture information used for formatting and parsing operations.
    /// </summary>
    /// <remarks>Changing this property affects how dates, numbers, and other culture-sensitive data are
    /// interpreted and displayed. By default, the current system culture is used.</remarks>
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;
}
