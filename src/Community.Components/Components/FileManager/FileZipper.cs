using System.IO.Compression;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a file zipper.
/// </summary>
internal static class FileZipper
{
    /// <summary>
    /// Gets an array of <see cref="byte"/> which represents the data of a file in an asynchronous way.
    /// </summary>
    /// <param name="path">The file to open to get the data.</param>
    /// <returns>Returns a <see cref="Task{TResult}"/> which contains the array of byte of the file.</returns>
    private static async Task<byte[]> GetFileDataAsync(string path)
    {
        if (File.Exists(path))
        {
            var data = await File.ReadAllBytesAsync(path);
            File.Delete(path);

            return data;
        }

        return [];
    }

    /// <summary>
    /// Zip a collection of files.
    /// </summary>
    /// <typeparam name="TItem">Type of the image.</typeparam>
    /// <param name="folder">Destination folder where we transfert the files.</param>
    /// <param name="entries">Entries to zip.</param>
    /// <returns>Returns a task which zip the entries.</returns>
    private static async Task ZipInternalAsync<TItem>(string folder, IEnumerable<FileManagerEntry<TItem>> entries) where TItem : class, new()
    {
        static void CreateDirectoryIfNotExists(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        CreateDirectoryIfNotExists(folder);

        foreach (var entry in entries)
        {
            if (entry.IsDirectory)
            {
                var currentFolder = $"{folder}\\{entry.Name}";
                CreateDirectoryIfNotExists(currentFolder);

                foreach (var directoryEntry in entry.GetDirectories())
                {
                    await ZipInternalAsync($"{currentFolder}\\{directoryEntry.Name}", directoryEntry.Enumerate());
                }

                foreach (var item in entry.GetFiles())
                {
                    var data = await item.GetBytesAsync();
                    await File.WriteAllBytesAsync($"{currentFolder}\\{item.Name}", data);
                }
            }
            else
            {
                var data = await entry.GetBytesAsync();
                await File.WriteAllBytesAsync($"{folder}\\{entry.Name}", data);
            }
        }
    }

    /// <summary>
    /// Zip the entries in an asynchronous way.
    /// </summary>
    /// <typeparam name="TItem">Type of the item.</typeparam>
    /// <param name="entries">Entries to zip.</param>
    /// <returns>Returns a <see cref="Task{TResult}" /> which contains a <see cref="FileManagerEntry{TItem}"/>
    ///  which contains the zipped file.</returns>
    public static async Task<FileManagerEntry<TItem>?> ZipAsync<TItem>(
        IEnumerable<FileManagerEntry<TItem>> entries) where TItem : class, new()
    {
        if (entries.Any())
        {
            var path = Path.GetTempPath();
            var folder = $"{Path.GetTempPath()}FileManager";
            var archiveFileName = $"archive_{DateTime.Now:yyyyMMdd_HHmmss}.zip";
            var fullPath = $"{path}{archiveFileName}";

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            Directory.CreateDirectory(folder);

            await ZipInternalAsync(folder, entries);

            ZipFile.CreateFromDirectory(folder, fullPath);

            Directory.Delete(folder, true);

            return FileManagerEntry<TItem>.CreateEntry(
                async () => await GetFileDataAsync(fullPath),
                archiveFileName,
                new FileInfo(fullPath).Length);
        }

        return null;
    }
}
