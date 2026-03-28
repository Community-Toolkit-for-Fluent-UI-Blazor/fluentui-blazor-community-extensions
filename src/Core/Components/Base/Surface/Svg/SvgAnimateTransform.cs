namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an SVG &lt;animateTransform&gt; element, which animates transformations such as translation, scaling, rotation,
/// or skewing on a target SVG element.
/// </summary>
public sealed class SvgAnimateTransform : SvgElement
{
    /// <inheritdoc />
    public override string TagName => "animateTransform";
}
