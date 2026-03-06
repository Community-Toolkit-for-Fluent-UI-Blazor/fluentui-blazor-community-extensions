using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class StreamUploadedEventArgsTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var candidate = new UploadFileCandidate { Index = 1, Name = "file.txt", Size = 10 };
        var args = new StreamUploadedEventArgs("parent", candidate)
        {
            Descriptor = new UploadFileDescriptor(
                "id",
                "file.txt",
                10,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                () => Task.FromResult(new byte[] { 1 }))
        };

        Assert.Equal("parent", args.ParentId);
        Assert.Same(candidate, args.File);
        Assert.NotNull(args.Descriptor);
    }
}
