namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the different image formats that can be used when exporting a signature from the Signature component.
/// </summary>
public enum SignatureImageFormat
{
    /// <summary>
    /// Represents the BMP (Bitmap) image format.
    /// </summary>
    /// <remarks>The BMP format is a widely used raster graphics image file format that stores bitmap digital
    /// images. It supports various color depths and compression methods.</remarks>
    Bmp,

    /// <summary>
    /// Represents the Portable Network Graphics (PNG) image format.
    /// </summary>
    /// <remarks>PNG is a raster-graphics file format that supports lossless data compression. It is commonly
    /// used for web graphics and supports transparency.</remarks>
    Png,

    /// <summary>
    /// Represents a JPEG image format.
    /// </summary>
    /// <remarks>This class or enumeration is used to identify or work with JPEG image files,  which are
    /// commonly used for storing and transmitting photographic images.</remarks>
    Jpeg,

    /// <summary>
    /// Represents the WebP image format.
    /// </summary>
    /// <remarks>WebP is a modern image format that provides superior lossless and lossy compression for
    /// images on the web. It is designed to reduce image file sizes while maintaining high visual quality.</remarks>
    Webp,

    /// <summary>
    /// Represents an animated image in the Graphics Interchange Format (GIF).
    /// </summary>
    /// <remarks>This class provides functionality for working with GIF images, including accessing frames,
    /// metadata, and playback properties. It can be used to load, manipulate, and display GIF animations.</remarks>
    Gif,

    /// <summary>
    /// Represents an SVG (Scalable Vector Graphics) element, which is a vector-based image format  for defining
    /// graphics using XML.
    /// </summary>
    /// <remarks>This class provides functionality for working with SVG elements, including creating, 
    /// manipulating, and rendering SVG content. SVG is widely used for scalable and resolution-independent  graphics in
    /// web and application development.</remarks>
    Svg,

    /// <summary>
    /// Represents a PDF (Portable Document Format) document, which is a file format used to present and exchange.
    /// </summary>
    /// <remarks>PDF documents are widely used for sharing and printing documents while preserving their formatting.</remarks>
    Pdf
}
