using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleOptionsTests
{
    [Fact]
    public void Defaults_AreConfigured()
    {
        var options = new ConsoleOptions();

        Assert.Equal(10_000, options.MaxMessages);
        Assert.True(options.AutoScroll);
        Assert.True(options.EnableBatching);
        Assert.Equal(TimeSpan.FromMilliseconds(100), options.BatchInterval);
        Assert.True(options.StockExceptions);
        Assert.True(options.EnableThreadSafety);
    }

    [Fact]
    public void CanConfigureOptions()
    {
        var options = new ConsoleOptions
        {
            MaxMessages = 5000,
            AutoScroll = false,
            EnableBatching = false,
            BatchInterval = TimeSpan.FromSeconds(1),
            StockExceptions = false,
            EnableThreadSafety = false
        };

        Assert.Equal(5000, options.MaxMessages);
        Assert.False(options.AutoScroll);
        Assert.False(options.EnableBatching);
        Assert.Equal(TimeSpan.FromSeconds(1), options.BatchInterval);
        Assert.False(options.StockExceptions);
        Assert.False(options.EnableThreadSafety);
    }
}
