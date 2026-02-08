using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleExportOptionsTests
{
    [Fact]
    public void FromDateTime_SetsFromOffset()
    {
        var options = new ConsoleExportOptions();
        var date = new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        options.FromDateTime = date;

        Assert.Equal(date, options.From?.UtcDateTime);
    }

    [Fact]
    public void ToDateTime_ClearsToOffset_WhenNull()
    {
        var options = new ConsoleExportOptions
        {
            To = DateTimeOffset.UtcNow
        };

        options.ToDateTime = null;

        Assert.Null(options.To);
    }
}
