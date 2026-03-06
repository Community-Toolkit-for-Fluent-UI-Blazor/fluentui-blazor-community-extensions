using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class FileManagerProgressStateTests
{
    [Fact]
    public void None_IsZero()
    {
        Assert.Equal(0, (int)FileManagerProgressState.None);
    }

    [Fact]
    public void States_HaveExpectedValues()
    {
        var expectations = new[]
        {
            (FileManagerProgressState.Uploading, 1),
            (FileManagerProgressState.Downloading, 2),
            (FileManagerProgressState.Deleting, 3),
            (FileManagerProgressState.Moving, 4),
            (FileManagerProgressState.Creation, 5),
            (FileManagerProgressState.Renaming, 6),
            (FileManagerProgressState.Searching, 7)
        };

        foreach (var (state, expected) in expectations)
        {
            Assert.Equal(expected, (int)state);
        }
    }
}
