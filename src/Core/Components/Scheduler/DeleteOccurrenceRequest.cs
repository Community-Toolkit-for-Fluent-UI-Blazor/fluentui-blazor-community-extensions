namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a request to delete a specific occurrence identified by its unique ID and date.
/// </summary>
/// <param name="Id">The unique identifier of the occurrence to delete.</param>
/// <param name="OccurrenceDate">The date of the occurrence to delete.</param>
public record DeleteOccurrenceRequest(long Id, DateTime OccurrenceDate);
