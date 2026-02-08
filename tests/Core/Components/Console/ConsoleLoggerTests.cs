using FluentUI.Blazor.Community.Components;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleLoggerTests
{
    [Fact]
    public async Task Log_WritesToConsoleWriter()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var writer = new TestConsoleWriter();
        var logger = new ConsoleLogger(writer, "TestCategory");
        var state = new Dictionary<string, object?> { ["Key"] = "Value" };

        logger.Log(LogLevel.Warning, new EventId(5, "Event"), state, null, (s, _) => "Message");

        var write = await writer.WriteTask.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken);

        Assert.Equal(ConsoleLevel.Warning, write.Level);
        Assert.Equal("Message", write.Message);
        Assert.Equal("TestCategory", write.Context?.Category);
        Assert.Equal(5, write.Context?.Properties?["EventId"]);
        Assert.Equal("Event", write.Context?.Properties?["EventName"]);
        Assert.Equal("Value", write.Context?.Properties?["Key"]);
    }

    [Fact]
    public void BeginScope_UsesStateProperties()
    {
        var writer = new TestConsoleWriter();
        var logger = new ConsoleLogger(writer, "TestCategory");
        var state = new Dictionary<string, object?> { ["Scope"] = "Value" };

        using var scope = logger.BeginScope(state);

        Assert.Equal("Value", writer.LastScope?.Properties?["Scope"]);
    }

    private sealed class TestConsoleWriter : IConsoleWriter
    {
        private readonly TaskCompletionSource<WriteRecord> _tcs = new();

        public Task<WriteRecord> WriteTask => _tcs.Task;

        public ConsoleWriteContext? LastScope { get; private set; }

        public Task WriteAsync(ConsoleLevel level, string message, Exception? exception = null, ConsoleWriteContext? context = null)
        {
            _tcs.TrySetResult(new WriteRecord(level, message, exception, context));
            return Task.CompletedTask;
        }

        public Task TraceAsync(string message, ConsoleWriteContext? context = null) => WriteAsync(ConsoleLevel.Trace, message, null, context);

        public Task DebugAsync(string message, ConsoleWriteContext? context = null) => WriteAsync(ConsoleLevel.Debug, message, null, context);

        public Task InfoAsync(string message, ConsoleWriteContext? context = null) => WriteAsync(ConsoleLevel.Information, message, null, context);

        public Task WarningAsync(string message, ConsoleWriteContext? context = null) => WriteAsync(ConsoleLevel.Warning, message, null, context);

        public Task ErrorAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null)
            => WriteAsync(ConsoleLevel.Error, message, exception, context);

        public Task CriticalAsync(string message, Exception? exception = null, ConsoleWriteContext? context = null)
            => WriteAsync(ConsoleLevel.Critical, message, exception, context);

        public IDisposable BeginScope(ConsoleWriteContext context)
        {
            LastScope = context;
            return new Scope();
        }

        private sealed class Scope : IDisposable
        {
            public void Dispose() { }
        }

        public sealed record WriteRecord(ConsoleLevel Level, string Message, Exception? Exception, ConsoleWriteContext? Context);
    }
}
