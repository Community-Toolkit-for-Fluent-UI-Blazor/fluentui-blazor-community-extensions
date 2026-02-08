using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleChangeKindTests
{
    [Fact]
    public void ConsoleChangeKind_Values_AreExpected()
    {
        var values = Enum.GetValues<ConsoleChangeKind>();

        Assert.Equal(
            [
                ConsoleChangeKind.MessageAdded,
                ConsoleChangeKind.FilterChanged,
                ConsoleChangeKind.Cleared,
                ConsoleChangeKind.BatchCompleted
            ],
            values);
    }
}
