using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Helpers.Pdf417;

/// <summary>
/// Represents a segment of data encoded in a specific mode for a PDF417 barcode.
/// </summary>
/// <param name="Mode">The encoding mode used for this segment.</param>
/// <param name="Content">The data content to be encoded in this segment.</param>
internal readonly record struct Pdf417Segment(Pdf417EncodingMode Mode, string Content);
