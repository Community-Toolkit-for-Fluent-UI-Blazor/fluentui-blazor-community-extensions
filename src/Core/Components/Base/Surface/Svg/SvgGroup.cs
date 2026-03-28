namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an SVG group ('g') element that is used to group SVG shapes and elements for collective transformations
/// or styling.
/// </summary>
public sealed class SvgGroup : SvgElement
{
    /// <inheritdoc />
    public override string TagName => "g";
}
