using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Utils to compute CRC32 checksums, used to validate the integrity of the binary format of the surface payload.
/// </summary>
internal class Crc32Utils
{
    /// <summary>
    /// Contains the precomputed lookup table used for efficient calculations.
    /// </summary>
    /// <remarks>This table is typically used in algorithms that require fast access to constant values, such
    /// as CRC or hashing computations. The table is generated once and reused to improve performance.</remarks>
    private static readonly uint[] Table = GenerateTable();

    /// <summary>
    /// Generates a lookup table used for efficient CRC-32 checksum calculations.
    /// </summary>
    /// <remarks>The returned table is typically used to accelerate CRC-32 computations by avoiding repeated
    /// polynomial division at runtime. Each entry corresponds to a possible byte value and its associated CRC-32
    /// remainder.</remarks>
    /// <returns>An array of 256 unsigned integers representing the precomputed CRC-32 table.</returns>
    private static uint[] GenerateTable()
    {
        var table = new uint[256];
        const uint poly = 0xEDB88320;

        for (uint i = 0; i < table.Length; i++)
        {
            var crc = i;

            for (var j = 0; j < 8; j++)
            {
                var bit = (crc & 1) == 1;
                crc >>= 1;

                if (bit)
                {
                    crc ^= poly;
                }
            }

            table[i] = crc;
        }

        return table;
    }

    /// <summary>
    /// Calculates the CRC-32 checksum for the specified sequence of bytes.
    /// </summary>
    /// <remarks>The method processes the input data using the standard CRC-32 algorithm. The result can be
    /// used to verify data integrity or detect accidental changes to raw data.</remarks>
    /// <param name="data">The input data over which to compute the CRC-32 checksum.</param>
    /// <returns>The computed CRC-32 checksum as an unsigned 32-bit integer.</returns>
    public static uint Compute(ReadOnlySpan<byte> data)
    {
        var crc = 0xFFFFFFFF;

        foreach (var b in data)
        {
            var index = (byte)(crc ^ b);
            crc = (crc >> 1) ^ Table[index];
        }

        return ~crc;
    }

    /// <summary>
    /// Validates the CRC32 checksum at the end of the specified stream to ensure data integrity.
    /// </summary>
    /// <remarks>The method reads the last four bytes of the stream as the stored CRC32 checksum and computes
    /// the CRC32 of the preceding data. If the values do not match, an exception is thrown. The stream's position is
    /// modified during validation.</remarks>
    /// <param name="stream">The stream containing the data and its CRC32 checksum. The stream must be readable and seekable, with the CRC32
    /// value stored as the last four bytes.</param>
    /// <exception cref="InvalidDataException">Thrown if the stream is too small to contain a CRC32 value or if the computed CRC32 does not match the stored
    /// value, indicating possible data corruption.</exception>
    public static void Validate(Stream stream)
    {
        var totalLength = stream.Length;

        if (totalLength < sizeof(uint))
        {
            throw new InvalidDataException("File too small to contain CRC.");
        }

        var crcPosition = totalLength - sizeof(uint);

        stream.Position = crcPosition;
        using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        var storedCrc = reader.ReadUInt32();

        stream.Position = 0;
        var data = new byte[crcPosition];
        stream.ReadExactly(data.AsSpan());

        var computedCrc = Compute(data);

        if (computedCrc != storedCrc)
        {
            throw new InvalidDataException("CRC32 mismatch. File corrupted.");
        }
    }
}
