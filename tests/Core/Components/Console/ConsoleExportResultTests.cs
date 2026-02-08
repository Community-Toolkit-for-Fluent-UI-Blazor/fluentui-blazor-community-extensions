using FluentUI.Blazor.Community.Components;
using Xunit;

namespace FluentUI.Blazor.Community.Tests.Components.Console;

public class ConsoleExportResultTests
{
    [Fact]
    public void ConsoleExportResult_Defaults_AreEmpty()
    {
        var result = new ConsoleExportResult();

        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.ContentType);
        Assert.Empty(result.Content);
    }
}
