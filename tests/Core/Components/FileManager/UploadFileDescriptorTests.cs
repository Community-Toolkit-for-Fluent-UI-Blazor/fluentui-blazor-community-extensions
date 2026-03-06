using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class UploadFileDescriptorTests
{
    [Fact]
    public void Properties_AreSettable()
    {
        static Task<byte[]> GetBytesAsync() => Task.FromResult(new byte[] { 1, 2 });
        var created = DateTimeOffset.UtcNow;
        var modified = DateTimeOffset.UtcNow;

        var descriptor = new UploadFileDescriptor("id", "file.txt", 10, created, modified, GetBytesAsync);

        Assert.Equal("id", descriptor.Id);
        Assert.Equal("file.txt", descriptor.Name);
        Assert.Equal(10, descriptor.Size);
        Assert.Equal(created, descriptor.CreatedDate);
        Assert.Equal(modified, descriptor.ModifiedDate);
        Assert.NotNull(descriptor.GetBytesAsync);
    }
}
