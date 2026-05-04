namespace FluentUI.Blazor.Community.Components.Helpers;

/// <summary>
/// Represents a dynamic list of bits that supports adding individual bits or sequences of bits derived from integer
/// values.
/// </summary>
/// <remarks>BitList provides methods to append single bits or multiple bits extracted from an integer, and allows
/// conversion of the list to an array of Boolean values. This class is intended for internal use where efficient
/// bit-level storage and manipulation are required.</remarks>
internal sealed class BitList
{
    /// <summary>
    /// Represents the internal storage of bits as a list of Boolean values.
    /// </summary>
    private readonly List<bool> _bits = [];

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public int Length => _bits.Count;

    /// <summary>
    /// Adds a Boolean value to the end of the bit collection.
    /// </summary>
    /// <param name="bit">The Boolean value to add to the collection. Use <see langword="true"/> to represent a set bit; otherwise, <see
    /// langword="false"/>.</param>
    public void Add(bool bit) => _bits.Add(bit);

    /// <summary>
    /// Appends the specified number of least significant bits from the given integer value to the current bit
    /// collection.
    /// </summary>
    /// <remarks>Bits are added in order from the most significant to the least significant within the
    /// specified bit count. For example, if bitCount is 3 and value is 5 (binary 101), the bits 1, 0, 1 are appended in
    /// that order.</remarks>
    /// <param name="value">The integer value whose bits are to be added.</param>
    /// <param name="bitCount">The number of least significant bits from the value to append. Must be between 1 and 32.</param>
    public void Add(int value, int bitCount)
    {
        for (var i = bitCount - 1; i >= 0; i--)
        {
            _bits.Add(((value >> i) & 1) == 1);
        }
    }

    /// <summary>
    /// Returns an array containing the values of all bits in the collection.
    /// </summary>
    /// <returns>A Boolean array representing the state of each bit in the collection. Each element is <see langword="true"/> if
    /// the corresponding bit is set; otherwise, <see langword="false"/>.</returns>
    public bool[] ToArray() => [.. _bits];
}

