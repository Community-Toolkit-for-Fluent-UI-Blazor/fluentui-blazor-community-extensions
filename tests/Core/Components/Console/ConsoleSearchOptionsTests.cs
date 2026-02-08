using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleSearchOptionsTests
{
    [Fact]
    public void Defaults_AreEnabled()
    {
        var options = new ConsoleSearchOptions();

        Assert.True(options.IndexMessage);
        Assert.True(options.IndexCategory);
        Assert.True(options.IndexSource);
        Assert.True(options.IndexException);
        Assert.True(options.IndexProperties);
        Assert.True(options.IndexTags);
    }

    [Fact]
    public void Top_AllowsLimitingResults()
    {
        var options = new ConsoleSearchOptions { Top = 5 };

        Assert.Equal(5, options.Top);
    }
}
