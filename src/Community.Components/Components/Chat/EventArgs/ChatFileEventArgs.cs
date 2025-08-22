using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an event args for a chat file.
/// </summary>
public record ChatFileEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileEventArgs"/> class with the specified id, name, content type, and data function.
    /// </summary>
    /// <param name="id">Identifier of the file.</param>
    /// <param name="name">Name of the file.</param>
    /// <param name="contentType">Content type of the file.</param>
    /// <param name="data">Data of the file as an asynchronous task.</param>
    /// <param name="isRecordedAudio">Value indicating if the chat file is a recorded audio file.</param>
    public ChatFileEventArgs(long id, string name, string contentType, Func<Task<byte[]>> data, bool isRecordedAudio = false)
    {
        Id = $"f{id}";
        Name = name;
        ContentType = contentType;
        DataFunc = data;
        IsRecordedAudio = isRecordedAudio;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileEventArgs"/> class with the specified name, content type, and data.
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="contentType">Content type of the file.</param>
    /// <param name="data">Data of the file.</param>
    /// <param name="isRecordedAudio">Value indicating if the chat file is a recorded audio file.</param>
    public ChatFileEventArgs(string name, string contentType, byte[] data, bool isRecordedAudio = false)
    {
        Id = Identifier.NewId();
        Name = name;
        ContentType = contentType;
        Data = data;
        IsRecordedAudio = isRecordedAudio;
    }

    /// <summary>
    /// Gets or sets the identifier of the file.
    /// </summary>
    public string? Id { get; }

    /// <summary>
    /// Gets the name of the file.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the content type of the file.
    /// </summary>
    public string ContentType { get;  }

    /// <summary>
    /// Gets the function that retrieves the data of the file asynchronously.
    /// </summary>
    public Func<Task<byte[]>>? DataFunc { get; }

    /// <summary>
    /// Gets the data of the file.
    /// </summary>
    public byte[] Data { get; } = [];

    /// <summary>
    /// Gets or sets if the file is a recorded audio.
    /// </summary>
    public bool IsRecordedAudio { get; }
}
