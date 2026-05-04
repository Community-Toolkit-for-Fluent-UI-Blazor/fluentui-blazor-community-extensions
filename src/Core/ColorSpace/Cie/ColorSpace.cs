using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.ColorSpace.Vision;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Cie;

/// <summary>
/// Represents a color space defined by an ICC profile.
/// </summary>
public sealed class ColorSpace
{
    /// <summary>
    /// Represents the transformation matrix used to convert RGB color values to the CIE XYZ color space.
    /// </summary>
    /// <remarks>This matrix defines the linear transformation applied when converting colors from the RGB
    /// color space to the XYZ color space, which is commonly used in color science for device-independent color
    /// representation.</remarks>
    private readonly Matrix3x3 rgbToXyz;

    /// <summary>
    /// Represents the transformation matrix used to convert color values from the CIE XYZ color space to the RGB color
    /// space.
    /// </summary>
    /// <remarks>This matrix defines how XYZ color coordinates are mapped to the RGB color model. The specific
    /// values depend on the RGB color space and white point in use. Modifying or using this matrix directly requires
    /// understanding of color science and matrix operations.</remarks>
    private readonly Matrix3x3 xyzToRgb;

    /// <summary>
    /// Initializes a new instance of the ColorSpace class with the specified ICC profile and chromatic adaptation method.
    /// </summary>
    /// <param name="profile">The ICC profile to associate with the color space.</param>
    /// <param name="adaptation">The chromatic adaptation method to use for color conversions.</param>
    private ColorSpace(IccProfile profile, ChromaticAdaptationMethod adaptation)
    {
        Profile = profile;
        Adaptation = adaptation;

        WorkingSpace = RgbWorkingSpace.Create(profile, adaptation);

        rgbToXyz = WorkingSpace.RgbToXyzMatrix;
        xyzToRgb = WorkingSpace.XyzToRgbMatrix;
    }

    /// <summary>
    /// Gets the ICC profile associated with the current instance.
    /// </summary>
    public IccProfile Profile { get; }

    /// <summary>
    /// Gets the RGB working space used for color conversions.
    /// </summary>
    public RgbWorkingSpace WorkingSpace { get; }

    /// <summary>
    /// Gets the chromatic adaptation method used for color conversions.
    /// </summary>
    public ChromaticAdaptationMethod Adaptation { get; }

    /// <summary>
    /// Creates a new instance of the ColorSpace class using the specified ICC profile and chromatic adaptation method.
    /// </summary>
    /// <param name="name">The ICC profile name that defines the color space to be created.</param>
    /// <param name="adaptation">The chromatic adaptation method to use when converting between color spaces. Defaults to Bradford if not
    /// specified.</param>
    /// <returns>A ColorSpace instance configured with the specified ICC profile and chromatic adaptation method.</returns>
    public static ColorSpace Create(
        IccProfileName name,
        ChromaticAdaptationMethod adaptation = ChromaticAdaptationMethod.Bradford)
    {
        return new(IccProfiles.Get(name), adaptation);
    }

    /// <summary>
    /// Converts an sRGB color to its equivalent CIE XYZ color representation.
    /// </summary>
    /// <remarks>The conversion uses the current color profile's gamma correction. This method is useful for
    /// color space transformations and colorimetric calculations.</remarks>
    /// <param name="color">The sRGB color to convert. The color must be in the sRGB color space with 8-bit channel values.</param>
    /// <returns>A <see cref="Xyz"/> structure representing the color in the CIE XYZ color space.</returns>
    public Xyz ToXyz(Srgb8 color) => ColorSpaceConverters.ToXyz(color, rgbToXyz, Profile.Gamma);

    /// <summary>
    /// Converts a color value from the CIE XYZ color space to the sRGB color space with 8-bit channel precision.
    /// </summary>
    /// <remarks>The conversion applies the current color profile's gamma correction. Use this method to
    /// obtain a display-ready sRGB color from a device-independent XYZ value.</remarks>
    /// <param name="xyz">The color value in the CIE XYZ color space to convert.</param>
    /// <returns>An Srgb8 structure representing the equivalent color in the sRGB color space with 8 bits per channel.</returns>
    public Srgb8 ToSrgb8(Xyz xyz) => ColorSpaceConverters.ToSrgb8(xyz, xyzToRgb, Profile.Gamma);

    /// <summary>
    /// Applies how a specified color appears to individuals with a given type of color vision deficiency.
    /// </summary>
    /// <remarks>Use this method to preview color accessibility for users with different types of color vision
    /// deficiencies. This can help ensure that color choices in user interfaces or visualizations remain
    /// distinguishable for all users.</remarks>
    /// <param name="color">The original color to be transformed, represented in the sRGB color space.</param>
    /// <param name="type">The type of color vision deficiency to simulate. Determines how the input color will be altered.</param>
    /// <returns>A new Srgb8 color representing how the input color would appear to someone with the specified color vision
    /// deficiency.</returns>
    public Srgb8 SimulateVision(Srgb8 color, ColorVisionType type) => ColorVisionTransform.Apply(color, type, WorkingSpace);
}

