namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the SVG defs element, which is used to define graphical objects that can be reused within an SVG
/// document.
/// </summary>
public sealed class SvgDefs : SvgElement
{
    /// <inheritdoc />
    public override string TagName => "defs";
}
