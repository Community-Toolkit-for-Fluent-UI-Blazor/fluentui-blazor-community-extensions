using FluentUI.Blazor.Community.Components.ColorSpace.Cie;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Icc;

/// <summary>
/// Represents the factory for creating instances of <see cref="IccProfile"/>.
/// </summary>
public static class IccProfiles
{
    /// <summary>
    /// Represents the dictionary of ICC profiles.
    /// </summary>
    private static readonly Dictionary<IccProfileName, IccProfile> s_profiles = new(EqualityComparer<IccProfileName>.Default)
    {
        [IccProfileName.Adobe] = new IccProfile(
            IccProfileName.Adobe,
            2.2,
            IlluminantName.D65,
            0.64,
            0.33,
            0.297361,
            0.21,
            0.71,
            0.627355,
            0.15,
            0.06,
            0.075285),

        [IccProfileName.AppleRgb] = new IccProfile(
            IccProfileName.AppleRgb,
            1.8,
            IlluminantName.D65,
            0.625,
            0.34,
            0.244634,
            0.28,
            0.595,
            0.672034,
            0.155,
            0.07,
            0.083332),

        [IccProfileName.BestRgb] = new IccProfile(
            IccProfileName.BestRgb,
            2.2,
            IlluminantName.D50,
            0.7347,
            0.2653,
            0.228457,
            0.215,
            0.775,
            0.737352,
            0.13,
            0.035,
            0.034191),

        [IccProfileName.BetaRgb] = new IccProfile(
            IccProfileName.BetaRgb,
            2.2,
            IlluminantName.D50,
            0.6888,
            0.3112,
            0.303273,
            0.1986,
            0.7551,
            0.663786,
            0.1265,
            0.0352,
            0.032941),

        [IccProfileName.BruceRgb] = new IccProfile(
            IccProfileName.BruceRgb,
            2.2,
            IlluminantName.D65,
            0.64,
            0.33,
            0.240995,
            0.28,
            0.65,
            0.683554,
            0.15,
            0.06,
            0.075452),

        [IccProfileName.CieRgb] = new IccProfile(
            IccProfileName.CieRgb,
            2.2,
            IlluminantName.E,
            0.735,
            0.265,
            0.176204,
            0.274,
            0.717,
            0.812985,
            0.167,
            0.009,
            0.010811),

        [IccProfileName.ColorMatch] = new IccProfile(
            IccProfileName.ColorMatch,
            1.8,
            IlluminantName.D50,
            0.63,
            0.34,
            0.274884,
            0.295,
            0.605,
            0.658132,
            0.15,
            0.075,
            0.066985),

        [IccProfileName.DonRgb4] = new IccProfile(
            IccProfileName.DonRgb4,
            2.2,
            IlluminantName.D50,
            0.696,
            0.3,
            0.27835,
            0.215,
            0.765,
            0.68797,
            0.13,
            0.035,
            0.03368),

        [IccProfileName.EciRgb] = new IccProfile(
            IccProfileName.EciRgb,
            1.8,
            IlluminantName.D50,
            0.67,
            0.33,
            0.32025,
            0.21,
            0.71,
            0.602071,
            0.14,
            0.08,
            0.077679),

        [IccProfileName.EktaSpacePS5] = new IccProfile(
            IccProfileName.EktaSpacePS5,
            2.2,
            IlluminantName.D50,
            0.695,
            0.305,
            0.260629,
            0.26,
            0.7,
            0.734946,
            0.11,
            0.005,
            0.004425),

        [IccProfileName.GenericRgb] = new IccProfile(
            IccProfileName.GenericRgb,
            1.8,
            IlluminantName.D65,
            0.6295,
            0.3407,
            0.232546,
            0.2949,
            0.6055,
            0.672501,
            0.1551,
            0.0762,
            0.094952),

        [IccProfileName.Hdtv] = new IccProfile(
            IccProfileName.Hdtv,
            1.95,
            IlluminantName.D65,
            0.64,
            0.33,
            0.212673,
            0.3,
            0.6,
            0.715152,
            0.15,
            0.06,
            0.072175),

        [IccProfileName.Ntsc] = new IccProfile(
            IccProfileName.Ntsc,
                    2.2,
                    IlluminantName.C,
                    0.67,
                    0.33,
                    0.298839,
                    0.21,
                    0.71,
                    0.586811,
                    0.14,
                    0.08,
                    0.11435),

        [IccProfileName.PalSecam] = new IccProfile(
            IccProfileName.PalSecam,
                    2.2,
                    IlluminantName.D65,
                    0.64,
                    0.33,
                    0.222021,
                    0.29,
                    0.6,
                    0.706645,
                    0.15,
                    0.06,
                    0.071334),

        [IccProfileName.ProPhoto] = new IccProfile(
            IccProfileName.ProPhoto,
                    1.8,
                    IlluminantName.D50,
                    0.7347,
                    0.2653,
                    0.28804,
                    0.1596,
                    0.8404,
                    0.711874,
                    0.0366,
                    0.0001,
                    0.000086),

        [IccProfileName.Sgi] = new IccProfile(
            IccProfileName.Sgi,
                    1.47,
                    IlluminantName.D65,
                    0.625,
                    0.34,
                    0.244651,
                    0.28,
                    0.595,
                    0.672030,
                    0.155,
                    0.07,
                    0.083319),

        [IccProfileName.Smpte240M] = new IccProfile(
            IccProfileName.Smpte240M,
                    1.92,
                    IlluminantName.D65,
                    0.63,
                    0.34,
                    0.212413,
                    0.31,
                    0.595,
                    0.701044,
                    0.155,
                    0.07,
                    0.086543),

        [IccProfileName.Smptec] = new IccProfile(
            IccProfileName.Smptec,
                    2.2,
                    IlluminantName.D65,
                    0.63,
                    0.34,
                    0.212395,
                    0.31,
                    0.595,
                    0.701049,
                    0.155,
                    0.07,
                    0.086556),

        [IccProfileName.Srgb] = new IccProfile(
            IccProfileName.Srgb,
                    2.2,
                    IlluminantName.D65,
                    0.64,
                    0.33,
                    0.212656,
                    0.3,
                    0.6,
                    0.715158,
                    0.15,
                    0.06,
                    0.072186),

        [IccProfileName.WideGamut] = new IccProfile(
            IccProfileName.WideGamut,
            2.2,
            IlluminantName.D50,
            0.7347,
            0.2653,
            0.258187,
            0.1152,
            0.8264,
            0.724938,
            0.1566,
            0.0177,
            0.016875)
    };

    /// <summary>
    /// Gets the <see cref="IccProfile"/> associated with the profile name defined by <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The name of the profile to create.</param>
    /// <returns>Returns the created profile.</returns>
    public static IccProfile Get(IccProfileName value)
    {
        if (!s_profiles.TryGetValue(value, out var profile))
        {
            throw new ArgumentException($"The profile name '{value}' is not defined.", nameof(value));
        }

        return profile;
    }
}
