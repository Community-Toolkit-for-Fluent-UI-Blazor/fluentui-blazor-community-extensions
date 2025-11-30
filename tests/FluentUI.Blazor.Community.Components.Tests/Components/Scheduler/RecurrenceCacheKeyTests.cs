namespace FluentUI.Blazor.Community.Components.Tests.Components.Scheduler;

public class RecurrenceCacheKeyTests
{
    private static RecurrenceRule CreateRule(
        RecurrenceFrequency freq = RecurrenceFrequency.Daily,
        int interval = 1,
        DateTime? until = null,
        int? count = null,
        IEnumerable<DayOfWeek>? days = null,
        IEnumerable<int>? months = null,
        int? dayOfMonth = null)
    {
        var rule = new RecurrenceRule
        {
            Frequency = freq,
            Interval = interval,
            Until = until,
            Count = count,
            DayOfMonth = dayOfMonth
        };

        if (days is not null)
        {
            rule.DaysOfWeek.AddRange(days);
        }

        if (months is not null)
        {
            rule.Months.AddRange(months);
        }

        return rule;
    }

    [Fact]
    public void Equals_SameValues_AreEqualAndHaveSameHash()
    {
        var itemId = 42L;
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(7);
        var anchor = from;
        var rule = CreateRule(RecurrenceFrequency.Weekly, interval: 2, days: new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
        var exceptions = new[] { from.AddDays(1), from.AddDays(3) };

        var key1 = new RecurrenceCacheKey(itemId, from, to, anchor, rule, exceptions);
        var key2 = new RecurrenceCacheKey(itemId, from, to, anchor, rule, new List<DateTime>(exceptions));

        Assert.True(key1.Equals(key2));
        Assert.True(key1 == key2);
        Assert.False(key1 != key2);
        Assert.Equal(key1.GetHashCode(), key2.GetHashCode());
        Assert.True(key1.Equals((object)key2));
    }

    [Fact]
    public void DifferentItemId_AreNotEqual()
    {
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(1);
        var anchor = from;
        var rule = CreateRule();

        var key1 = new RecurrenceCacheKey(1, from, to, anchor, rule, null);
        var key2 = new RecurrenceCacheKey(2, from, to, anchor, rule, null);

        Assert.False(key1.Equals(key2));
        Assert.False(key1 == key2);
        Assert.True(key1 != key2);
    }

    [Fact]
    public void DifferentRule_AreNotEqual()
    {
        var itemId = 10L;
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(10);
        var anchor = from;

        var rule1 = CreateRule(RecurrenceFrequency.Daily);
        var rule2 = CreateRule(RecurrenceFrequency.Monthly);

        var key1 = new RecurrenceCacheKey(itemId, from, to, anchor, rule1, null);
        var key2 = new RecurrenceCacheKey(itemId, from, to, anchor, rule2, null);

        Assert.False(key1.Equals(key2));
        Assert.NotEqual(key1.GetHashCode(), key2.GetHashCode());
    }

    [Fact]
    public void Exceptions_NullAndEmpty_AreTreatedEqually()
    {
        var itemId = 7L;
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(2);
        var anchor = from;
        var rule = CreateRule();

        var keyWithNull = new RecurrenceCacheKey(itemId, from, to, anchor, rule, null);
        var keyWithEmpty = new RecurrenceCacheKey(itemId, from, to, anchor, rule, new List<DateTime>());

        Assert.True(keyWithNull.Equals(keyWithEmpty));
        Assert.Equal(keyWithNull.GetHashCode(), keyWithEmpty.GetHashCode());
    }

    [Fact]
    public void Equals_ObjectOfDifferentType_ReturnsFalse()
    {
        var itemId = 1L;
        var from = DateTime.UtcNow.Date;
        var to = from.AddDays(1);
        var anchor = from;
        var rule = CreateRule();

        var key = new RecurrenceCacheKey(itemId, from, to, anchor, rule, null);

        Assert.False(key.Equals("not-a-key"));
    }
}
