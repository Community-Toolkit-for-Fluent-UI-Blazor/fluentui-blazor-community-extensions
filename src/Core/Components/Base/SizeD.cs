namespace FluentUI.Blazor.Community.Components.Base;

/// <summary>
/// Represents a structure that defines the size of an object using double-precision floating-point values for width and height.
/// </summary>
public readonly struct SizeD
{
    private static readonly SizeD _empty = new(0, 0);

    /// <summary>
    /// Gets an empty size with zero width and height.
    /// </summary>
    public static SizeD Empty => _empty;

    /// <summary>
    /// Gets a value indicating whether the size is empty.
    /// </summary>
    public readonly bool IsEmpty
    {
        get
        {
            if (Width == 0)
            {
                return Height == 0;
            }

            return false;
        }
    }

    /// <summary>
    /// Gets the width of the size.
    /// </summary>
    public double Width { get; init; }

    /// <summary>
    /// Gets the height of the size.
    /// </summary>
    public double Height { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SizeD"/> struct with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the size.</param>
    /// <param name="height">The height of the size.</param>
    public SizeD(double width, double height)
    {
        Width = width;
        Height = height;
    }
}
