namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Specifies the version of a QR code to use when generating.
/// </summary>
/// <remarks>QR code versions determine the size and data capacity of the QR code symbol. Version 1 is the
/// smallest, and version 40 is the largest, with each increment increasing the number of modules and data capacity. The
/// 'Auto' value allows the system to select the most appropriate version based on the input data length and error
/// correction requirements.</remarks>
public enum QRVersion
{
    /// <summary>
    /// Specifies that the value is determined automatically based on context.
    /// </summary>
    Auto = 0,

    /// <summary>
    /// Specifies that the QR code version is 1, which is the smallest version with a 21x21 module matrix and can encode up to 25 alphanumeric characters.
    /// </summary>
    V1 = 1,

    /// <summary>
    /// Specifies that the QR code version is 2, which has a 25x25 module matrix and can encode up to 47 alphanumeric characters.
    /// </summary>
    V2 = 2,

    /// <summary>
    /// Specifies that the QR code version is 3, which has a 29x29 module matrix and can encode up to 77 alphanumeric characters.
    /// </summary>
    V3 = 3,

    /// <summary>
    /// Specifies that the QR code version is 4, which has a 33x33 module matrix and can encode up to 114 alphanumeric characters.
    /// </summary>
    V4 = 4,

    /// <summary>
    /// Specifies that the QR code version is 5, which has a 37x37 module matrix and can encode up to 154 alphanumeric characters.
    /// </summary>
    V5 = 5,

    /// <summary>
    /// Specifies that the QR code version is 6, which has a 41x41 module matrix and can encode up to 195 alphanumeric characters.
    /// </summary>
    V6 = 6,

    /// <summary>
    /// Specifies that the QR code version is 7, which has a 45x45 module matrix and can encode up to 224 alphanumeric characters.
    /// </summary>
    V7 = 7,

    /// <summary>
    /// Specifies that the QR code version is 8, which has a 49x49 module matrix and can encode up to 279 alphanumeric characters.
    /// </summary>
    V8 = 8,

    /// <summary>
    /// Specifies that the QR code version is 9, which has a 53x53 module matrix and can encode up to 335 alphanumeric characters.
    /// </summary>
    V9 = 9,

    /// <summary>
    /// Specifies that the QR code version is 10, which has a 57x57 module matrix and can encode up to 395 alphanumeric characters.
    /// </summary>
    V10 = 10,

    /// <summary>
    /// Specifies that the QR code version is 11, which has a 61x61 module matrix and can encode up to 468 alphanumeric characters.
    /// </summary>
    V11 = 11,

    /// <summary>
    /// Specifies that the QR code version is 12, which has a 65x65 module matrix and can encode up to 535 alphanumeric characters.
    /// </summary>
    V12 = 12,

    /// <summary>
    /// Specifies that the QR code version is 13, which has a 69x69 module matrix and can encode up to 619 alphanumeric characters.
    /// </summary>
    V13 = 13,

    /// <summary>
    /// Specifies that the QR code version is 14, which has a 73x73 module matrix and can encode up to 667 alphanumeric characters.
    /// </summary>
    V14 = 14,

    /// <summary>
    /// Specifies that the QR code version is 15, which has a 77x77 module matrix and can encode up to 758 alphanumeric characters.
    /// </summary>
    V15 = 15,

    /// <summary>
    /// Specifies that the QR code version is 16, which has an 81x81 module matrix and can encode up to 864 alphanumeric characters.
    /// </summary>
    V16 = 16,

    /// <summary>
    /// Specifies that the QR code version is 17, which has an 85x85 module matrix and can encode up to 938 alphanumeric characters.
    /// </summary>
    V17 = 17,

    /// <summary>
    /// Specifies that the QR code version is 18, which has an 89x89 module matrix and can encode up to 1,062 alphanumeric characters.
    /// </summary>
    V18 = 18,

    /// <summary>
    /// Specifies that the QR code version is 19, which has a 93x93 module matrix and can encode up to 1,122 alphanumeric characters.
    /// </summary>
    V19 = 19,

