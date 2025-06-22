namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the shape of the image.
/// </summary>
public enum ImageShape
{
    /// <summary>
    /// The image is square.
    /// </summary>
    Square,

    /// <summary>
    /// The image is round square.
    /// </summary>
    RoundSquare,

    /// <summary>
    /// The image is circle.
    /// </summary>
    Circle
}

/// <summary>
/// Extensions for the <see cref="ImageShape"/> enum.
/// </summary>
internal static class ImageShapeExtensions
{
    /// <summary>
    /// Converts the <see cref="ImageShape"/> to a CSS class.
    /// </summary>
    /// <param name="shape">The image shape.</param>
    /// <returns>The CSS class representing the image shape.</returns>
    internal static string ToBorderRadius(this ImageShape shape)
    {
        return shape switch
        {
            ImageShape.Square => "0px",
            ImageShape.RoundSquare => "8px",
            ImageShape.Circle => "100000px",
            _ => throw new ArgumentOutOfRangeException(nameof(shape), shape, null)
        };
    }
}
