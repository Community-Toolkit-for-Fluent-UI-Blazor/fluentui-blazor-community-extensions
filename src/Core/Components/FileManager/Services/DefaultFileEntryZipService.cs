using System.IO.Compression;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Components.FileManager.Services;

/// <summary>
/// Provides a default implementation of a service that creates ZIP archives from collections of file entries of a
/// specified type.
/// </summary>
/// <remarks>This service is intended for use cases where file entries, potentially including directories and
/// nested files, need to be packaged into a ZIP archive. The resulting archive is represented as a new file entry
/// containing the ZIP data. The service creates temporary directories and files during the zipping process, which are
/// cleaned up automatically after the archive is created.</remarks>
/// <typeparam name="TItem">The type of the item associated with each file entry. Must be a reference type with a parameterless constructor.</typeparam>
internal sealed class DefaultFileEntryZipService<TItem> : IFileEntryZipService<TItem>
    where TItem : class, new()
{
    /// <inheritdoc />
    public async ValueTask<FileEntry<TItem>> ZipAsync(IEnumerable<FileEntry<TItem>> entries)
    {
        var tempFolder = Path.Combine(Path.GetTempPath(), "FileManagerZip_" + Guid.NewGuid());
        Directory.CreateDirectory(tempFolder);

        await CopyEntriesAsync(entries, tempFolder);

        var zipPath = Path.Combine(Path.GetTempPath(), $"archive_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
        ZipFile.CreateFromDirectory(tempFolder, zipPath);

        Directory.Delete(tempFolder, true);

        return new FileEntry<TItem>(
            id: Identifier.NewId(),
            name: Path.GetFileName(zipPath),
            isDirectory: false,
            size: new FileInfo(zipPath).Length,
            createdDate: DateTime.Now,
            modifiedDate: DateTime.Now,
            item: new TItem())
        {
            DataProviderAsync = async () => await File.ReadAllBytesAsync(zipPath)
        };
    }

    /// <summary>
    /// Asynchronously copies the specified file and directory entries to the given folder, preserving the directory
    /// structure.
    /// </summary>
    /// <remarks>If an entry is a directory, its contents are recursively copied. If an entry provides a data
    /// provider delegate, it is used to obtain the file data; otherwise, an empty file is created. Existing files in
    /// the target folder with the same name will be overwritten.</remarks>
    /// <param name="entries">The collection of file and directory entries to copy. Each entry may represent a file or a directory and may
    /// contain child entries if it is a directory.</param>
    /// <param name="folder">The target folder path where the entries will be copied. If the folder does not exist, it will be created.</param>
    /// <returns>A task that represents the asynchronous copy operation.</returns>
    private static async Task CopyEntriesAsync(
        IEnumerable<FileEntry<TItem>> entries,
        string folder)
    {
        foreach (var entry in entries)
        {
            if (entry.IsDirectory)
            {
                var dir = Path.Combine(folder, entry.Name);
                Directory.CreateDirectory(dir);
                await CopyEntriesAsync(entry.Children, dir);
            }
            else
            {
                var data = entry.DataProviderAsync != null
                    ? await entry.DataProviderAsync()
                    : entry.DataProvider?.Invoke() ?? [];

                await File.WriteAllBytesAsync(Path.Combine(folder, entry.Name), data);
            }
        }
    }
}
