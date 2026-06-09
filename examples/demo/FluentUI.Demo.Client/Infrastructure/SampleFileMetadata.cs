namespace FluentUI.Demo.Client.Infrastructure;

public class SampleFileMetadata
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Name { get; set; }

    public string? ParentId { get; set; }

    public bool IsDirectory { get; set; }

    public long Size { get; set; }

    public DateTimeOffset LastModified { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;

    public string DataFile { get; set; } = string.Empty;

    public bool IsRenameAllowed { get; set; } = true;
    public bool IsDeleteAllowed { get; set; } = true;
    public bool IsDownloadAllowed { get; set; } = true;
    public bool IsMoveAllowed { get; set; } = true;
}
