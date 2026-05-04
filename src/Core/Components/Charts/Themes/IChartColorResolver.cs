using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a service that resolves chart colors based on provided input parameters.
/// </summary>
public interface IChartColorResolver
{
    /// <summary>
    /// Asynchronously resolves an sRGB color value based on the provided parameters, using a fallback if necessary.
    /// </summary>
    /// <param name="srgb8Value">An optional sRGB color value to use if specified. If null, the method attempts to resolve the color using the
    /// variable value or the fallback.</param>
    /// <param name="varValue">An optional variable name or value that may be used to resolve the sRGB color. If null or resolution fails, the
    /// fallback value is used.</param>
    /// <param name="fallbackValue">The sRGB color value to use if neither srgb8Value nor varValue can be resolved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the resolved sRGB color value.</returns>
    Task<Srgb8> ResolveAsync(
        Srgb8? srgb8Value,
        string? varValue,
        Srgb8 fallbackValue);
}
