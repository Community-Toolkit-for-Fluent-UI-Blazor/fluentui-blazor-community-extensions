// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.IO.Compression;

namespace FluentUI.Blazor.Community.Components;

internal static class FileZipper
{
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
                string currentFolder = $"{folder}\\{entry.Name}";
                CreateDirectoryIfNotExists(currentFolder);

                foreach (var directoryEntry in entry.GetDirectories())
                {
                    await ZipInternalAsync($"{currentFolder}\\{directoryEntry.Name}", directoryEntry.Enumerate());
                }

                foreach (var item in entry.GetFiles())
                {
                    byte[] data = await item.GetBytesAsync();
                    await File.WriteAllBytesAsync($"{currentFolder}\\{item.Name}", data);
                }
            }
            else
            {
                byte[] data = await entry.GetBytesAsync();
                await File.WriteAllBytesAsync($"{folder}\\{entry.Name}", data);
            }
        }
    }

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

