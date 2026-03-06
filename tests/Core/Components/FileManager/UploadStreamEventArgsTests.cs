using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class UploadStreamEventArgsTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        var buffer = new byte[] { 1, 2, 3 };
        var args = new UploadStreamEventArgs(1, "file.txt", buffer);

        Assert.Equal(1, args.Index);
        Assert.Equal("file.txt", args.Name);
        Assert.Same(buffer, args.Buffer);
    }
}
