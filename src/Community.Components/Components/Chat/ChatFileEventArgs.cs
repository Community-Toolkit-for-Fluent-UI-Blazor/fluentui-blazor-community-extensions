namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an event args for a chat file.
/// </summary>
/// <param name="Id">Id of the file.</param>
/// <param name="Name">Name of the file.</param>
/// <param name="ContentType">Content type of the file.</param>
/// <param name="Data">Data of the file.</param>
public record ChatFileEventArgs
{
    public ChatFileEventArgs(long id, string name, string contentType, Func<Task<byte[]>> data)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        DataFunc = data;
    }

   public ChatFileEventArgs(string name, string contentType, byte[] data)
   {
      Name = name;
      ContentType = contentType;
      Data = data;
    }

    public long? Id { get; set; }

    public string Name { get; init; }

    public string ContentType { get; init; }

    public Func<Task<byte[]>>? DataFunc { get; init; }

    public byte[] Data { get; init; } = [];
}
