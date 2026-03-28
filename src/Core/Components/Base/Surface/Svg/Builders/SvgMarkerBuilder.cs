using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a builder for configuring and constructing SVG marker elements within an SVG document.
/// </summary>
/// <remarks>Use this class to fluently set marker attributes such as reference point, size, and orientation, and
/// to add path elements to the marker. The builder pattern enables chaining of configuration methods before returning
/// to the parent SVG builder context.</remarks>
public sealed class SvgMarkerBuilder : SvgElementBuilderBase<SvgMarkerBuilder, SvgMarker>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SvgMarkerBuilder"/> class with the specified parent builder and marker
    /// definition.
    /// </summary>
    /// <param name="parent">The parent <see cref="SvgBuilder"/> that this builder is associated with. Cannot be null.</param>
    /// <param name="marker">The <see cref="SvgMarker"/> instance that defines the marker to be built. Cannot be null.</param>
    public SvgMarkerBuilder(SvgBuilder parent, SvgMarker marker)
        : base(parent, marker)
    {
    }

    /// <summary>
    /// Sets the reference point coordinates for the marker element in the SVG output.
    /// </summary>
    /// <remarks>The reference point determines the position within the marker that will be aligned with the
    /// point on the path where the marker is placed.</remarks>
    /// <param name="refX">The x-coordinate of the reference point, in user units, relative to the marker's bounding box.</param>
    /// <param name="refY">The y-coordinate of the reference point, in user units, relative to the marker's bounding box.</param>
    /// <returns>The current instance of <see cref="SvgMarkerBuilder"/> to allow method chaining.</returns>
    public SvgMarkerBuilder WithRef(double refX, double refY)
    {
        return WithAttribute("refX", refX.ToSvg())
            .WithAttribute("refY", refY.ToSvg());
    }

    /// <summary>
    /// Sets the width and height of the SVG marker element in user units.
    /// </summary>
    /// <remarks>Use this method to specify the dimensions of the marker when rendering SVG graphics. Setting
    /// appropriate marker size ensures correct scaling and appearance in the final SVG output.</remarks>
    /// <param name="width">The width of the marker, in user units. Must be a non-negative value.</param>
    /// <param name="height">The height of the marker, in user units. Must be a non-negative value.</param>
    /// <returns>The current <see cref="SvgMarkerBuilder"/> instance with the updated marker size.</returns>
    public SvgMarkerBuilder WithSize(double width, double height)
    {
        return WithAttribute("markerWidth", width.ToSvg())
            .WithAttribute("markerHeight", height.ToSvg());
    }

    /// <summary>
    /// Sets the orientation attribute for the SVG marker element.
    /// </summary>
    /// <remarks>The 'orient' attribute determines the rotation of the marker relative to the path it is
    /// attached to. Common values include 'auto' or a numeric angle in degrees.</remarks>
    /// <param name="orient">The orientation value to assign to the marker's 'orient' attribute. This can be a specific angle or the keyword
    /// 'auto' to allow automatic orientation.</param>
    /// <returns>The current instance of <see cref="SvgMarkerBuilder"/>, enabling method chaining.</returns>
    public SvgMarkerBuilder WithOrient(string orient)
    {
        return WithAttribute("orient", orient);
    }

    /// <summary>
    /// Adds a new path element to the current SVG marker and returns a builder for configuring the path.
    /// </summary>
    /// <remarks>Use the returned <see cref="SvgPathBuilder"/> to set properties or add commands to the newly
    /// created path. This method is typically used when constructing complex SVG graphics programmatically.</remarks>
    /// <returns>A new instance of <see cref="SvgPathBuilder"/> for further configuration of the added path element.</returns>
    public SvgPathBuilder AddPath()
    {
        var path = new SvgPath();
        Element.Children.Add(path);

        return new SvgPathBuilder(Parent, path);
    }
}

