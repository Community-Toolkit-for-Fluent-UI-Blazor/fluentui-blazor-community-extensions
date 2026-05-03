using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents a payload that includes animation options for a chart item.
/// </summary>
public interface IAnimatedPayload
{
    /// <summary>
    /// Gets the animation settings applied to the chart item, if any.
    /// </summary>
    ChartItemAnimation? Animation { get; set; }

    /// <summary>
    /// Gets a value indicating whether animations are enabled for this payload.
    /// </summary>
    bool AnimationEnabled { get; set; }

    /// <summary>
    /// Gets the animation effect used when rendering the chart.
    /// </summary>
    ChartAnimationEffect Effect { get; set; }

    /// <summary>
    /// Gets the trigger event that initiates the animation.
    /// </summary>
    ChartAnimationTrigger Trigger { get; set; }

    /// <summary>
    /// Gets the unique identifier of the chart to which this payload belongs.
    /// </summary>
    string ChartId { get; }

    /// <summary>
    /// Gets the group identifier for the chart item.
    /// </summary>
    string GroupId { get; }

    /// <summary>
    /// Gets the identifier of the chart item associated with this payload.
    /// </summary>
    string? Id { get; }
}

