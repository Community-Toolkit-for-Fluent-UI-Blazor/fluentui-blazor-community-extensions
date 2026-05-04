namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the available image export formats that can be used when exporting a surface.
/// </summary>
/// <remarks>This enumeration supports bitwise combination of its member values to allow specifying multiple
/// export formats. Use the None value to indicate that no export formats are permitted.</remarks>
[Flags]
public enum SurfaceExportFormat
{
    /// <summary>
    /// Specifies that no export format is allowed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Specifies that the AVIF image format is allowed for exporting the surface.
    /// </summary>
    Avif = 1,

    /// <summary>
    /// Specifies that the BMP image format is allowed for exporting the surface.
    /// </summary>
    Bmp = 2,

    /// <summary>
    /// Specifies that the HEIF image format is allowed for exporting the surface.
    /// </summary>
    Heif = 4,

    /// <summary>
    /// Specifies that the JPEG image format is allowed for exporting the surface.
    /// </summary>
    Jpeg = 8,

    /// <summary>
    /// Specifies that the PNG image format is allowed for exporting the surface.
    /// </summary>
    Png = 16,

    /// <summary>
    /// Specifies that the TIFF image format is allowed for exporting the surface.
    /// </summary>
    Tiff = 32,

    /// <summary>
    /// Specifies that the WEBP image format is allowed for exporting the surface.
    /// </summary>
    Webp = 64,

    /// <summary>
    /// Represents a combination of all supported image formats.
    /// </summary>
    All = Avif | Bmp | Heif | Jpeg | Png | Tiff | Webp
}
