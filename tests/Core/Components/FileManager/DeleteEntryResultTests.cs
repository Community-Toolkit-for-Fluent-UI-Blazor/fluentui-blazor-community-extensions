using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class DeleteEntryResultTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var result = new DeleteEntryResult("id")
        {
            Success = true,
            ErrorMessage = "error"
        };

        Assert.Equal("id", result.Id);
        Assert.True(result.Success);
        Assert.Equal("error", result.ErrorMessage);
    }
}
