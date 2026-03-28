using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides version information for QR codes, including data for all supported QR code versions.
/// </summary>
/// <remarks>This class contains static data representing the characteristics of each QR code version, which can
/// be used to determine encoding capacities and error correction capabilities. It is intended for internal use within
/// QR code processing components.</remarks>
internal static class QRVersionInfo
{
    /// <summary>
    /// Provides an array containing data for each supported QR code version.
    /// </summary>
    /// <remarks>Each element in the array corresponds to a specific QR code version, indexed from 0 to 39.
    /// The data can be used to retrieve version-specific information when encoding or decoding QR codes.</remarks>
    public static readonly QRVersionData[] Versions =
    [
       new()
       {
            Version = 1,
            Size = 21,
            TotalCodewords = 26,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 1, DataCodewords = 19, EccCodewords = 7 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 1, DataCodewords = 16, EccCodewords = 10 }],
                [QRErrorCorrectionLevel.Quartile] = [new QREccInfo { BlockCount = 1, DataCodewords = 13, EccCodewords = 13 }],
                [QRErrorCorrectionLevel.High] = [new QREccInfo { BlockCount = 1, DataCodewords = 9, EccCodewords = 17 }]
            }
       },
       new()
       {
            Version = 2,
            Size = 25,
            TotalCodewords = 44,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 1, DataCodewords = 34, EccCodewords = 10 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 1, DataCodewords = 28, EccCodewords = 16 }],
                [QRErrorCorrectionLevel.Quartile] = [new QREccInfo { BlockCount = 1, DataCodewords = 22, EccCodewords = 22 }],
                [QRErrorCorrectionLevel.High] = [new QREccInfo { BlockCount = 1, DataCodewords = 16, EccCodewords = 28 }]
            }
       },
        new()
        {
            Version = 3,
            Size = 29,
            TotalCodewords = 70,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 1, DataCodewords = 55, EccCodewords = 15 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 1, DataCodewords = 44, EccCodewords = 26 }],
                [QRErrorCorrectionLevel.Quartile] = [new QREccInfo { BlockCount = 2, DataCodewords = 17, EccCodewords = 18 }],
                [QRErrorCorrectionLevel.High] = [new QREccInfo { BlockCount = 2, DataCodewords = 13, EccCodewords = 22 }]
            }
        },
        new()
        {
            Version = 4,
            Size = 33,
            TotalCodewords = 100,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 1, DataCodewords = 80, EccCodewords = 20 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 2, DataCodewords = 32, EccCodewords = 18 }],
                [QRErrorCorrectionLevel.Quartile] = [new QREccInfo { BlockCount = 2, DataCodewords = 24, EccCodewords = 26 }],
                [QRErrorCorrectionLevel.High] = [new QREccInfo { BlockCount = 4, DataCodewords = 9, EccCodewords = 16 }]
            }
        },
        new()
        {
            Version = 5,
            Size = 37,
            TotalCodewords = 134,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 1, DataCodewords = 108, EccCodewords = 26 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 2, DataCodewords = 43, EccCodewords = 24 }],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 15, EccCodewords = 18 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 16, EccCodewords = 18 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 11, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 12, EccCodewords = 22 }
                ]
            }
        },
        new()
        {
            Version = 6,
            Size = 41,
            TotalCodewords = 172,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 2, DataCodewords = 68, EccCodewords = 18 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 4, DataCodewords = 27, EccCodewords = 16 }],
                [QRErrorCorrectionLevel.Quartile] = [new QREccInfo { BlockCount = 4, DataCodewords = 19, EccCodewords = 24 }],
                [QRErrorCorrectionLevel.High] = [new QREccInfo { BlockCount = 4, DataCodewords = 15, EccCodewords = 28 }]
            }
        },
        new()
        {
            Version = 7,
            Size = 45,
            TotalCodewords = 196,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 2, DataCodewords = 78, EccCodewords = 20 }],
                [QRErrorCorrectionLevel.Medium] = [new QREccInfo { BlockCount = 4, DataCodewords = 31, EccCodewords = 18 }],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 14, EccCodewords = 18 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 15, EccCodewords = 18 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 13, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 14, EccCodewords = 26 }
                ]
            }
        },
        new()
        {
            Version = 8,
            Size = 49,
            TotalCodewords = 242,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 2, DataCodewords = 97, EccCodewords = 24 }],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 38, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 39, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 18, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 19, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 14, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 15, EccCodewords = 26 }
                ]
            }
        },
        new()
        {
            Version = 9,
            Size = 53,
            TotalCodewords = 292,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 2, DataCodewords = 116, EccCodewords = 30 }],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 36, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 37, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 16, EccCodewords = 20 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 17, EccCodewords = 20 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 12, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 13, EccCodewords = 24 }
                ]
            }
        },
        new()
        {
            Version = 10,
            Size = 57,
            TotalCodewords = 346,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 68, EccCodewords = 18 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 69, EccCodewords = 18 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 43, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 44, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 19, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 20, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 15, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 16, EccCodewords = 28 }
                ]
            }
        },
        new()
        {
            Version = 11,
            Size = 61,
            TotalCodewords = 404,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 4, DataCodewords = 81, EccCodewords = 20 }],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 1, DataCodewords = 50, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 51, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 22, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 23, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 12, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 8, DataCodewords = 13, EccCodewords = 24 }
                ]
            }
        },
        new()
        {
            Version = 12,
            Size = 65,
            TotalCodewords = 466,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 92, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 93, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 36, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 37, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 20, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 21, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 7, DataCodewords = 14, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 15, EccCodewords = 28 }
                ]
            }
        },
        new()
        {
            Version = 13,
            Size = 69,
            TotalCodewords = 532,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [new QREccInfo { BlockCount = 4, DataCodewords = 107, EccCodewords = 26 }],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 37, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 38, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 20, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 21, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 12, DataCodewords = 11, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 12, EccCodewords = 22 }
                ]
            }
        },
        new()
        {
            Version = 14,
            Size = 73,
            TotalCodewords = 581,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 40, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 41, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 16, EccCodewords = 20 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 17, EccCodewords = 20 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 12, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 13, EccCodewords = 24 }
                ]
            }
        },
        new()
        {
            Version = 15,
            Size = 77,
            TotalCodewords = 655,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 87, EccCodewords = 22 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 88, EccCodewords = 22 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 41, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 42, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 12, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 13, EccCodewords = 24 }
                ]
            }
        },
        new()
        {
            Version = 16,
            Size = 81,
            TotalCodewords = 733,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 98, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 99, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 7, DataCodewords = 45, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 3, DataCodewords = 46, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 15, DataCodewords = 19, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 20, EccCodewords = 24 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 13, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 17,
            Size = 85,
            TotalCodewords = 815,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 1, DataCodewords = 107, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 108, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 10, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 1, DataCodewords = 22, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 15, DataCodewords = 23, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 14, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 17, DataCodewords = 15, EccCodewords = 28 }
                ]
            }
        },
        new()
        {
            Version = 18,
            Size = 89,
            TotalCodewords = 901,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 120, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 121, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 9, DataCodewords = 43, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 44, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 22, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 23, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 14, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 19, DataCodewords = 15, EccCodewords = 28 }
                ]
            }
        },
        new()
        {
            Version = 19,
            Size = 93,
            TotalCodewords = 991,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 113, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 114, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 44, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 11, DataCodewords = 45, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 21, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 22, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 9, DataCodewords = 13, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 16, DataCodewords = 14, EccCodewords = 26 }
                ]
            }
        },
        new()
        {
            Version = 20,
            Size = 97,
            TotalCodewords = 1085,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 107, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 108, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 46, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 47, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 22, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 23, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 15, DataCodewords = 13, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 14, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 21,
            Size = 101,
            TotalCodewords = 1156,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 116, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 117, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 14, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 14, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 14, DataCodewords = 12, EccCodewords = 24 },
                    new QREccInfo { BlockCount = 16, DataCodewords = 13, EccCodewords = 24 }
                ]
            }
        },
        new()
        {
            Version = 22,
            Size = 105,
            TotalCodewords = 1258,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 111, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 112, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 7, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 22, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 22, DataCodewords = 13, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 13, DataCodewords = 14, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 23,
            Size = 109,
            TotalCodewords = 1364,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 121, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 5, DataCodewords = 122, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 18, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 22, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 23, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 23, DataCodewords = 12, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 15, DataCodewords = 13, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 24,
            Size = 113,
            TotalCodewords = 1474,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 117, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 118, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 45, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 19, DataCodewords = 46, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 23, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 26, DataCodewords = 24, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 12, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 13, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 25,
            Size = 117,
            TotalCodewords = 1588,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 106, EccCodewords = 26 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 107, EccCodewords = 26 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 22, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 33, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 12, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 11, DataCodewords = 13, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 26,
            Size = 121,
            TotalCodewords = 1706,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 10, DataCodewords = 114, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 2, DataCodewords = 115, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 19, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 28, DataCodewords = 22, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 23, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 33, DataCodewords = 12, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 13, DataCodewords = 13, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 27,
            Size = 125,
            TotalCodewords = 1828,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 122, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 123, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 22, DataCodewords = 45, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 3, DataCodewords = 46, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 8, DataCodewords = 23, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 26, DataCodewords = 24, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 12, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 28, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 28,
            Size = 129,
            TotalCodewords = 1921,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 117, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 118, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 3, DataCodewords = 45, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 23, DataCodewords = 46, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 31, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 31, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 29,
            Size = 133,
            TotalCodewords = 2051,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 7, DataCodewords = 116, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 117, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 21, DataCodewords = 45, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 46, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 1, DataCodewords = 23, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 37, DataCodewords = 24, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 19, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 26, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 30,
            Size = 137,
            TotalCodewords = 2185,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 5, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 19, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 15, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 25, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 23, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 25, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 31,
            Size = 141,
            TotalCodewords = 2323,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 13, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 3, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 29, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 42, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 23, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 28, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 32,
            Size = 145,
            TotalCodewords = 2465,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 10, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 23, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 10, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 35, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 19, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 35, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 33,
            Size = 149,
            TotalCodewords = 2611,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 14, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 21, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 29, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 19, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 11, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 46, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 34,
            Size = 153,
            TotalCodewords = 2761,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 13, DataCodewords = 115, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 8, DataCodewords = 116, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 14, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 23, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 44, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 59, DataCodewords = 16, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 17, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 35,
            Size = 157,
            TotalCodewords = 2876,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 12, DataCodewords = 121, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 12, DataCodewords = 122, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 12, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 26, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 39, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 14, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 22, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 41, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 36,
            Size = 161,
            TotalCodewords = 3034,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 121, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 22, DataCodewords = 122, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 6, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 34, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 46, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 2, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 64, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 37,
            Size = 165,
            TotalCodewords = 3196,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 122, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 4, DataCodewords = 123, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 29, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 14, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 49, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 24, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 46, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 38,
            Size = 169,
            TotalCodewords = 3362,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 17, DataCodewords = 122, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 7, DataCodewords = 123, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 14, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 46, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 49, DataCodewords = 16, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 10, DataCodewords = 17, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 39,
            Size = 173,
            TotalCodewords = 3532,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 4, DataCodewords = 122, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 18, DataCodewords = 123, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 46, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 16, DataCodewords = 47, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 34, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 59, DataCodewords = 16, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 1, DataCodewords = 17, EccCodewords = 30 }
                ]
            }
        },
        new()
        {
            Version = 40,
            Size = 177,
            TotalCodewords = 3706,
            Ecc = new(EqualityComparer<QRErrorCorrectionLevel>.Default)
            {
                [QRErrorCorrectionLevel.Low] = [
                    new QREccInfo { BlockCount = 19, DataCodewords = 117, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 6, DataCodewords = 118, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.Medium] = [
                    new QREccInfo { BlockCount = 30, DataCodewords = 47, EccCodewords = 28 },
                    new QREccInfo { BlockCount = 22, DataCodewords = 48, EccCodewords = 28 }
                ],
                [QRErrorCorrectionLevel.Quartile] = [
                    new QREccInfo { BlockCount = 34, DataCodewords = 24, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 34, DataCodewords = 25, EccCodewords = 30 }
                ],
                [QRErrorCorrectionLevel.High] = [
                    new QREccInfo { BlockCount = 20, DataCodewords = 15, EccCodewords = 30 },
                    new QREccInfo { BlockCount = 61, DataCodewords = 16, EccCodewords = 30 }
                ]
            }
        }
    ];
}
