using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleLevelTests
{
    [Fact]
    public void ConsoleLevel_Values_AreExpected()
    {
        var values = Enum.GetValues<ConsoleLevel>();

        Assert.Equal(
            [
                ConsoleLevel.Trace,
                ConsoleLevel.Debug,
                ConsoleLevel.Information,
                ConsoleLevel.Warning,
                ConsoleLevel.Error,
                ConsoleLevel.Critical
            ],
            values);
    }
}
