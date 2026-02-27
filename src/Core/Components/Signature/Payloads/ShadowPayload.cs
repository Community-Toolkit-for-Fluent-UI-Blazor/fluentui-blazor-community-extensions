namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration for a shadow effect, including color, opacity, blur, offset, and scaling behavior.
/// </summary>
public sealed class ShadowPayload
    : IEquatable<ShadowPayload>
{
    /// <summary>
    /// Gets or sets a value indicating whether the shadow is enabled.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the color of the shadow.
    /// </summary>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the opacity level of the shadow.
    /// </summary>
    public double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the blur radius to apply to the shadow, in pixels.
    /// </summary>
    public double Blur { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset value.
    /// </summary>
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset value.
    /// </summary>
    public double OffsetY { get; set; }

    /// <inheritdoc />
    public bool Equals(ShadowPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Enabled == other.Enabled
            && Color == other.Color
            && Opacity == other.Opacity
            && Blur == other.Blur
            && OffsetX == other.OffsetX
            && OffsetY == other.OffsetY;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not ShadowPayload other)
        {
            return false;
        }

        return Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(Enabled);
        hash.Add(Color);
        hash.Add(Opacity);
        hash.Add(Blur);
        hash.Add(OffsetX);
        hash.Add(OffsetY);

        return hash.ToHashCode();
    }
}
