using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a unique key for identifying cached recurrence data for a specific item and date range.
/// </summary>
/// <remarks>Use this key to efficiently retrieve or store recurrence information in a cache for a given item and
/// time interval. The combination of item identifier and date range ensures uniqueness for each cached entry.</remarks>
public readonly struct RecurrenceCacheKey : IEquatable<RecurrenceCacheKey>
{
    /// <summary>
    /// Internal hash code for the cache key.
    /// </summary>
    private readonly int _hashCode;

    /// <summary>
    /// Gets the unique identifier for the item.
    /// </summary>
    public long ItemId { get; }

    /// <summary>
    /// Gets the starting date and time for the range or period represented by this instance.
    /// </summary>
    public long From { get; }

    /// <summary>
    /// Gets the end date and time of the range represented by this instance.
    /// </summary>
    public long To { get; }

    /// <summary>
    /// Gets the starting anchor point as a date and time value.
    /// </summary>
    public long AnchorStart { get; }

    /// <summary>
    /// Gets the hash code representing the current rule configuration.
    /// </summary>
    public int RuleHash { get; }

    /// <summary>
    /// Gets the hash code representing the current set of exceptions.
    /// </summary>
    public int ExceptionsHash { get; }

    /// <summary>
    /// Initializes a new instance of the RecurrenceCacheKey class using the specified item identifier, date range,
    /// anchor start date, recurrence rule, and exception dates.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item for which the recurrence cache key is generated.</param>
    /// <param name="from">The start date and time of the recurrence range to be represented by the cache key.</param>
    /// <param name="to">The end date and time of the recurrence range to be represented by the cache key.</param>
    /// <param name="anchorStart">The anchor start date and time used to calculate the recurrence pattern.</param>
    /// <param name="rule">The recurrence rule that defines the pattern for generating occurrences.</param>
    /// <param name="exceptions">A collection of dates to be excluded from the recurrence pattern. Can be null if there are no exceptions.</param>
    public RecurrenceCacheKey(long itemId, DateTime from, DateTime to,
                              DateTime anchorStart, RecurrenceRule rule,
                              IEnumerable<DateTime>? exceptions)
    {
        ItemId = itemId;
        From = from.Ticks;
        To = to.Ticks;
        AnchorStart = anchorStart.Ticks;
        RuleHash = HashRecurrence(rule);
        ExceptionsHash = HashExceptions(exceptions);

        unchecked
        {
            var hashcode = new HashCode();
            hashcode.Add(ItemId);
            hashcode.Add(From);
            hashcode.Add(To);
            hashcode.Add(AnchorStart);
            hashcode.Add(RuleHash);
            hashcode.Add(ExceptionsHash);

            _hashCode = hashcode.ToHashCode();
        }
    }

    /// <summary>
    /// Calculates a hash code for the specified recurrence rule based on its frequency, interval, and other recurrence
    /// parameters.
    /// </summary>
    /// <remarks>This method combines all relevant recurrence properties to produce a hash code suitable for
    /// use in collections or comparisons. The hash code reflects the values of frequency, interval, until date, count,
    /// day of month, days of week, and months in the rule.</remarks>
    /// <param name="rule">The recurrence rule to compute the hash code for. Cannot be null.</param>
    /// <returns>An integer representing the hash code of the recurrence rule.</returns>
    private static int HashRecurrence(RecurrenceRule rule)
    {
        unchecked
        {
            var hashcode = new HashCode();
            hashcode.Add(rule.Frequency);
            hashcode.Add(rule.Interval);
            hashcode.Add(rule.Until);
            hashcode.Add(rule.Count);
            hashcode.Add(rule.DayOfMonth);

            foreach (var d in rule.DaysOfWeek)
            {
                hashcode.Add(d);
            }

            foreach (var m in rule.Months)
            {
                hashcode.Add(m);
            }

            return hashcode.ToHashCode();
        }
    }

    /// <summary>
    /// Calculates a hash code for a sequence of exception dates.
    /// </summary>
    /// <param name="exceptions">A collection of <see cref="DateTime"/> values representing exception dates to include in the hash calculation.
    /// If <see langword="null"/>, the method returns 0.</param>
    /// <returns>An integer hash code representing the provided exception dates. Returns 0 if <paramref name="exceptions"/> is
    /// <see langword="null"/>.</returns>
    private static int HashExceptions(IEnumerable<DateTime>? exceptions)
    {
        if (exceptions == null || !exceptions.Any())
        {
            return 0;
        }

        unchecked
        {
            var hashcode = new HashCode();

            foreach (var ex in exceptions)
            {
                hashcode.Add(ex);
            }

            return hashcode.ToHashCode();
        }
    }

    /// <inheritdoc/>
    public bool Equals(RecurrenceCacheKey other)
    {
        return ItemId == other.ItemId &&
               From == other.From &&
               To == other.To &&
               AnchorStart == other.AnchorStart &&
               RuleHash == other.RuleHash &&
               ExceptionsHash == other.ExceptionsHash;
    }

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is RecurrenceCacheKey other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <summary>
    /// Determines whether two <see cref="RecurrenceCacheKey"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="RecurrenceCacheKey"/> to compare.</param>
    /// <param name="right">The second <see cref="RecurrenceCacheKey"/> to compare.</param>
    /// <returns>true if the specified <see cref="RecurrenceCacheKey"/> instances are equal; otherwise, false.</returns>
    public static bool operator ==(RecurrenceCacheKey left, RecurrenceCacheKey right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="RecurrenceCacheKey"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="RecurrenceCacheKey"/> to compare.</param>
    /// <param name="right">The second <see cref="RecurrenceCacheKey"/> to compare.</param>
    /// <returns>true if the specified <see cref="RecurrenceCacheKey"/> instances are not equal; otherwise, false.</returns>
    public static bool operator !=(RecurrenceCacheKey left, RecurrenceCacheKey right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return _hashCode;
    }
}
