namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Returns a predefined set of colors that are designed to be accessible and distinguishable.
/// </summary>
public sealed class AccessibilitySafePlugin
    : IColorPlugin
{
    /// <summary>
    /// Represents a set of colors that are designed to be accessible and distinguishable.
    /// </summary>
    private static readonly List<string> _safeColors = [
        "#66C2A5", "#FC8D62", "#8DA0CB", "#E78AC3",
        "#A6D854", "#FFD92F", "#E5C494", "#B3B3B3"
    ];

    /// <inheritdoc />
    public string Name => "AccessibilitySafe";

    /// <inheritdoc />
    public List<string> Generate(
        string baseColor,
        int steps,
        GenerationOptions options)
    {
        return [.. _safeColors.Take(Math.Min(steps, _safeColors.Count))];
    }
}
