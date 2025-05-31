// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Comparers;

internal sealed class FluentCxTileGridItemComparer<TItem>
    : IComparer<FluentCxTileGridItem<TItem>> where TItem : class, new()
{
    public static FluentCxTileGridItemComparer<TItem> Default { get; } = new();

    /// <inheritdoc />
    public int Compare(FluentCxTileGridItem<TItem>? x, FluentCxTileGridItem<TItem>? y)
    {
        if (x is null)
        {
            return y is null ? 0 : 1;
        }

        if (y is null)
        {
            return x is null ? 0 : -1;
        }

        return x.Order.CompareTo(y.Order);
    }
}
