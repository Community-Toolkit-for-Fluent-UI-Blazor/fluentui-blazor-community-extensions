namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the shape to be used at the join between two connected lines.
/// </summary>
public enum LineJoin
{
    /// <summary>
    /// Represents a sharp corner or a pointed join between two lines.
    /// </summary>
    Miter,

    /// <summary>
    /// Represents a rounded join between two lines.
    /// </summary>
    Round,

    /// <summary>
    /// Represents a beveled join, which creates a flattened corner between two lines.
    /// </summary>
    Bevel
}
