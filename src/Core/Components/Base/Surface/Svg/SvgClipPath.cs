namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an SVG clipPath element, which defines a clipping path to restrict the region of visibility for child
/// elements.
/// </summary>
public sealed class SvgClipPath : SvgElement
{
    /// <inheritdoc />
    public override string TagName => "clipPath";
}
