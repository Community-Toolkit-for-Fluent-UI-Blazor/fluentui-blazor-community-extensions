using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Charts.Styles;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Payloads;

/// <summary>
/// Represents the base payload for a chart item.
/// </summary>
public abstract record ChartItemPayloadBase : IAnimatedPayload, ILayerPayload
{
    /// <summary>
    /// Gets the unique identifier for the element.
    /// </summary>
    public required string? Id { get; init; }

    /// <summary>
    /// Gets the unique identifier for the group this element belongs to.
    /// </summary>
    public required string GroupId { get; init; }

    /// <summary>
    /// Gets the unique identifier for the chart instance.
    /// </summary>
    public required string ChartId { get; init; }

    /// <summary>
    /// Gets the index of the element within its group.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// Gets the index of the series this element belongs to.
    /// </summary>
    public required int SerieIndex { get; init; }

    /// <summary>
    /// Gets the visual state style for the normal state.
    /// </summary>
    public required ChartVisualStateStyle Normal { get; init; }

    /// <summary>
    /// Gets the visual state style for the hover state.
    /// </summary>
    public ChartVisualStateStyle? Hover { get; init; }

    /// <summary>
    /// Gets the visual state style for the pressed state.
    /// </summary>
    public ChartVisualStateStyle? Pressed { get; init; }

    /// <summary>
    /// Gets the visual state style for the selected state.
    /// </summary>
    public ChartVisualStateStyle? Selected { get; init; }

    /// <summary>
    /// Gets the visual state style for the disabled state.
    /// </summary>
    public ChartVisualStateStyle? Disabled { get; init; }

    /// <summary>
    /// Gets the animation options used to control chart rendering transitions.
    /// </summary>
    public ChartItemAnimation? Animation { get; set; }

    /// <summary>
    /// Gets a value indicating whether animations are enabled for this bar payload.
    /// </summary>
    public bool AnimationEnabled { get; set; }

    /// <summary>
    /// Gets the current interaction state of the chart, indicating how the user is interacting with the chart elements.
    /// </summary>
    /// <remarks>Use this property to determine the current interaction mode, such as normal, hovered, or
    /// selected, which may affect chart rendering or behavior.</remarks>
    public ChartInteractionState InteractionState { get; init; } = ChartInteractionState.Normal;

    /// <inheritdoc />
    public ChartAnimationEffect Effect { get; set; }

    /// <inheritdoc />
    public ChartAnimationTrigger Trigger { get; set; } = ChartAnimationTrigger.InitialAppear;

    /// <summary>
    /// Gets a value indicating whether the component is disabled and cannot be interacted with.
    /// </summary>
    public bool IsDisabled { get; init; }

    /// <summary>
    /// Gets the tooltip payload associated with the chart item, providing information for displaying tooltips.
    /// </summary>
    public ChartTooltipPayload Tooltip { get; init; } = new();

    /// <summary>
    /// Retrieves the visual style associated with the current chart interaction state.
    /// </summary>
    /// <remarks>The returned style corresponds to the current value of InteractionState. If a style for the
    /// current state is not set, the method falls back to styles for less specific states, ultimately returning the
    /// Normal style if no other is available.</remarks>
    /// <returns>A ChartVisualStateStyle representing the style for the current interaction state. If a specific style for the
    /// state is not defined, a fallback style is returned based on the available styles.</returns>
    internal ChartVisualStateStyle GetStyleForState()
    {
        if (IsDisabled)
        {
            return Disabled ?? Normal;
        }

        return InteractionState switch
        {
            ChartInteractionState.Hover => Hover ?? Normal,
            ChartInteractionState.Pressed => Pressed ?? Hover ?? Normal,
            ChartInteractionState.Selected => Selected ?? Normal,
            _ => Normal
        };
    }
}
