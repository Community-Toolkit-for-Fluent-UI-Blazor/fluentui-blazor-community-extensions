using System.Diagnostics.CodeAnalysis;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Internal;

/// <summary>
/// Represents the comparer for a value item of a component.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
/// <remarks>The component must implement <see cref="IItemValue{TItem}"/>.</remarks>
internal sealed class ChildComponentValueEqualityComparer<TItem>
    : IEqualityComparer<FluentComponentBase>
{
    /// <summary>
    /// Gets the default instance of the comparer.
    /// </summary>
    public static ChildComponentValueEqualityComparer<TItem> Default { get; } = new();

    /// <inheritdoc />
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

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] FluentComponentBase obj)
    {
        if (obj is IItemValue<TItem> a && a.Value is not null)
        {
            return a.Value.GetHashCode();
        }

        return -1;
    }
}
