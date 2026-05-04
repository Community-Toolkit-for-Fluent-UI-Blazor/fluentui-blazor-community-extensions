namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the payload for configuring a background, including its visibility, color, and opacity.
/// </summary>
public sealed class BackgroundPayload : ILayerPayload, IEquatable<BackgroundPayload>
{
    /// <summary>
    /// Gets the color of the background.
    /// </summary>
    public string Color { get; init; } = string.Empty;

    /// <summary>
    /// Gets the opacity of the background.
    /// </summary>
    public double Opacity { get; init; }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Color, Opacity);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is BackgroundPayload other)
        {
            return Equals(this, other);
        }

        return false;
    }

    /// <inheritdoc />
    public bool Equals(BackgroundPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Color.Equals(other.Color, StringComparison.OrdinalIgnoreCase) &&
               Opacity == other.Opacity;
    }
}
