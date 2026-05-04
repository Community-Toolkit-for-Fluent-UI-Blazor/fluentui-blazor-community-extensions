namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration for a pen used in drawing operations, including color, width, style, and shadow
/// effects.
/// </summary>
public sealed class PenPayload : IEquatable<PenPayload>
{
    /// <summary>
    /// Gets or sets the color of the pen.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the pen stroke.
    /// </summary>
    public double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the style used to draw the ends of lines or strokes.
    /// </summary>
    public LineCap LineCap { get; set; }

    /// <summary>
    /// Gets or sets the style used to join two lines where they meet at a corner.
    /// </summary>
    public LineJoin LineJoin { get; set; }

    /// <summary>
    /// Gets or sets the sequence of lengths that specify the pattern of dashes and gaps used when rendering lines or
    /// shapes.
    /// </summary>
    public double[] DashArray { get; set; } = [];

    /// <summary>
    /// Gets or sets the shadow configuration for the component.
    /// </summary>
    public ShadowPayload Shadow { get; set; } = new ShadowPayload();

    /// <summary>
    /// Gets or sets the width value for the pen.
    /// </summary>
    public double Width { get; set; } = 1.0;

    /// <inheritdoc />
    public bool Equals(PenPayload? other)
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
               Opacity.Equals(other.Opacity) &&
               LineCap == other.LineCap &&
               LineJoin == other.LineJoin &&
               Shadow.Equals(other.Shadow) &&
               DashArray.SequenceEqual(other.DashArray);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is PenPayload other)
        {
            return Equals(other);
        }

        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Color);
        hashCode.Add(Opacity);
        hashCode.Add(LineCap);
        hashCode.Add(LineJoin);
        hashCode.Add(Shadow);

        foreach (var item in DashArray)
        {
            hashCode.Add(item);
        }

        return hashCode.ToHashCode();
    }
}
