using System.ComponentModel;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents length units for CSS properties.
/// </summary>
public enum LengthUnit
{
    /// <summary>
    /// Represents pixels, the most common unit for lengths in web design.
    /// </summary>
    [Description("px")]
    Pixels,

    /// <summary>
    /// Represents percentage, a relative unit based on the parent element's size.
    /// </summary>
    [Description("%")]
    Percent,

    /// <summary>
    /// Represents em, a relative unit based on the font size of the element.
    /// </summary>
    [Description("em")]
    Em,

    /// <summary>
    /// Represents rem, a relative unit based on the font size of the root element.
    /// </summary>
    [Description("rem")]
    Rem,

    /// <summary>
    /// Represents vw, a relative unit based on 1% of the viewport's width.
    /// </summary>
    [Description("vw")]
    ViewportWidth,

    /// <summary>
    /// Represents vh, a relative unit based on 1% of the viewport's height.
    /// </summary>
    [Description("vh")]
    ViewportHeight,

    /// <summary>
    /// Represents vmin, a relative unit based on 1% of the smaller dimension of the viewport (width or height).
    /// </summary>
    [Description("vmin")]
    ViewportMin,

    /// <summary>
    /// Represents vmax, a relative unit based on 1% of the larger dimension of the viewport (width or height).
    /// </summary>
    [Description("vmax")]
    ViewportMax,

    /// <summary>
    /// Represents centimeters, a physical unit of length.
    /// </summary>
    [Description("cm")]
    Centimeters,

    /// <summary>
    /// Represents millimeters, a physical unit of length.
    /// </summary>
    [Description("mm")]
    Millimeters,

    /// <summary>
    /// Represents inches, a physical unit of length.
    /// </summary>
    [Description("in")]
    Inches,

    /// <summary>
    /// Represents points, a physical unit of length commonly used in typography (1 point = 1/72 inch).
    /// </summary>
    [Description("pt")]
    Points,

    /// <summary>
    /// Represents picas, a physical unit of length commonly used in typography (1 pica = 12 points).
    /// </summary>
    [Description("pc")]
    Picas,

    /// <summary>
    /// Represents character units, a relative unit based on the width of the "0" (zero) character in the element's font.
    /// </summary>
    [Description("ch")]
    Character,

    /// <summary>
    /// Represents x-height units, a relative unit based on the height of lowercase "x" in the element's font.
    /// </summary>
    [Description("ex")]
    XHeight,
}
