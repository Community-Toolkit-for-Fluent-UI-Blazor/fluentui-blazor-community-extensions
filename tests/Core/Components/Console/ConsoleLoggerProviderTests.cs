using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Enums;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleLoggerProviderTests
{
    [Fact]
    public void CreateLogger_ReturnsConsoleLogger()
    {
        var provider = new ConsoleLoggerProvider(new TestConsoleWriter());

        var logger = provider.CreateLogger("Test");

        Assert.IsType<ConsoleLogger>(logger);
    }

    private sealed class TestConsoleWriter : IConsoleWriter
    {
        public Task WriteAsync(ConsoleLevel level, string message, Exception? exception = null, ConsoleWriteContext? context = null)
            => Task.CompletedTask;

        public Task TraceAsync(string message, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public Task DebugAsync(string message, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public Task InfoAsync(string message, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public Task WarningAsync(string message, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public Task ErrorAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public Task CriticalAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null) => Task.CompletedTask;

        public IDisposable BeginScope(ConsoleWriteContext context) => new Scope();

        private sealed class Scope : IDisposable
        {
            public void Dispose() { }
        }
    }
}
