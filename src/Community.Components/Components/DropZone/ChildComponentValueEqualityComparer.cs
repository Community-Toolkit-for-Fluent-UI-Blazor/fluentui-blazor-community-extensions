// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

internal sealed class ChildComponentValueEqualityComparer<TItem>
    : IEqualityComparer<FluentComponentBase>
{
    public static ChildComponentValueEqualityComparer<TItem> Default { get; } = new();

    public bool Equals(FluentComponentBase? x, FluentComponentBase? y)
    {
        if (x is null)
        {
            return y is null;
        }

        if (y is null)
        {
            return x is null;
        }

        if (x is IItemValue<TItem> a && y is IItemValue<TItem> b)
        {
            return EqualityComparer<TItem>.Default.Equals(a.Value, b.Value);
        }

        return false;
    }

    public int GetHashCode([DisallowNull] FluentComponentBase obj)
    {
        if (obj is IItemValue<TItem> a && a.Value is not null)
        {
            return a.Value.GetHashCode();
        }

        return -1;
    }
}
