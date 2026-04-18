using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Icc;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents the RGB working space derived from an ICC profile, including the transformation matrices for converting
/// between RGB and XYZ color spaces.
/// </summary>
public sealed class RgbWorkingSpace
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RgbWorkingSpace"/> class using the specified ICC profile and chromatic
    /// adaptation method.
    /// </summary>
    /// <param name="profile">The ICC profile used to define the RGB working space.</param>
    /// <param name="adaptation">The chromatic adaptation method to use for color conversions.</param>
    private RgbWorkingSpace(
        IccProfile profile,
        ChromaticAdaptationMethod adaptation)
    {
        Profile = profile;
        Adaptation = adaptation;
        WhitePointSource = CieIlluminants.Get(profile.IlluminantName).ToXyz(IlluminantPointOfView.TwoDegrees);
        WhitePointDestination = CieIlluminants.Get(IlluminantName.D65).ToXyz(IlluminantPointOfView.TwoDegrees);

        var matrix = profile.Matrix;
        var red = ChromaticAdapter.Adapt(matrix.M11, matrix.M12, matrix.M13, WhitePointSource, WhitePointDestination, adaptation);
        var green = ChromaticAdapter.Adapt(matrix.M21, matrix.M22, matrix.M23, WhitePointSource, WhitePointDestination, adaptation);
        var blue = ChromaticAdapter.Adapt(matrix.M31, matrix.M32, matrix.M33, WhitePointSource, WhitePointDestination, adaptation);

        RgbToXyzMatrix = new Matrix3x3(red.X, red.Y, red.Z, green.X, green.Y, green.Z, blue.X, blue.Y, blue.Z);
        XyzToRgbMatrix = Matrix3x3.Inverse(RgbToXyzMatrix);
    }

    /// <summary>
    /// Gets the ICC profile associated with the current object.
    /// </summary>
    /// <remarks>The ICC profile provides color management information that can be used to interpret or
    /// convert color data accurately. Use this property to access the color profile details for further processing or
    /// analysis.</remarks>
    public IccProfile Profile { get; }

    /// <summary>
    /// Gets the chromatic adaptation method used for color conversions.
    /// </summary>
    public ChromaticAdaptationMethod Adaptation { get; }

    /// <summary>
    /// Gets the matrix used to convert RGB color values to the CIE XYZ color space.
    /// </summary>
    /// <remarks>This matrix defines the linear transformation from the RGB color space to the CIE XYZ color
    /// space, which is commonly used in color science for device-independent color representation. The specific values
    /// of the matrix depend on the RGB color space in use (such as sRGB or Adobe RGB).</remarks>
    public Matrix3x3 RgbToXyzMatrix { get; }

    /// <summary>
    /// Gets the matrix used to convert color values from the CIE XYZ color space to the RGB color space.
    /// </summary>
    /// <remarks>The returned matrix defines the linear transformation applied when converting XYZ color
    /// coordinates to their corresponding RGB representation. The specific values depend on the RGB color space and
    /// white point in use.</remarks>
    public Matrix3x3 XyzToRgbMatrix { get; }

    /// <summary>
    /// Gets the reference white point used for color calculations in the CIE XYZ color space.
    /// </summary>
    /// <remarks>The white point defines the color that is considered 'white' for the purpose of color
    /// conversions and comparisons. Different white points may be used depending on the application or standard, such
    /// as D65 or D50.</remarks>
    public Xyz WhitePointSource { get; }

    /// <summary>
    /// Gets the destination white point used for color conversion operations.
    /// </summary>
    public Xyz WhitePointDestination { get; }

    /// <summary>
    /// Creates a new instance of the RgbWorkingSpace class using the specified ICC profile and chromatic adaptation
    /// method.
    /// </summary>
    /// <param name="profile">The ICC profile that defines the RGB color space to use. Cannot be null.</param>
    /// <param name="adaptation">The chromatic adaptation method to apply when converting between color spaces. Defaults to Bradford if not
    /// specified.</param>
    /// <returns>A new RgbWorkingSpace instance configured with the specified profile and adaptation method.</returns>
    public static RgbWorkingSpace Create(
        IccProfileName profile,
        ChromaticAdaptationMethod adaptation = ChromaticAdaptationMethod.Bradford)
    {
        var iccProfile = IccProfiles.Get(profile);

        return new(iccProfile, adaptation);
    }

    /// <summary>
    /// Creates a new instance of the RgbWorkingSpace class using the specified ICC profile and chromatic adaptation
    /// method.
    /// </summary>
    /// <param name="profile">The ICC profile that defines the RGB color space to use. Cannot be null.</param>
    /// <param name="adaptation">The chromatic adaptation method to apply when converting between color spaces. Defaults to Bradford if not
    /// specified.</param>
    /// <returns>A new RgbWorkingSpace instance configured with the specified profile and adaptation method.</returns>
    public static RgbWorkingSpace Create(
        IccProfile profile,
        ChromaticAdaptationMethod adaptation = ChromaticAdaptationMethod.Bradford)
    {
        return new(profile, adaptation);
    }
}

