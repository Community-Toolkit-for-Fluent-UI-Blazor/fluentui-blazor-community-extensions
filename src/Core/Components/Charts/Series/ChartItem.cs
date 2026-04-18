using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Charts.Series;

/// <summary>
/// Base class for all chart items.
/// Contains only metadata, style, tooltip, interaction and animation.
/// Data properties belong to derived types.
/// </summary>
public abstract class ChartItem
{
    /// <summary>
    /// Gets the unique identifier for the instance.
    /// </summary>
    public string? Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets the parent series identifier associated with this item.
    /// </summary>
    public string SerieId { get; internal set; } = string.Empty;

    /// <summary>
    /// Gets the zero-based index associated with this item.
    /// </summary>
    public int Index { get; internal set; }

    /// <summary>
    /// Gets the label text of the chart item.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets the optional name of the item.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets the optional tag object for custom use.
    /// </summary>
    public object? Tag { get; set; }

    /// <summary>
    /// Gets the style settings applied to the chart item.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the chart item, such as colors, borders, or
    /// other visual attributes. If not set, the default style is used.</remarks>
    public ChartItemStyle? Style { get; set; }

    /// <summary>
    /// Gets the tooltip content to display for the chart item.
    /// </summary>
    internal ChartItemTooltip Tooltip { get; set; } = new();

    /// <summary>
    /// Gets the interaction behavior associated with the chart item.
    /// </summary>
    public ChartItemInteraction? Interaction { get; set; }

    /// <summary>
    /// Gets the animation settings applied to the chart item.
    /// </summary>
    public ChartItemAnimation? Animation { get; set; }

    /// <summary>
    /// Gets the current interaction state of the chart item.
    /// </summary>
    public ChartInteractionState InteractionState { get; internal set; } = ChartInteractionState.Normal;

    /// <summary>
    /// Gets the animation effect used when rendering the chart.
    /// </summary>
    public ChartAnimationEffect Effect { get; init; } = ChartAnimationEffect.Fade;

    /// <summary>
    /// Gets the trigger event that initiates the animation.
    /// </summary>
    internal ChartAnimationTrigger Trigger { get; set; } = ChartAnimationTrigger.InitialAppear;

    /// <summary>
    /// Gets if the item is visible.
    /// </summary>
    public bool IsVisible { get; set; } = true;
}
