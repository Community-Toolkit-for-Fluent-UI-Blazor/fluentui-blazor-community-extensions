namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a plugin for generating color palettes based on a base color and specified options.
/// </summary>
public interface IColorPlugin
{
    /// <summary>
    /// Gets the name of the color plugin.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Generates a list of color variations based on the specified base color.
    /// </summary>
    /// <param name="baseColor">The base color from which variations will be generated. Must be a valid color string (e.g., a hex code or color
    /// name).</param>
    /// <param name="steps">The number of variations to generate. Must be a positive integer.</param>
    /// <param name="options">An object specifying additional options for color generation, such as brightness or saturation adjustments.</param>
    /// <returns>A list of strings representing the generated color variations. The list will contain exactly <paramref
    /// name="steps"/> items.</returns>
    List<string> Generate(string baseColor, int steps, GenerationOptions options);
}

