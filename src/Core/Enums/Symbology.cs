namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the supported barcode symbologies.
/// </summary>
/// <remarks>Use this enumeration to select the barcode format when generating or processing barcodes. Additional
/// symbologies may be added in future versions.</remarks>
public enum Symbology
{
    /// <summary>
    /// Represents the Code 128 barcode symbology, which encodes alphanumeric data using a high-density linear barcode
    /// format.
    /// </summary>
    Code128,

    /// <summary>
    /// Represents the Code 39 barcode symbology, which encodes alphanumeric data using a standardized set of
    /// characters.
    /// </summary>
    Code39,

    /// <summary>
    /// Represents a barcode format that uses the UPC-A (Universal Product Code version A) symbology.
    /// </summary>
    UpcA,

    /// <summary>
    /// Represents a Universal Product Code version E (UPC-E) barcode format.
    /// </summary>
    UpcE,

    /// <summary>
    /// Represents the Code 93 barcode symbology, which is used for encoding alphanumeric data in a compact barcode
    /// format.
    /// </summary>
    Code93,

    /// <summary>
    /// Represents an EAN-13 barcode, a 13-digit international standard used for product identification.
    /// </summary>
    Ean13,

    /// <summary>
    /// Represents the EAN-8 barcode symbology, which encodes 8-digit numbers for product identification.
    /// </summary>
    /// <remarks>EAN-8 barcodes are commonly used on small packages where space is limited. They encode a
    /// 7-digit data payload plus a check digit, and are a subset of the EAN/UPC barcode family.</remarks>
    Ean8,

    /// <summary>
    /// Represents the GS1-128 barcode symbology, which encodes data using the GS1 system for supply chain
    /// identification and tracking.
    /// </summary>
    GS1_128,

    /// <summary>
    /// Represents the ITF-14 barcode symbology, which is used for encoding Global Trade Item Numbers (GTIN) on shipping
    /// containers.
    /// </summary>
    Itf14,

    /// <summary>
    /// Represents the Codabar barcode symbology used for encoding numeric and select special characters in barcodes.
    /// </summary>
    /// <remarks>Codabar is commonly used in libraries, blood banks, and air parcel services for encoding
    /// numbers and a limited set of special characters. It is a discrete, self-checking symbology that does not require
    /// a checksum for most applications.</remarks>
    Codabar,

    /// <summary>
    /// Represents the QR Code (Quick Response Code) symbology, a two-dimensional barcode format that encodes data in a matrix of black and white squares.
    /// QR Code can encode a wide variety of data types, including numeric, alphanumeric, byte/binary characters, making it suitable for applications such as URL encoding, contact information sharing, and product labeling.
    /// </summary>
    QRCode,

    /// <summary>
    /// Specifies the PDF417 barcode symbology.
    /// </summary>
    /// <remarks>PDF417 is a two-dimensional barcode format capable of encoding large amounts of data. It is
    /// commonly used in applications such as transport, identification cards, and inventory management.</remarks>
    Pdf417
}
