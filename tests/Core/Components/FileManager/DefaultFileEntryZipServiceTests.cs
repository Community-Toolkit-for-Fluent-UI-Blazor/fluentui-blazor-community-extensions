using System;
using System.IO;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Components.FileManager.Services;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class DefaultFileEntryZipServiceTests
{
    private sealed class DummyItem
    {
    }

    [Fact]
    public async Task ZipAsync_CreatesArchiveEntry()
    {
        var service = new DefaultFileEntryZipService<DummyItem>();
        var entry = new FileEntry<DummyItem>(
            id: "file",
            name: "file.txt",
            isDirectory: false,
            size: 3,
            createdDate: DateTime.UtcNow,
            modifiedDate: DateTime.UtcNow,
            item: new DummyItem())
        {
            DataProvider = () => new byte[] { 1, 2, 3 }
        };

        var result = await service.ZipAsync([entry]);
        var zipPath = Path.Combine(Path.GetTempPath(), result.Name);

        try
        {
            Assert.False(result.IsDirectory);
            Assert.EndsWith(".zip", result.Name, StringComparison.OrdinalIgnoreCase);
            var dataProvider = result.DataProviderAsync;

            Assert.NotNull(dataProvider);

            var bytes = await dataProvider();
            Assert.NotEmpty(bytes);
        }
        finally
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
        }
    }
}
