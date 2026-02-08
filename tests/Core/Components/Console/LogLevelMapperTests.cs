using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class LogLevelMapperTests
{
    [Theory]
    [InlineData(LogLevel.Trace, ConsoleLevel.Trace)]
    [InlineData(LogLevel.Debug, ConsoleLevel.Debug)]
    [InlineData(LogLevel.Information, ConsoleLevel.Information)]
    [InlineData(LogLevel.Warning, ConsoleLevel.Warning)]
    [InlineData(LogLevel.Error, ConsoleLevel.Error)]
    [InlineData(LogLevel.Critical, ConsoleLevel.Critical)]
    public void ToConsoleLevel_MapsCorrectly(LogLevel logLevel, ConsoleLevel expected)
    {
        var result = LogLevelMapper.ToConsoleLevel(logLevel);

        Assert.Equal(expected, result);
    }
}
