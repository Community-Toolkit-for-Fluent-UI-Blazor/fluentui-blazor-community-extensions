namespace FluentUI.Blazor.Community.Components.ColorSpace.Cie;

/// <summary>
/// Represents the standard illuminants defined by the CIE (International Commission on Illumination).
/// </summary>
public static class CieIlluminants
{
    /// <summary>
    /// Provides a lookup table that maps each illuminant name to its corresponding CIE illuminant data, including
    /// chromaticity coordinates and color temperature.
    /// </summary>
    /// <remarks>This table contains standard illuminant definitions commonly used in color science. The data
    /// can be used to retrieve chromaticity and temperature information for a given illuminant by its name.</remarks>
    private static readonly Dictionary<IlluminantName, CieIlluminant> s_table =
        new(EqualityComparer<IlluminantName>.Default)
        {
            [IlluminantName.A] = new(0.44757, 0.40745, 0.45117, 0.40594, 2856),
            [IlluminantName.B] = new(0.34842, 0.35161, 0.34980, 0.35270, 4874),
            [IlluminantName.C] = new(0.31006, 0.31616, 0.31039, 0.31905, 6774),
            [IlluminantName.D50] = new(0.34567, 0.35850, 0.34773, 0.35952, 5003),
            [IlluminantName.D55] = new(0.33242, 0.34743, 0.33411, 0.34877, 5503),
            [IlluminantName.D65] = new(0.31271, 0.32902, 0.31382, 0.33100, 6504),
            [IlluminantName.D75] = new(0.29902, 0.31485, 0.29968, 0.31740, 7504),
            [IlluminantName.E] = new(1.0 / 3, 1.0 / 3, 1.0 / 3, 1.0 / 3, 5454),
            [IlluminantName.F1] = new(0.31310, 0.33727, 0.31811, 0.33559, 6430),
            [IlluminantName.F2] = new(0.37208, 0.37529, 0.37925, 0.36733, 4230),
            [IlluminantName.F3] = new(0.40910, 0.39430, 0.41761, 0.38324, 3450),
            [IlluminantName.F4] = new(0.44018, 0.40329, 0.44920, 0.39074, 2940),
            [IlluminantName.F5] = new(0.31379, 0.34531, 0.31975, 0.34246, 6350),
            [IlluminantName.F6] = new(0.37790, 0.38835, 0.38660, 0.37847, 4150),
            [IlluminantName.F7] = new(0.31292, 0.32933, 0.31569, 0.32960, 6500),
            [IlluminantName.F8] = new(0.34588, 0.35875, 0.34902, 0.35939, 5000),
            [IlluminantName.F9] = new(0.37417, 0.37281, 0.37829, 0.37045, 4150),
            [IlluminantName.F10] = new(0.34609, 0.35986, 0.35090, 0.35444, 5000),
            [IlluminantName.F11] = new(0.38052, 0.37713, 0.38541, 0.37123, 4000),
            [IlluminantName.F12] = new(0.43695, 0.40441, 0.44256, 0.39717, 3000)
        };

    /// <summary>
    /// Gets the CIE illuminant corresponding to the specified name.
    /// </summary>
    /// <param name="name">The name of the illuminant.</param>
    /// <returns>Returns the CIE illuminant corresponding to the specified name.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified name does not correspond to a valid illuminant.</exception>
    public static CieIlluminant Get(IlluminantName name)
    {
        if (!s_table.TryGetValue(name, out var illuminant))
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }

        return illuminant;
    }
}
