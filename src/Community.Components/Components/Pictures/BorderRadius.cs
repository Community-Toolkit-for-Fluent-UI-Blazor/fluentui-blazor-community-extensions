namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the border radius options for a picture component.
/// </summary>
public enum BorderRadius
{
    /// <summary>
    /// No radius applied.
    /// </summary>
    None,

    /// <summary>
    /// A circular radius applied, making the picture appear as a circle.
    /// </summary>
    Circle,

    /// <summary>
    /// A square radius applied, making the picture appear with sharp corners.
    /// </summary>
    Square,

    /// <summary>
    /// A rounded square radius applied, making the picture appear with slightly rounded corners.
    /// </summary>
    RoundSquare,

    /// <summary>
    /// A custom radius applied, allowing for specific radius values to be set.
    /// </summary>
    Custom
}
