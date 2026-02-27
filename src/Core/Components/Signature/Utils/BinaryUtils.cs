using System.Text;
using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

internal static class BinaryUtils
{
    /// <summary>
    /// Represents the magic header value used to identify or validate a specific file or data format.
    /// </summary>
    /// <remarks>This constant can be used to check for the presence of a valid header when reading or writing
    /// files that follow the associated format. The value is typically compared against the initial bytes of a file or
    /// stream to ensure compatibility.</remarks>
    private const ulong MagicHeader = 0x4643585355524600;

    /// <summary>
    /// Represents the current version number of the component or feature.
    /// </summary>
    private const int Version = 1;

    /// <summary>
    /// Asynchronously serializes the specified surface payload and export options into a binary format with a CRC32
    /// checksum appended.
    /// </summary>
    /// <remarks>The output format includes a magic header, payload type identifier, version, selected data
    /// sections, and a CRC32 checksum for integrity verification. The method uses UTF-8 encoding for binary
    /// writing.</remarks>
    /// <typeparam name="TPayload">The type of the payload data to serialize.</typeparam>
    /// <param name="payload">The surface payload containing the data sections to be exported.</param>
    /// <param name="options">The export options that determine which sections of the payload are included in the output.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a byte array with the serialized
    /// binary data and appended CRC32 checksum.</returns>
    public static async ValueTask<byte[]> WriteAsync<TPayload>(
        SurfacePayload<TPayload> payload,
        ExportOptions options)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);

        // 1. Magic header
        writer.Write(MagicHeader);

        // 2. Payload type ID
        writer.Write(PayloadTypeId.FromType<TPayload>());

        // 3. Version
        writer.Write(Version);

        // 4. Sections
        WriteSection(writer, payload.View, options.IncludeView);
        WriteSection(writer, payload.Background, options.IncludeBackground);
        WriteSection(writer, payload.Grid, options.IncludeGrid);
        WriteSection(writer, payload.Axes, options.IncludeAxes);
        WriteSection(writer, payload.Content);
        WriteSection(writer, payload.Watermark, options.IncludeWatermark);
        writer.Flush();

        var data = ms.ToArray();
        var crc = Crc32Utils.Compute(data);

        using var final = new MemoryStream(data.Length + sizeof(uint));
        final.Write(data, 0, data.Length);
        using var crcWriter = new BinaryWriter(final, Encoding.UTF8, leaveOpen: true);
        crcWriter.Write(crc);

        return final.ToArray();
    }

    /// <summary>
    /// Asynchronously reads a surface payload of the specified type from the provided stream, validating the format and
    /// integrity of the data.
    /// </summary>
    /// <remarks>The method expects the stream to contain a valid surface payload in the expected binary
    /// format, including a magic header, type identifier, version, and section data. The stream is not closed by this
    /// method.</remarks>
    /// <typeparam name="TPayload">The type of the payload to deserialize from the stream.</typeparam>
    /// <param name="stream">The stream from which to read the surface payload. The stream must be readable and positioned at the start of
    /// the payload data.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the deserialized surface
    /// payload of the specified type.</returns>
    /// <exception cref="InvalidDataException">Thrown if the stream does not contain a valid surface payload, if the header, type, or version is invalid, or if
    /// the data integrity check fails.</exception>
    public static async ValueTask<SurfacePayload<TPayload>> ReadAsync<TPayload>(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

        // 1. Magic header
        var magic = reader.ReadUInt64();

        if (magic != MagicHeader)
        {
            throw new InvalidDataException("Invalid FCXSURF header.");
        }

        // 2. Payload type ID
        var typeId = reader.ReadUInt32();
        var expectedTypeId = PayloadTypeId.FromType<TPayload>();

        if (typeId != expectedTypeId)
        {
            throw new InvalidDataException($"Payload type mismatch. Expected {expectedTypeId}, got {typeId}.");
        }

        // 3. Version
        var version = reader.ReadInt32();

        if (version != Version)
        {
            throw new InvalidDataException($"Unsupported FCXSURF version: {version}");
        }

        // 4. Sections
        var view = ReadSection<ViewPayload>(reader);
        var background = ReadSection<BackgroundPayload>(reader);
        var grid = ReadSection<GridPayload>(reader);
        var axes = ReadSection<AxesPayload>(reader);
        var content = ReadSection<TPayload>(reader);
        var watermark = ReadSection<WatermarkPayload>(reader);

        // 5. CRC32 validation
        Crc32Utils.Validate(stream);

        return new SurfacePayload<TPayload>
        {
            View = view,
            Background = background,
            Grid = grid,
            Axes = axes,
            Content = content,
            Watermark = watermark
        };
    }

    /// <summary>
    /// Writes a serialized JSON representation of the specified section object to the binary writer, prefixed by its
    /// byte length. Writes a zero if the section is null or not included.
    /// </summary>
    /// <remarks>The section object is serialized to JSON using UTF-8 encoding. If <paramref name="section"/>
    /// is null or <paramref name="include"/> is <see langword="false"/>, only a zero is written to indicate the absence
    /// of section data.</remarks>
    /// <param name="writer">The binary writer to which the section data will be written.</param>
    /// <param name="section">The object representing the section to serialize and write. If null, a zero is written instead.</param>
    /// <param name="include">A value indicating whether to include the section in the output. If set to <see langword="false"/>, the section
    /// is not written.</param>
    private static void WriteSection(
        BinaryWriter writer,
        object? section,
        bool include = true)
    {
        if (!include || section is null)
        {
            writer.Write(0);
            return;
        }

        var json = JsonSerializer.Serialize(section);
        var bytes = Encoding.UTF8.GetBytes(json);

        writer.Write(bytes.Length);
        writer.Write(bytes);
    }

    /// <summary>
    /// Deserializes a section of binary data from the specified reader into an object of type T using UTF-8 encoded
    /// JSON.
    /// </summary>
    /// <remarks>The method expects the section to be prefixed with a 4-byte integer indicating the length of
    /// the section in bytes. If the length is zero, the method returns the default value for type T. The section is
    /// assumed to contain UTF-8 encoded JSON data.</remarks>
    /// <typeparam name="T">The type of object to deserialize from the binary section.</typeparam>
    /// <param name="reader">The binary reader from which to read the section data. The reader must be positioned at the start of the
    /// section.</param>
    /// <returns>An instance of type T deserialized from the section, or the default value of T if the section length is zero.</returns>
    private static T? ReadSection<T>(BinaryReader reader)
    {
        var length = reader.ReadInt32();

        if (length == 0)
        {
            return default;
        }

        var bytes = reader.ReadBytes(length);
        var json = Encoding.UTF8.GetString(bytes);

        return JsonSerializer.Deserialize<T>(json);
    }
}