    /// <summary>
    /// Specifies that the QR code version is 20, which has a 97x97 module matrix and can encode up to 1,305 alphanumeric characters.
    /// </summary>
    V20 = 20,

    /// <summary>
    /// Specifies that the QR code version is 21, which has a 101x101 module matrix and can encode up to 1,593 alphanumeric characters.
    /// </summary>
    V21 = 21,

    /// <summary>
    /// Specifies that the QR code version is 22, which has a 105x105 module matrix and can encode up to 1,919 alphanumeric characters.
    /// </summary>
    V22 = 22,

    /// <summary>
    /// Specifies that the QR code version is 23, which has a 109x109 module matrix and can encode up to 2,335 alphanumeric characters.
    /// </summary>
    V23 = 23,

    /// <summary>
    /// Specifies that the QR code version is 24, which has a 113x113 module matrix and can encode up to 2,655 alphanumeric characters.
    /// </summary>
    V24 = 24,

    /// <summary>
    /// Specifies that the QR code version is 25, which has a 117x117 module matrix and can encode up to 2,953 alphanumeric characters.
    /// </summary>
    V25 = 25,

    /// <summary>
    /// Specifies that the QR code version is 26, which has a 121x121 module matrix and can encode up to 3,589 alphanumeric characters.
    /// </summary>
    V26 = 26,

    /// <summary>
    /// Specifies that the QR code version is 27, which has a 125x125 module matrix and can encode up to 4,253 alphanumeric characters.
    /// </summary>
    V27 = 27,

    /// <summary>
    /// Specifies that the QR code version is 28, which has a 129x129 module matrix and can encode up to 4,769 alphanumeric characters.
    /// </summary>
    V28 = 28,

    /// <summary>
    /// Specifies that the QR code version is 29, which has a 133x133 module matrix and can encode up to 5,929 alphanumeric characters.
    /// </summary>
    V29 = 29,

    /// <summary>
    /// Specifies that the QR code version is 30, which has a 137x137 module matrix and can encode up to 6,653 alphanumeric characters.
    /// </summary>
    V30 = 30,

    /// <summary>
    /// Specifies that the QR code version is 31, which has a 141x141 module matrix and can encode up to 7,089 alphanumeric characters.
    /// </summary>
    V31 = 31,

    /// <summary>
    /// Specifies that the QR code version is 32, which has a 145x145 module matrix and can encode up to 8,077 alphanumeric characters.
    /// </summary>
    V32 = 32,

    /// <summary>
    /// Specifies that the QR code version is 33, which has a 149x149 module matrix and can encode up to 8,583 alphanumeric characters.
    /// </summary>
    V33 = 33,

    /// <summary>
    /// Specifies that the QR code version is 34, which has a 153x153 module matrix and can encode up to 9,321 alphanumeric characters.
    /// </summary>
    V34 = 34,

    /// <summary>
    /// Specifies that the QR code version is 35, which has a 157x157 module matrix and can encode up to 9,932 alphanumeric characters.
    /// </summary>
    V35 = 35,

    /// <summary>
    /// Specifies that the QR code version is 36, which has a 161x161 module matrix and can encode up to 10,737 alphanumeric characters.
    /// </summary>
    V36 = 36,

    /// <summary>
    /// Specifies that the QR code version is 37, which has a 165x165 module matrix and can encode up to 11,089 alphanumeric characters.
    /// </summary>
    V37 = 37,

    /// <summary>
    /// Specifies that the QR code version is 38, which has a 169x169 module matrix and can encode up to 12,525 alphanumeric characters.
    /// </summary>
    V38 = 38,

    /// <summary>
    /// Specifies that the QR code version is 39, which has a 173x173 module matrix and can encode up to 13,347 alphanumeric characters.
    /// </summary>
    V39 = 39,

    /// <summary>
    /// Specifies that the QR code version is 40, which has a 177x177 module matrix and can encode up to 14,647 alphanumeric characters.
    /// </summary>
    V40 = 40
}

