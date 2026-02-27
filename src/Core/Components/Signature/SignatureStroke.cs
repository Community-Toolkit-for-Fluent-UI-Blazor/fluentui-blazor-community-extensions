namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single stroke in a signature.
/// </summary>
public class SignatureStroke
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureStroke"/> class with the specified style.
    /// </summary>
    /// <param name="style">Style information for this stroke.</param>
    public SignatureStroke(SignatureStrokeStyle style)
    {
        Style = style;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureStroke"/> class with the specified style and points.
    /// </summary>
    /// <param name="style">Style information for this stroke.</param>
    /// <param name="points">Points that define the shape of this stroke.</param>
    public SignatureStroke(SignatureStrokeStyle style, IEnumerable<SignaturePoint> points)
    {
        Style = style;
        Points = [.. points];
    }

    /// <summary>
    /// Gets the unique identifier for this stroke.
    /// </summary>
    public string Id { get; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets the list of points that define the shape of this stroke.
    ///  Each point includes coordinates and possibly pressure information.
    /// </summary>
    public List<SignaturePoint> Points { get; } = [];

    /// <summary>
    /// Gets the style information for this stroke, including properties such as color, thickness, and other visual attributes that determine how the stroke is rendered on the canvas.
    /// </summary>
    public SignatureStrokeStyle Style { get; }

    /// <summary>
    /// Clones this stroke, creating a new instance with the same style and a deep copy of the points.
    /// </summary>
    /// <returns>Returns the cloned instance.</returns>
    public SignatureStroke Clone()
    {
        return new SignatureStroke(Style.Clone(), Points.Select(x => x.Clone()));
    }

    /// <summary>
    /// Replaces the contents of this stroke with the contents of another stroke. This method updates the points and style of this stroke to match those of the provided stroke, effectively making this stroke identical to the other stroke in terms of its visual representation and shape. The unique identifier (Id) of this stroke remains unchanged, as it is meant to represent the same logical stroke even after its contents have been replaced.
    /// </summary>
    /// <param name="other">Other stroke whose contents will replace the contents of this stroke.</param>
    public void ReplaceWith(SignatureStroke other)
    {
        Points.Clear();
        Points.AddRange(other.Points.Select(p => p.Clone()));

        Style.Engine = other.Style.Engine.Clone();
        Style.Rendering = other.Style.Rendering.Clone();
    }

    /// <summary>
    /// Translates the stroke by a specified amount in the X and Y directions. This method updates the coordinates of each point in the stroke by adding the specified delta values to their current coordinates, effectively moving the entire stroke on the canvas without altering its shape or style.
    /// </summary>
    /// <param name="deltaX">Delta value to be added to the X-coordinate.</param>
    /// <param name="deltaY">Delta value to be added to the Y-coordinate.</param>
    public void Translate(double deltaX, double deltaY)
    {
        for (var i = 0; i < Points.Count; ++i)
        {
            var point = Points[i];
            point.X += deltaX;
            point.Y += deltaY;
        }
    }

    /// <summary>
    /// Gets the bounding box of this stroke, which is defined by the minimum and maximum X and Y coordinates of the points that make up the stroke. 
    /// </summary>
    /// <returns>Returns the bounding box of this current instance.</returns>
    public (double MinX, double MinY, double MaxX, double MaxY) GetBounds()
    {
        if (Points.Count == 0)
        {
            return (0, 0, 0, 0);
        }

        var minX = Points.Min(p => p.X);
        var minY = Points.Min(p => p.Y);
        var maxX = Points.Max(p => p.X);
        var maxY = Points.Max(p => p.Y);

        return (minX, minY, maxX, maxY);
    }
}
