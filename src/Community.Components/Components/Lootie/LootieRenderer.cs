namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the rendering format options for a Lootie animation.
/// </summary>
/// <remarks>Use this enumeration to select the desired output format when rendering a Lootie animation.</remarks>
public enum LootieRenderer
{
    /// <summary>
    /// Represents an SVG (Scalable Vector Graphics) element, which is a vector-based image format for defining graphics
    /// using XML.
    /// </summary>
    /// <remarks>This class provides functionality for working with SVG elements, including creating,
    /// manipulating, and rendering SVG content. SVG is widely used for scalable and resolution-independent graphics in
    /// web and application development.</remarks>
    Svg,

    /// <summary>
    /// Represents a drawable surface for rendering graphical elements.
    /// </summary>
    /// <remarks>The <see cref="Canvas"/> class provides a surface for drawing shapes, images, and other
    /// graphical content. It can be used in graphical applications to manage and render visual elements.</remarks>
    Canvas,

    /// <summary>
    /// Represents an HTML document or fragment, providing methods and properties to manipulate and interact with its
    /// structure and content.
    /// </summary>
    /// <remarks>This class can be used to parse, modify, and generate HTML content. It provides functionality
    /// for working with elements, attributes, and text nodes within an HTML structure. Use this class to
    /// programmatically create or manipulate HTML documents.</remarks>
    Html
}
