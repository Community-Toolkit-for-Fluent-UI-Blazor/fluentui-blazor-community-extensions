namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides utility methods for adding barcode bar patterns to a collection based on a binary string representation.
/// </summary>
/// <remarks>This class is intended for internal use in barcode rendering scenarios where a pattern string defines
/// the sequence of bars and spaces. Each character in the pattern string represents either a bar ('1') or a space
/// ('0'), and the method translates this pattern into a sequence of bar objects with specified dimensions. The class is
/// not intended to be used directly by application code.</remarks>
internal static class GS1Pattern
{
    /// <summary>
    /// Adds a sequence of bars to the specified collection based on a binary pattern, updating the horizontal position
    /// as bars and spaces are processed.
    /// </summary>
    /// <remarks>Each contiguous run of '1's in the pattern results in a single bar being added to the
    /// collection. The method advances the horizontal position for both bars and spaces according to the specified
    /// width. The pattern must not be null and should only contain '1' and '0' characters.</remarks>
    /// <param name="bars">The list to which new Barcode1DBar instances representing bars will be added.</param>
    /// <param name="x">The reference to the current horizontal position. This value is updated to reflect the position after the added
    /// bars and spaces.</param>
    /// <param name="width">The width, in device-independent units, of each bar or space in the pattern. Must be positive.</param>
    /// <param name="height">The height, in device-independent units, to assign to each bar.</param>
    /// <param name="pattern">A string consisting of '1' and '0' characters, where '1' represents a bar and '0' represents a space. The
    /// sequence determines the arrangement of bars and spaces.</param>
    public static void Add(
        List<Barcode1DBar> bars,
        ref double x,
        double width,
        double height,
        string pattern)
    {
        var isBar = true;
        var runWidth = 0.0;

        for (var i = 0; i < pattern.Length; i++)
        {
            var bit = pattern[i];

            if (bit == '1')
            {
                if (!isBar)
                {
                    x += runWidth;
                    runWidth = 0.0;
                    isBar = true;
                }

                runWidth += width;
            }
            else // '0'
            {
                if (isBar)
                {
                    if (runWidth > 0)
                    {
                        bars.Add(new Barcode1DBar
                        {
                            X = x,
                            Width = runWidth,
                            Height = height
                        });

                        x += runWidth;
                        runWidth = 0.0;
                    }

                    isBar = false;
                }

                runWidth += width;
            }
        }

        if (isBar && runWidth > 0)
        {
            bars.Add(new Barcode1DBar
            {
                X = x,
                Width = runWidth,
                Height = height
            });

            x += runWidth;
        }
        else
        {
            x += runWidth;
        }
    }
}
