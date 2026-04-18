using FluentUI.Blazor.Community.Components.Charts.Drawing;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts.Engines;

/// <summary>
/// Represents the engine responsible for managing interactions (hover, press, select) on chart items.
/// </summary>
internal sealed class ChartInteractionEngine
{
    /// <summary>
    /// Represents a delegate that provides a collection of chart series when invoked.
    /// </summary>
    private readonly Func<IEnumerable<ChartSerie>> _seriesProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartInteractionEngine"/> class with the specified series provider.
    /// </summary>
    /// <param name="seriesProvider">A delegate that provides a collection of chart series.</param>
    public ChartInteractionEngine(Func<IEnumerable<ChartSerie>> seriesProvider)
    {
        _seriesProvider = seriesProvider;
    }

    /// <summary>
    /// Searches for a chart series and item that match the specified group and item identifiers.
    /// </summary>
    private (ChartSerie serie, ChartItem item)? Resolve(string groupId, string itemId)
    {
        foreach (var serie in _seriesProvider())
        {
            if (!string.Equals(serie.Id, groupId, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var item = serie.RawItems.FirstOrDefault(i => i.Id == itemId);

            if (item != null)
            {
                return (serie, item);
            }
        }

        return null;
    }

    /// <summary>
    /// Computes the animation trigger corresponding to a transition between two interaction states.
    /// </summary>
    private static ChartAnimationTrigger GetTriggerForTransition(
        ChartInteractionState previous,
        ChartInteractionState next)
    {
        if (previous == next)
        {
            return ChartAnimationTrigger.None;
        }

        return (previous, next) switch
        {
            (ChartInteractionState.Normal, ChartInteractionState.Hover) => ChartAnimationTrigger.HoverIn,
            (ChartInteractionState.Hover, ChartInteractionState.Normal) => ChartAnimationTrigger.HoverOut,

            (ChartInteractionState.Normal, ChartInteractionState.Pressed) => ChartAnimationTrigger.PressIn,
            (ChartInteractionState.Pressed, ChartInteractionState.Normal) => ChartAnimationTrigger.PressOut,

            (ChartInteractionState.Normal, ChartInteractionState.Selected) => ChartAnimationTrigger.SelectIn,
            (ChartInteractionState.Selected, ChartInteractionState.Normal) => ChartAnimationTrigger.SelectOut,

            (ChartInteractionState.Disabled, ChartInteractionState.Normal) => ChartAnimationTrigger.Enable,
            (ChartInteractionState.Normal, ChartInteractionState.Disabled) => ChartAnimationTrigger.Disable,

            _ => ChartAnimationTrigger.None
        };
    }

    /// <summary>
    /// Sets the interaction state of an item and updates its animation trigger if applicable.
    /// </summary>
    private static bool SetInteractionState(ChartItem item, ChartInteractionState newState)
    {
        var previous = item.InteractionState;

        if (previous == newState)
        {
            item.Trigger = ChartAnimationTrigger.None;
            return false;
        }

        item.InteractionState = newState;

        var trigger = GetTriggerForTransition(previous, newState);

        if (trigger == ChartAnimationTrigger.None)
        {
            item.Trigger = ChartAnimationTrigger.None;

            return false;
        }

        item.Trigger = trigger;

        return true;
    }

    /// <summary>
    /// Attempts to retrieve a tooltip chart item for the specified group and item identifiers.
    /// </summary>
    /// <param name="groupId">The identifier of the group.</param>
    /// <param name="itemId">The identifier of the item.</param>
    /// <param name="item">When this method returns, contains the chart item if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the chart item was found; otherwise, <see langword="false"/>.</returns>
    public bool TryGetTooltip(string groupId, string itemId, out ChartItem? item)
    {
        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            item = null;
            return false;
        }

        var (_, target) = resolved.Value;
        item = target;

        return true;
    }

    /// <summary>
    /// Sets the hover interaction state for the specified chart item and clears any previous hover states.
    /// </summary>
    public bool Hover(
        string groupId,
        string itemId)
    {
        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            return false;
        }

        var (_, target) = resolved.Value;
        var changed = false;

        foreach (var serie in _seriesProvider())
        {
            foreach (var item in serie.RawItems)
            {
                if (item == target)
                {
                    continue;
                }

                if (item.InteractionState == ChartInteractionState.Selected)
                {
                    continue;
                }

                if (item.InteractionState == ChartInteractionState.Hover)
                {
                    item.Trigger = ChartAnimationTrigger.HoverOut;
                    item.InteractionState = ChartInteractionState.Normal;
                    changed = true;
                }
            }
        }

        if (target.InteractionState == ChartInteractionState.Selected)
        {
            return changed;
        }

        if (target.InteractionState != ChartInteractionState.Hover)
        {
            target.Trigger = ChartAnimationTrigger.HoverIn;
            target.InteractionState = ChartInteractionState.Hover;
            changed = true;
        }

        return changed;
    }

