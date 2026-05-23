using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class UploadFileCandidateTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var candidate = new UploadFileCandidate
        {
            Index = 2,
            Name = "file.txt",
            Size = 10
        };

        Assert.Equal(2, candidate.Index);
        Assert.Equal("file.txt", candidate.Name);
        Assert.Equal(10, candidate.Size);
    }
}
