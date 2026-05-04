namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration payload for displaying debug text in a signature component, including font family, font
/// size, and color.
/// </summary>
public sealed class DebugTextPayload : IEquatable<DebugTextPayload>
{
    /// <summary>
    /// Gets the font family used for rendering text in the component.
    /// </summary>
    public string FontFamily { get; init; } = "monospace";

    /// <summary>
    /// Gets the font size used to render text content.
    /// </summary>
    public double FontSize { get; init; } = 14;

    /// <summary>
    /// Gets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; init; } = "#FF0000";

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Color, FontFamily, FontSize);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is DebugTextPayload other)
        {
            return Equals(this, other);
        }

        return false;
    }

    /// <inheritdoc />
    public bool Equals(DebugTextPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Color, other.Color, StringComparison.OrdinalIgnoreCase) &&
               FontSize == other.FontSize &&
               string.Equals(FontFamily, other.FontFamily, StringComparison.OrdinalIgnoreCase);
    }
}

