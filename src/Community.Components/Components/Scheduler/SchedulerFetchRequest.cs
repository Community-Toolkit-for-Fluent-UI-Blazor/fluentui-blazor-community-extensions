namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a request to fetch scheduler data for a specified view and date range.
/// </summary>
/// <param name="View">The type of scheduler view to retrieve, such as daily, weekly, or monthly.</param>
/// <param name="StartDate">The start date of the range for which scheduler data is requested.</param>
/// <param name="EndDate">The end date of the range for which scheduler data is requested.</param>
/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
public record SchedulerFetchRequest(SchedulerView View, DateTime StartDate, DateTime EndDate, CancellationToken cancellationToken = default)
{
}
