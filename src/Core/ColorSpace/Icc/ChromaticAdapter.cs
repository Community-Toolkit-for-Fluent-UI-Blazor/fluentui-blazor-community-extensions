using FluentUI.Blazor.Community.Components.ColorSpace.Cie;
using FluentUI.Blazor.Community.Components.ColorSpace.Converters;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using FluentUI.Blazor.Community.Components.Maths;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Icc;

/// <summary>
/// Represents a utility class for performing chromatic adaptation between different white points using specified methods.
/// </summary>
internal static class ChromaticAdapter
{
    internal static Xyz Adapt(
        double m1,
        double m2,
        double m3,
        Xyz whitePointSource,
        Xyz whitePointDestination,
        ChromaticAdaptationMethod adaptation)
    {
        var matrixA = Matrix3x3.Identity;
        var matrixB = Matrix3x3.Identity;
        var color = ColorSpaceConverters.ToXyz(new Xyy(m1, m2, m3));

        var whitePointSourceMatrix = new Matrix3x3(whitePointSource.X, whitePointSource.Y, whitePointSource.Z);
        var whitePointDestinationMatrix = new Matrix3x3(whitePointDestination.X, whitePointDestination.Y, whitePointDestination.Z);

        switch (adaptation)
        {
            case ChromaticAdaptationMethod.VonKries:
                {
                    matrixA = new Matrix3x3(0.400240, -0.226300, 0, 0.707600, 1.165320, 0, -0.080810, 0.045700, 0.918220);
                    matrixB = new Matrix3x3(1.859936, 0.361191, 0, -1.129382, 0.638812, 0, 0.219897, -0.000006, 1.089064);
                }

                break;

            case ChromaticAdaptationMethod.Bradford:
                {
                    matrixA = new Matrix3x3(0.895100, 0.26640000, -0.16139900, -0.75019900, 1.71350, 0.0367000, 0.03889900, -0.0685000, 1.02960000);
                    matrixB = new Matrix3x3(0.986993, -0.14705399, 0.15996299, 0.43230499, 0.51836, 0.0492912, -0.00852866, 0.0400428, 0.96848699);
                }

                break;
        }

        var crd = Matrix3x3.Multiply(matrixA, whitePointDestinationMatrix);
        var crs = Matrix3x3.Multiply(matrixB, whitePointSourceMatrix);
        var scale = new Matrix3x3(m11: crd.M11 / crs.M11, m22: crd.M12 / crs.M12, m33: crd.M13 / crs.M13);
        var final = Matrix3x3.Multiply(matrixB, Matrix3x3.Multiply(scale, Matrix3x3.Multiply(matrixA, new Matrix3x3(color.X, color.Y, color.Z))));

        return new Xyz(final.M11, final.M12, final.M13);
    }
}
