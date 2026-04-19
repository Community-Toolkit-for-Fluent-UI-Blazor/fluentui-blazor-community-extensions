namespace FluentUI.Blazor.Community.Components.Charts.Utils;

/// <summary>
/// Represents an internal utility class for measuring the size of text labels in a chart, taking into account font size and rotation angle.
/// </summary>
internal static class LabelMeasurer
{
    /// <summary>
    /// Approximate width of each character in the default font (Segoe UI) at 1em size.
    /// </summary>
    private static readonly Dictionary<char, double> CharWidth = new()
    {
        // Digits
        ['0'] = 0.56,
        ['1'] = 0.32,
        ['2'] = 0.55,
        ['3'] = 0.55,
        ['4'] = 0.56,
        ['5'] = 0.55,
        ['6'] = 0.55,
        ['7'] = 0.50,
        ['8'] = 0.56,
        ['9'] = 0.56,

        // Punctuation
        ['.'] = 0.28,
        [','] = 0.28,
        ['-'] = 0.35,
        ['+'] = 0.35,
        [' '] = 0.30,

        // Lowercase letters
        ['a'] = 0.55,
        ['b'] = 0.56,
        ['c'] = 0.50,
        ['d'] = 0.56,
        ['e'] = 0.55,
        ['f'] = 0.32,
        ['g'] = 0.56,
        ['h'] = 0.56,
        ['i'] = 0.24,
        ['j'] = 0.24,
        ['k'] = 0.50,
        ['l'] = 0.24,
        ['m'] = 0.82,
        ['n'] = 0.56,
        ['o'] = 0.56,
        ['p'] = 0.56,
        ['q'] = 0.56,
        ['r'] = 0.35,
        ['s'] = 0.50,
        ['t'] = 0.32,
        ['u'] = 0.56,
        ['v'] = 0.50,
        ['w'] = 0.74,
        ['x'] = 0.50,
        ['y'] = 0.50,
        ['z'] = 0.50,

        // Uppercase letters
        ['A'] = 0.70,
        ['B'] = 0.65,
        ['C'] = 0.70,
        ['D'] = 0.72,
        ['E'] = 0.62,
        ['F'] = 0.58,
        ['G'] = 0.75,
        ['H'] = 0.72,
        ['I'] = 0.28,
        ['J'] = 0.50,
        ['K'] = 0.65,
        ['L'] = 0.55,
        ['M'] = 0.85,
        ['N'] = 0.72,
        ['O'] = 0.75,
        ['P'] = 0.60,
        ['Q'] = 0.75,
        ['R'] = 0.65,
        ['S'] = 0.60,
        ['T'] = 0.58,
        ['U'] = 0.72,
        ['V'] = 0.65,
        ['W'] = 0.95,
        ['X'] = 0.65,
        ['Y'] = 0.65,
        ['Z'] = 0.60,
    };

    /// <summary>
    /// Represents a cache for storing the measured width and height of labels based on their text and font size, to avoid redundant calculations.
    /// </summary>
    private static readonly Dictionary<(string, double), (double w, double h)> Cache = [];

    private const double Cos45 = 0.70710678;
    private const double Sin45 = 0.70710678;

    /// <summary>
    /// Measure the maximum width/height among all labels, with rotation.
    /// </summary>
    public static (double width, double height) MeasureMax(
        IReadOnlyList<string> labels,
        double fontSize,
        double angle)
    {
        if (labels.Count == 0)
        {
            return (0, 0);
        }

        var maxW = 0.0;
        var maxH = 0.0;

        foreach (var label in labels)
        {
            var (w, h) = MeasureSingle(label, fontSize);

            var (rw, rh) = angle switch
            {
                0 => (w, h),
                -45 => Rotate(w, h, Cos45, Sin45),
                -90 => (h, w),
                _ => (w, h)
            };

            if (rw > maxW)
            {
                maxW = rw;
            }

            if (rh > maxH)
            {
                maxH = rh;
            }
        }

        return (maxW, maxH);
    }

    /// <summary>
    /// Measure a single label (cached).
    /// </summary>
    public static (double w, double h) MeasureSingle(string label, double fontSize)
    {
        var key = (label, fontSize);

        if (Cache.TryGetValue(key, out var size))
        {
            return size;
        }

        var width = 0.0;

        foreach (var c in label)
        {
            if (CharWidth.TryGetValue(c, out var w))
            {
                width += w;
            }
            else
            {
                width += 0.56;
            }
        }

        width *= fontSize;

        var height = fontSize * 1.2;

        size = (width, height);
        Cache[key] = size;

        return size;
    }

    /// <summary>
    /// Calculates rotated dimensions using the provided trigonometric values.
    /// </summary>
    /// <param name="w">The width value to rotate.</param>
    /// <param name="h">The height value to rotate.</param>
    /// <param name="cos">The cosine value of the rotation angle.</param>
    /// <param name="sin">The sine value of the rotation angle.</param>
    /// <returns>A tuple containing the rotated width and height.</returns>
    private static (double width, double height) Rotate(
        double w,
        double h,
        double cos,
        double sin)
    {
        var rw = w * cos + h * sin;
        var rh = w * sin + h * cos;

        return (rw, rh);
    }
}
