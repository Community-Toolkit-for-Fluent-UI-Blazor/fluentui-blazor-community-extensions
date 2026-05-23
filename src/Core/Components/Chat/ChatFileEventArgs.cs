using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat;

/// <summary>
/// Represents an event args for a chat file.
/// </summary>
public record ChatFileEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileEventArgs"/> class with the specified name, content type, and data.
    /// </summary>
    /// <param name="id">Identifier of the file.</param>
    /// <param name="name">Name of the file.</param>
    /// <param name="contentType">Content type of the file.</param>
    /// <param name="data">Data of the file.</param>
    /// <param name="isRecordedAudio">Value indicating if the chat file is a recorded audio file.</param>
    public ChatFileEventArgs(string id, string name, string contentType, byte[] data, bool isRecordedAudio = false)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        Data = data;
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
        : this (Identifier.NewId(), name, contentType, data, isRecordedAudio)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileEventArgs"/> class with the specified name, content type, and data.
    /// </summary>
    /// <param name="id">Identifier of the file.</param>
    /// <param name="name">Name of the file.</param>
    /// <param name="contentType">Content type of the file.</param>
    /// <param name="dataFunc">Function that retrieves the data of the file asynchronously.</param>
    /// <param name="isRecordedAudio">Value indicating if the chat file is a recorded audio file.</param>
    public ChatFileEventArgs(string id, string name, string contentType, Func<Task<byte[]>> dataFunc, bool isRecordedAudio = false)
    {
        Id = id;
        Name = name;
        ContentType = contentType;
        DataFunc = dataFunc;
        IsRecordedAudio = isRecordedAudio;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatFileEventArgs"/> class with the specified name, content type, and data.
    /// </summary>
    /// <param name="name">Name of the file.</param>
    /// <param name="contentType">Content type of the file.</param>
    /// <param name="dataFunc">Function that retrieves the data of the file asynchronously.</param>
    /// <param name="isRecordedAudio">Value indicating if the chat file is a recorded audio file.</param>
    public ChatFileEventArgs(string name, string contentType, Func<Task<byte[]>> dataFunc, bool isRecordedAudio = false)
        : this(Identifier.NewId(), name, contentType, dataFunc, isRecordedAudio)
    { }

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
    public string ContentType { get; }

    /// <summary>
    /// Gets the function that retrieves the data of the file asynchronously.
    /// </summary>
    private Func<Task<byte[]>>? DataFunc { get; }

    /// <summary>
    /// Gets the data of the file.
    /// </summary>
    private byte[] Data { get; } = [];

    /// <summary>
    /// Gets or sets if the file is a recorded audio.
    /// </summary>
    public bool IsRecordedAudio { get; }

    /// <summary>
    /// Gets the data of the file asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the data of the file.</returns>
    public Task<byte[]> GetDataAsync() => DataFunc != null ? DataFunc() : Task.FromResult(Data ?? []);

    /// <summary>
    /// Retrieves the length of the file data.
    /// </summary>
    /// <returns>The length of the file data.</returns>
    internal int GetLength() => Data?.Length ?? 0;
}