    /// <summary>
    /// Attempts to set the interaction state of the specified chart item to pressed.
    /// </summary>
    public bool Press(string groupId, string itemId)
    {
        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            return false;
        }

        var (_, item) = resolved.Value;

        return SetInteractionState(item, ChartInteractionState.Pressed);
    }

    /// <summary>
    /// Attempts to select the specified item within the given group.
    /// </summary>
    public bool Select(string groupId, string itemId)
    {
        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            return false;
        }

        var (_, target) = resolved.Value;
        var changed = false;

        if (target.InteractionState == ChartInteractionState.Selected)
        {
            target.Trigger = ChartAnimationTrigger.SelectOut;
            target.InteractionState = ChartInteractionState.Normal;
            return true;
        }

        foreach (var serie in _seriesProvider())
        {
            foreach (var item in serie.RawItems)
            {
                if (item == target)
                {
                    continue;
                }

                if (item.InteractionState == ChartInteractionState.Selected)
                {
                    item.Trigger = ChartAnimationTrigger.SelectOut;
                    item.InteractionState = ChartInteractionState.Normal;
                    changed = true;
                }
            }
        }

        target.Trigger = ChartAnimationTrigger.SelectIn;
        target.InteractionState = ChartInteractionState.Selected;
        changed = true;

        return changed;
    }

    /// <summary>
    /// Handles pointer entering a specific item (semantic alias for Hover).
    /// </summary>
    /// <param name="groupId">The identifier of the group (series) containing the item.</param>
    /// <param name="itemId">The identifier of the item being hovered over.</param>
    public bool PointerEnter(
        string groupId,
        string itemId)
    {
        return Hover(groupId, itemId);
    }

    /// <summary>
    /// Handles pointer leaving a specific item.
    /// </summary>
    /// <returns>Returns true if any interaction state was changed as a result of this operation; otherwise, false.</returns>
    public bool PointerLeave()
    {
        var changed = false;

        foreach (var serie in _seriesProvider())
        {
            foreach (var item in serie.RawItems)
            {
                if (item.InteractionState == ChartInteractionState.Selected)
                {
                    continue;
                }

                if (item.InteractionState == ChartInteractionState.Hover)
                {
                    item.Trigger = ChartAnimationTrigger.HoverOut;
                    item.InteractionState = ChartInteractionState.Normal;
                    changed = true;
                }
            }
        }

        return changed;
    }

    /// <summary>
    /// Handles pointer down on a specific item (press in).
    /// </summary>
    public bool PointerDown(
        string groupId,
        string itemId) => Press(groupId, itemId);

    /// <summary>
    /// Handles pointer up on a specific item (press out, typically back to normal).
    /// </summary>
    public bool PointerUp(string groupId, string itemId)
    {
        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            return false;
        }

        var (_, item) = resolved.Value;

        if (item.InteractionState == ChartInteractionState.Pressed)
        {
            var result = SetInteractionState(item, ChartInteractionState.Normal);

            return result;
        }

        return false;
    }

    /// <summary>
    /// Updates the tooltip position and mode for a specific chart item, if applicable.
    /// </summary>
    /// <param name="groupId">The identifier of the group (series) containing the item.</param>
    /// <param name="itemId">The identifier of the item for which the tooltip is being updated.</param>
    /// <param name="position">The new position of the tooltip.</param>
    /// <param name="tooltipPosition">The position mode of the tooltip.</param>
    /// <param name="tooltipMode">The mode of the tooltip.</param>
    internal void UpdateTooltip(
        string groupId,
        string itemId,
        ChartPoint position,
        ChartTooltipPlacement tooltipPosition,
        ChartTooltipMode tooltipMode)
    {
        if (tooltipMode == ChartTooltipMode.None)
        {
            return;
        }

        var resolved = Resolve(groupId, itemId);

        if (resolved is null)
        {
            return;
        }

        var (_, item) = resolved.Value;

        if (item.Tooltip is null)
        {
            return;
        }

        item.Tooltip = item.Tooltip with
        {
            Position = position,
            Placement = tooltipPosition,
            Visible = true,
            Text = item is CategoryItem ci ? $"Value: {ci.Value}" : string.Empty,
            Title = item is CategoryItem ci2 ? $"Category: {ci2.Category}" : string.Empty
        };
    }

    /// <summary>
    /// Hides all tooltips for all chart items across all series.
    /// </summary>
    internal void HideTooltips()
    {
        foreach (var serie in _seriesProvider())
        {
            foreach (var item in serie.RawItems)
            {
                item.Tooltip?.Visible = false;
            }
        }
    }
}
