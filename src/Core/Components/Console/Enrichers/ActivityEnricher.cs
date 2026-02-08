namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to enrich console log entries with contextual information from the current activity, if
/// available.
/// </summary>
internal sealed class ActivityEnricher : IConsoleEnricher
{
    /// <inheritdoc />
    public void Enrich(ConsoleEnrichmentBag bag)
    {
        var activity = System.Diagnostics.Activity.Current;

        if (activity is null)
        {
            return;
        }

        bag.ActivityId = activity.Id;
        bag.CorrelationId = activity.TraceId.ToString();
        bag.Properties["Activity.Name"] = activity.DisplayName;
        bag.Properties["Activity.StartTime"] = activity.StartTimeUtc;
        bag.Properties["Activity.Duration"] = activity.Duration;
        bag.Properties["Activity.Tags"] = activity.Tags;
        bag.Properties["Activity.TraceId"] = activity.TraceId.ToString();
        bag.Properties["Activity.SpanId"] = activity.SpanId.ToString();
        bag.Properties["Activity.ParentId"] = activity.ParentId;
    }
}
