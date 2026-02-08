using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleStateChangedEventArgsTests
{
    [Fact]
    public void Constructor_SetsKind()
    {
        var args = new ConsoleStateChangedEventArgs(ConsoleChangeKind.FilterChanged);

        Assert.Equal(ConsoleChangeKind.FilterChanged, args.Kind);
    }
}
