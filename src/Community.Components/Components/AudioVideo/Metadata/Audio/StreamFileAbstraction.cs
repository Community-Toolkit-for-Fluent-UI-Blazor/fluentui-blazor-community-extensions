namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an abstraction for file operations using streams, implementing the <see
/// cref="TagLib.File.IFileAbstraction"/> interface.
/// </summary>
/// <param name="name">Name or identifier of the abstraction.</param>
/// <param name="readStream">Represents the readable stream.</param>
/// <param name="writeStream">Represents the writeable stream.</param>
internal sealed class StreamFileAbstraction(string name, Stream readStream, Stream writeStream)
    : TagLib.File.IFileAbstraction
{
    /// <inheritdoc />
    public string Name { get; } = name;

    /// <inheritdoc />
    public Stream ReadStream { get; } = readStream;

    /// <inheritdoc />
    public Stream WriteStream { get; } = writeStream;

    /// <inheritdoc />
    public void CloseStream(Stream stream)
    {
        stream.Close();
    }
}
