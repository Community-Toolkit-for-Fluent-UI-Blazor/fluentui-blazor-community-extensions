namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Represents helper for string.
/// </summary>
public static class StringHelper
{
    #region Fields

    /// <summary>
    /// Represents the characters that can be used to encode the object.
    /// </summary>
    private const string EncodedCharTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    /// <summary>
    /// Represents the number of characters available for encoding.
    /// </summary>
    private const int EncodedCharTableCount = 36;

    /// <summary>
    /// Represents the random generator.
    /// </summary>
    private static readonly Random _random = new((int)DateTime.Now.Ticks);

    /// <summary>
    /// Represents the 1st tick from which to start the creation of the object.
    /// </summary>
    private static int FirstTick = _random.Next(16000);

    #endregion Fields

    /// <summary>
    /// Generates an identifier for a component.
    /// </summary>
    /// <param name="length">Length of the identifier.</param>
    /// <returns>Returns the generated identifier.</returns>
    public static string GenerateId(int length = 15)
    {
        var decrement = 5;
        var start = length * decrement;

        var generatedValue = string.Create(length, Interlocked.Increment(ref FirstTick), (c, i) =>
        {
            var tick = FirstTick;
            var count = EncodedCharTableCount - 1;

            var value = EncodedCharTable[tick % count];

            c[length - 1] = value;

            for (var index = 1; index < length - 1; ++index)
            {
                c[index] = EncodedCharTable[(tick >> start) % count];
                start -= decrement;
            }

            value = EncodedCharTable[(tick >> start) % count];

            while (char.IsDigit(value))
            {
                tick++;
                value = EncodedCharTable[(tick >> start) % count];
            }

            c[0] = value;
        });

        return generatedValue;
    }
}
