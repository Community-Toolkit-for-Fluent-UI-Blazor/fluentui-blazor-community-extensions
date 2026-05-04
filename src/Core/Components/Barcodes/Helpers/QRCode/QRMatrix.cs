using System.Diagnostics;
using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Helpers;

internal sealed class QRMatrix
{
    private readonly bool[,] _isData;
    private readonly bool[,] _isReserved;

    public bool[,] Matrix { get; }

    public int Size { get; }

    /// <summary>
    /// Provides a lookup table that maps version numbers to alignment pattern positions for QR code generation.
    /// </summary>
    /// <remarks>The dictionary keys represent QR code version numbers, and the values are arrays of integers
    /// indicating the positions of alignment patterns for each version. This table is typically used when constructing
    /// QR code matrices to determine where alignment patterns should be placed.</remarks>
    private static readonly Dictionary<int, int[]> AlignmentTable = new()
 {
     { 1, [] },
     { 2, new[]{ 6, 18 } },
     { 3, new[]{ 6, 22 } },
     { 4, new[]{ 6, 26 } },
     { 5, new[]{ 6, 30 } },
     { 6, new[]{ 6, 34 } },
     { 7, new[]{ 6, 22, 38 } },
     { 8, new[]{ 6, 24, 42 } },
     { 9, new[]{ 6, 26, 46 } },
     { 10, new[]{ 6, 28, 50 } },
     { 11, new[]{ 6, 30, 54 } },
     { 12, new[]{ 6, 32, 58 } },
     { 13, new[]{ 6, 34, 62 } },
     { 14, new[]{ 6, 26, 46, 66 } },
     { 15, new[]{ 6, 26, 48, 70 } },
     { 16, new[]{ 6, 26, 50, 74 } },
     { 17, new[]{ 6, 30, 54, 78 } },
     { 18, new[]{ 6, 30, 56, 82 } },
     { 19, new[]{ 6, 30, 58, 86 } },
     { 20, new[]{ 6, 34, 62, 90 } },
     { 21, new[]{ 6, 28, 50, 72, 94 } },
     { 22, new[]{ 6, 26, 50, 74, 98 } },
     { 23, new[]{ 6, 30, 54, 78, 102 } },
     { 24, new[]{ 6, 28, 54, 80, 106 } },
     { 25, new[]{ 6, 32, 58, 84, 110 } },
     { 26, new[]{ 6, 30, 58, 86, 114 } },
     { 27, new[]{ 6, 34, 62, 90, 118 } },
     { 28, new[]{ 6, 26, 50, 74, 98, 122 } },
     { 29, new[]{ 6, 30, 54, 78, 102, 126 } },
     { 30, new[]{ 6, 26, 52, 78, 104, 130 } },
     { 31, new[]{ 6, 30, 56, 82, 108, 134 } },
     { 32, new[]{ 6, 34, 60, 86, 112, 138 } },
     { 33, new[]{ 6, 30, 58, 86, 114, 142 } },
     { 34, new[]{ 6, 34, 62, 90, 118, 146 } },
     { 35, new[]{ 6, 30, 54, 78, 102, 126, 150 } },
     { 36, new[]{ 6, 24, 50, 76, 102, 128, 154 } },
     { 37, new[]{ 6, 28, 54, 80, 106, 132, 158 } },
     { 38, new[]{ 6, 32, 58, 84, 110, 136, 162 } },
     { 39, new[]{ 6, 26, 54, 82, 110, 138, 166 } },
     { 40, new[]{ 6, 30, 58, 86, 114, 142, 170 } },
 };

    public QRMatrix(
        QRVersion version,
        byte[] codewords,
        QRErrorCorrectionLevel ecc)
    {
        var v = (int)version;
        Size = 21 + 4 * (v - 1);
        Matrix = new bool[Size, Size];
        _isData = new bool[Size, Size];
        _isReserved = new bool[Size, Size];

        PlaceTimingPatterns();
        PlaceFinderPatterns();
        PlaceAlignmentPatterns(version);
        PlaceDarkModule(version);
        ReserveFormatInfo();
        ReserveVersionInfo(version);
        WriteVersionInfo(version);
        WriteFormatInfo(0, ecc);
        PlaceData(codewords ?? []);
        var mask = ApplyBestMask(version, ecc);
        WriteFormatInfo(mask, ecc);
    }

    /// <summary>
    /// Places the timing patterns in the QR code matrix to enable accurate module coordinate determination during
    /// scanning.
    /// </summary>
    /// <remarks>Timing patterns are required for QR code decoding and are positioned along the sixth row and
    /// column, excluding the finder pattern areas. These patterns help scanners identify the grid structure and module
    /// alignment.</remarks>
    private void PlaceTimingPatterns()
    {
        for (var i = 0; i < Size; i++)
        {
            Matrix[6, i] = (i % 2 == 0);
            Matrix[i, 6] = (i % 2 == 0);
            _isReserved[6, i] = true;
            _isReserved[i, 6] = true;
        }
    }

    /// <summary>
    /// Places the standard finder patterns at their required positions within the matrix.
    /// </summary>
    /// <remarks>This method positions finder patterns in the top-left, top-right, and bottom-left corners, as
    /// specified by the QR code standard. These patterns are essential for QR code detection and alignment during
    /// scanning.</remarks>
    private void PlaceFinderPatterns()
    {
        PlaceFinder(0, 0);
        PlaceFinder(0, Size - 7);
        PlaceFinder(Size - 7, 0);
    }

    /// <summary>
    /// Marks a 7x7 region in the matrix as a finder pattern, setting the appropriate cells to represent the pattern at
    /// the specified starting position.
    /// </summary>
    /// <remarks>This method also sets a one-cell-wide separator around the finder pattern to distinguish it
    /// from adjacent regions. The method assumes that the specified position and the surrounding area are within the
    /// bounds of the matrix.</remarks>
    /// <param name="row">The zero-based row index of the top-left corner where the finder pattern should be placed.</param>
    /// <param name="col">The zero-based column index of the top-left corner where the finder pattern should be placed.</param>
    private void PlaceFinder(int row, int col)
    {
        for (var r = 0; r < 7; r++)
        {
            for (var c = 0; c < 7; c++)
            {
                var isBorder = r == 0 || r == 6 || c == 0 || c == 6;
                var isCenter = r >= 2 && r <= 4 && c >= 2 && c <= 4;

                Matrix[row + r, col + c] = isBorder || isCenter;
                _isReserved[row + r, col + c] = true;
            }
        }

        for (var i = -1; i <= 7; i++)
        {
            SetIfInBounds(row - 1, col + i, false);
            SetIfInBounds(row + 7, col + i, false);
            SetIfInBounds(row + i, col - 1, false);
            SetIfInBounds(row + i, col + 7, false);
        }
    }

    /// <summary>
    /// Sets the specified value in the matrix at the given row and column indices if both indices are within the valid
    /// bounds.
    /// </summary>
    /// <remarks>If either the row or column index is out of bounds, the method does not modify the
    /// matrix.</remarks>
    /// <param name="r">The zero-based row index at which to set the value. Must be greater than or equal to 0 and less than the matrix
    /// size.</param>
    /// <param name="c">The zero-based column index at which to set the value. Must be greater than or equal to 0 and less than the
    /// matrix size.</param>
    /// <param name="value">The Boolean value to assign to the specified matrix cell.</param>
    private void SetIfInBounds(int r, int c, bool value)
    {
        if (r >= 0 &&
            r < Size &&
            c >= 0 &&
            c < Size)
        {
            Matrix[r, c] = value;
            _isReserved[r, c] = true;
        }
    }

    /// <summary>
    /// Places alignment patterns on the QR code matrix for the specified QR version.
    /// </summary>
    /// <remarks>Alignment patterns are used in QR codes to improve decoding accuracy, especially for larger
    /// versions. This method skips positions that overlap with finder patterns to ensure correct placement.</remarks>
    /// <param name="version">The QR code version that determines the alignment pattern positions to be placed.</param>
    private void PlaceAlignmentPatterns(QRVersion version)
    {
        var positions = AlignmentTable[(int)version];

        if (positions.Length == 0)
        {
            return;
        }

        foreach (var r in positions)
        {
            foreach (var c in positions)
            {
                if (IsInFinderZone(r, c))
                {
                    continue;
                }

                PlaceAlignment(r - 2, c - 2);
            }
        }
    }

    /// <summary>
    /// Determines whether the specified row and column indices fall within the finder pattern zones of the QR code matrix.
    /// </summary>
    /// <param name="r">Row index to check. Must be a non-negative integer less than the matrix size.</param>
    /// <param name="c">Column index to check. Must be a non-negative integer less than the matrix size.</param>
    /// <returns>Returns <see langword="true" /> if the specified indices are within any of the three finder pattern zones; otherwise, returns <see langword="false"/>.</returns>
    private bool IsInFinderZone(int r, int c)
    {
        return (r <= 8 && c <= 8) ||
               (r <= 8 && c >= Size - 9) ||
               (r >= Size - 9 && c <= 8);
    }

    /// <summary>
    /// Places an alignment pattern in a 5x5 region of the matrix at the specified row and column coordinates.
    /// </summary>
    /// <remarks>The alignment pattern consists of a 5x5 square with the border and center cells set to true.
    /// Ensure that the specified coordinates allow the entire pattern to fit within the bounds of the matrix.</remarks>
    /// <param name="row">The zero-based row index in the matrix where the top edge of the alignment pattern will be placed.</param>
    /// <param name="col">The zero-based column index in the matrix where the left edge of the alignment pattern will be placed.</param>
    private void PlaceAlignment(int row, int col)
    {
        for (var r = 0; r < 5; r++)
        {
            for (var c = 0; c < 5; c++)
            {
                var isBorder = r == 0 || r == 4 || c == 0 || c == 4;
                var isCenter = r == 2 && c == 2;

                Matrix[row + r, col + c] = isBorder || isCenter;
                _isReserved[row + r, col + c] = true;
            }
        }
    }

    /// <summary>
    /// Places the dark module in the QR code matrix according to the specified QR version.
    /// </summary>
    /// <remarks>The dark module is a fixed black module required by the QR code specification and is placed
    /// at a version-dependent location. This method should be called during the QR code matrix construction
    /// process.</remarks>
    /// <param name="version">The QR code version that determines the position of the dark module.</param>
    private void PlaceDarkModule(QRVersion version)
    {
        var v = (int)version;

        Matrix[4 * v + 9, 8] = true;
        _isReserved[4 * v + 9, 8] = true;
    }

    /// <summary>
    /// Reserves the matrix regions used for format information in a QR code symbol.
    /// </summary>
    /// <remarks>This method marks the specific areas of the QR code matrix that are allocated for format
    /// information, ensuring that these regions are not used for data or other patterns. It should be called before
    /// encoding data or applying other patterns to the matrix.</remarks>
    private void ReserveFormatInfo()
    {
        // Top-left horizontal (row 8, col 0..5 and 7..8)
        for (var c = 0; c <= 8; c++)
        {
            if (c == 6)
            {
                continue;
            }

            _isReserved[8, c] = true;
        }

        // Top-left vertical (col 8, row 0..5 and 7..8)
        for (var r = 0; r <= 8; r++)
        {
            if (r == 6)
            {
                continue;
            }

            _isReserved[r, 8] = true;
        }

        // Top-right (row 8, col size-8..size-1) → 8 modules
        for (var c = Size - 8; c < Size; c++)
        {
            _isReserved[8, c] = true;
        }

        // Bottom-left (col 8, row size-7..size-1) → 7 modules
        for (var r = Size - 7; r < Size; r++)
        {
            _isReserved[r, 8] = true;
        }
    }

    /// <summary>
    /// Reserves the areas of the QR code matrix used for version information for versions 7 and above.
    /// </summary>
    /// <remarks>This method marks the specific regions in the QR code matrix that are allocated for version
    /// information, as required by the QR code specification for versions 7 and higher. For versions below 7, no
    /// version information areas are reserved.</remarks>
    /// <param name="version">The QR code version for which to reserve version information areas. Must be 7 or greater to have version
    /// information reserved.</param>
    private void ReserveVersionInfo(QRVersion version)
    {
        var v = (int)version;

        if (v < 7)
        {
            return;
        }

        // Vertical : columns size-11..size-9, rows 0..5
        for (var c = Size - 11; c <= Size - 9; c++)
        {
            for (var r = 0; r <= 5; r++)
            {
                _isReserved[r, c] = true;
            }
        }

        // Horizontal : rows ! size-11..size-9, columns : 0..5
        for (var r = Size - 11; r <= Size - 9; r++)
        {
            for (var c = 0; c <= 5; c++)
            {
                _isReserved[r, c] = true;
            }
        }
    }

    /// <summary>
    /// Writes the version information pattern to the QR code matrix for versions 7 and above.
    /// </summary>
    /// <remarks>This method modifies the QR code matrix to include the version information pattern, as
    /// required by the QR code specification for versions 7 and higher. For versions below 7, no version information is
    /// written.</remarks>
    /// <param name="version">The QR code version for which to write the version information. Must be 7 or greater to apply the version
    /// pattern.</param>
    private void WriteVersionInfo(QRVersion version)
    {
        var v = (int)version;

        if (v < 7)
        {
            return;
        }

        var bits = BCHVersion(v);

        for (var i = 0; i < 18; i++)
        {
            var bit = ((bits >> i) & 1) != 0;
            var a = Size - 11 + (i % 3);
            var b = i / 3;

            Matrix[a, b] = bit;
            _isReserved[a, b] = true;

            Matrix[b, a] = bit;
            _isReserved[b, a] = true;
        }
    }

    /// <summary>
    /// Calculates the 12-bit Bose–Chaudhuri–Hocquenghem (BCH) error correction code for a given QR code version number.
    /// </summary>
    /// <remarks>This method is typically used in QR code generation to encode version information with error
    /// correction. The result can be used to construct the version information pattern in QR codes of version 7 or
    /// higher.</remarks>
    /// <param name="version">The QR code version number for which to compute the BCH error correction code. Must be a non-negative integer.</param>
    /// <returns>A 12-bit integer representing the BCH error correction code for the specified version number.</returns>
    private static int BCHVersion(int version)
    {
        var rem = version;

        for (var i = 0; i < 12; i++)
        {
            if (((rem >> 11) & 1) == 1)
            {
                rem = (rem << 1) ^ 0x1F25;
            }
            else
            {
                rem <<= 1;
            }
        }

        rem &= 0xFFF;

        return (version << 12) | rem;
    }

    /// <summary>
    /// Writes the QR code format information bits to the appropriate locations in the matrix according to the specified
    /// mask pattern and error correction level.
    /// </summary>
    /// <remarks>This method encodes and places the format information bits in the reserved areas of the QR
    /// code matrix as required by the QR code standard. The format information encodes both the error correction level
    /// and the mask pattern used, and is essential for correct decoding by QR code readers.</remarks>
    /// <param name="maskId">The identifier of the mask pattern to apply to the QR code format information. Must be a valid mask pattern
    /// index as defined by the QR code specification.</param>
    /// <param name="ecc">The error correction level to encode in the format information. Determines the level of error resilience for the
    /// QR code.</param>
    /// <param name="scoring">true to force writing the format information regardless of current state; otherwise, false. The default is
    /// false.</param>
    private void WriteFormatInfo(int maskId, QRErrorCorrectionLevel ecc, bool scoring = false)
    {
        var format = FormatInfoBits(maskId, ecc);

        Console.WriteLine("BITS FORMAT : " + format);

        // Draw first copy
        for (var i = 0; i <= 5; i++)
        {
            SetFormatBit(i, 8, ((format >> i) & 1) != 0, scoring);
        }

        SetFormatBit(7, 8, ((format >> 6) & 1) != 0, scoring);
        SetFormatBit(8, 8, ((format >> 7) & 1) != 0, scoring);
        SetFormatBit(8, 7, ((format >> 8) & 1) != 0, scoring);

        for (var i = 9; i < 15; i++)
        {
            SetFormatBit(8, 14 - i, ((format >> i) & 1) != 0, scoring);
        }

        for (var i = 0; i < 8; i++)
        {
            var c = Size - 1 - i;
            SetFormatBit(8, c, ((format >> i) & 1) != 0, scoring);
        }

        for (var i = 8; i < 15; i++)
        {
            var r = Size - 15 + i;
            SetFormatBit(r, 8, ((format >> i) & 1) != 0, scoring);
        }

        SetFormatBit(Size - 8, 8, true, scoring);
    }

    /// <summary>
    /// Sets the specified bit value in the matrix at the given row and column coordinates, unless the coordinates
    /// correspond to reserved positions.
    /// </summary>
    /// <remarks>This method does not modify the matrix if the coordinates correspond to reserved format
    /// information positions, specifically (8, 6) or (6, 8).</remarks>
    /// <param name="r">The zero-based row index in the matrix where the bit value will be set.</param>
    /// <param name="c">The zero-based column index in the matrix where the bit value will be set.</param>
    /// <param name="bit">The bit value to assign at the specified matrix position.</param>
    /// <param name="scoring">The value indicating if we are running in scoring.</param>
    private void SetFormatBit(int r, int c, bool bit, bool scoring)
    {
        if (scoring)
        {
            Matrix[r, c] = bit;
            return;
        }

        if ((r == 8 && c == 6) || (r == 6 && c == 8))
        {
            return;
        }

        Matrix[r, c] = bit;
        _isReserved[r, c] = true;
    }

    /// <summary>
    /// Calculates the 15-bit format information value for a QR code based on the specified mask pattern and error
    /// correction level.
    /// </summary>
    /// <remarks>The returned value includes both the error correction level and mask pattern, encoded
    /// according to the QR code specification, with the required error correction bits and mask applied.</remarks>
    /// <param name="maskId">The identifier of the mask pattern to apply. Must be in the range 0 to 7, inclusive.</param>
    /// <param name="ecc">The error correction level to encode in the format information.</param>
    /// <returns>A 15-bit integer representing the encoded format information for the specified mask pattern and error correction
    /// level.</returns>
    private static int FormatInfoBits(int maskId, QRErrorCorrectionLevel ecc)
    {
        var eccBits = ecc switch
        {
            QRErrorCorrectionLevel.Low => 1,
            QRErrorCorrectionLevel.Medium => 0,
            QRErrorCorrectionLevel.Quartile => 3,
            QRErrorCorrectionLevel.High => 2,
            _ => 0
        };

        var value = (eccBits << 3) | maskId;
        var bch = BCHFormat(value);

        return ((value << 10) | bch) ^ 0x5412;
    }

    /// <summary>
    /// Calculates the 10-bit BCH (Bose–Chaudhuri–Hocquenghem) error correction code for a given 15-bit input value.
    /// </summary>
    /// <remarks>This method is typically used in QR code format information encoding to provide error
    /// detection and correction capabilities. The result can be combined with the original value to form a protected
    /// codeword.</remarks>
    /// <param name="value">The 15-bit integer value for which to compute the BCH error correction code. Only the lower 15 bits are used.</param>
    /// <returns>A 10-bit integer representing the BCH error correction code for the specified input value.</returns>
    private static int BCHFormat(int value)
    {
        var rem = value;

        for (var i = 0; i < 10; i++)
        {
            if (((rem >> 9) & 1) != 0)
            {
                rem = (rem << 1) ^ 0x537;
            }
            else
            {
                rem <<= 1;
            }
        }

        return rem & 0x3FF;
    }

    /// <summary>
    /// Applies the optimal mask pattern to the QR code matrix based on the specified error correction level.
    /// </summary>
    /// <remarks>This method evaluates all possible mask patterns and selects the one that minimizes the
    /// penalty score, which improves the readability and reliability of the generated QR code.</remarks>
    /// <param name="version">The version of the QRCode.</param>
    /// <param name="ecc">The error correction level to use when evaluating and applying mask patterns.</param>
    /// <returns>The index of the mask pattern that results in the lowest penalty score for the QR code matrix.</returns>
    private int ApplyBestMask(QRVersion version, QRErrorCorrectionLevel ecc)
    {
        var bestMask = 0;
        var bestScore = int.MaxValue;

        var backup = MatrixHelper.CloneMatrix(Matrix, Size);
        var backupReserved = MatrixHelper.CloneMatrix(_isReserved, Size);

        for (var mask = 0; mask < 8; mask++)
        {
            MatrixHelper.RestoreMatrix(Matrix, backup);
            MatrixHelper.RestoreMatrix(_isReserved, backupReserved);
            ApplyMask(mask);
            WriteFormatInfo(mask, ecc, true);
            WriteVersionInfo(version);

            var score = EvaluatePenalty();

            Debug.WriteLine($"Mask {mask} has penalty score : {score}");

            if (score < bestScore)
            {
                bestScore = score;
                bestMask = mask;
            }
        }

        MatrixHelper.RestoreMatrix(Matrix, backup);
        MatrixHelper.RestoreMatrix(_isReserved, backupReserved);
        ApplyMask(bestMask);
        WriteFormatInfo(bestMask, ecc);

        return bestMask;
    }

    /// <summary>
    /// Calculates the total penalty score by evaluating all defined penalty rules.
    /// </summary>
    /// <remarks>Each penalty rule contributes to the total score. The method aggregates the results to
    /// provide a comprehensive penalty assessment.</remarks>
    /// <returns>An integer representing the sum of penalties from all applicable rules.</returns>
    private int EvaluatePenalty()
    {
        var result = 0;

        // N1 + N3 (rows + columns)
        result += PenaltyN1N3();

        // N2
        result += PenaltyRule2();

        // N4
        result += PenaltyRule4();

        return result;
    }

    /// <summary>
    /// Calculates the total penalty score for the current matrix based on N1 and N3 evaluation rules.
    /// </summary>
    /// <remarks>This method is typically used in QR code or matrix-based encoding to assess the quality of
    /// the generated matrix by penalizing undesirable patterns. Higher penalty scores indicate a less optimal matrix
    /// according to the standard evaluation rules.</remarks>
    /// <returns>An integer representing the sum of penalties for consecutive runs of the same color (N1) and specific
    /// finder-like patterns (N3) in both rows and columns of the matrix.</returns>
    private int PenaltyN1N3()
    {
        var result = 0;

        // Lignes
        for (var y = 0; y < Size; y++)
        {
            var runColor = false;
            var runLen = 0;
            var history = new int[7];

            for (var x = 0; x < Size; x++)
            {
                var color = Matrix[y, x];

                if (color == runColor)
                {
                    runLen++;

                    if (runLen == 5)
                    {
                        result += 3;      // N1
                    }
                    else if (runLen > 5)
                    {
                        result += 1;
                    }
                }
                else
                {
                    FinderAddHistory(runLen, history);

                    if (!runColor)
                    {
                        result += FinderCountPatterns(history) * 40; // N3
                    }

                    runColor = color;
                    runLen = 1;
                }
            }

            result += FinderTerminateAndCount(runColor, runLen, history) * 40;
        }

        // Columns
        for (var x = 0; x < Size; x++)
        {
            var runColor = false;
            var runLen = 0;
            var history = new int[7];

            for (var y = 0; y < Size; y++)
            {
                var color = Matrix[y, x];

                if (color == runColor)
                {
                    runLen++;

                    if (runLen == 5)
                    {
                        result += 3;
                    }
                    else if (runLen > 5)
                    {
                        result += 1;
                    }
                }
                else
                {
                    FinderAddHistory(runLen, history);

                    if (!runColor)
                    {
                        result += FinderCountPatterns(history) * 40;
                    }

                    runColor = color;
                    runLen = 1;
                }
            }

            result += FinderTerminateAndCount(runColor, runLen, history) * 40;
        }

        return result;
    }

    /// <summary>
    /// Adds a new run length value to the beginning of the history array, shifting existing values and updating the
    /// first element.
    /// </summary>
    /// <remarks>The method modifies the <paramref name="history"/> array in place. The array must have a
    /// length of at least one. If the first element is zero, the method treats this as a special case and increases the
    /// run length value before storing it.</remarks>
    /// <param name="runLen">The run length value to add to the history. If the first element of <paramref name="history"/> is zero, the
    /// length of the array is added to this value before it is stored.</param>
    /// <param name="history">The array that stores the history of run lengths. The new value is inserted at the beginning, and existing
    /// values are shifted one position to the right.</param>
    private static void FinderAddHistory(int runLen, int[] history)
    {
        if (history[0] == 0)
        {
            runLen += history.Length;
        }

        Array.Copy(history, 0, history, 1, history.Length - 1);
        history[0] = runLen;
    }

    /// <summary>
    /// Finalizes the current run, updates the history, and returns the total number of detected patterns.
    /// </summary>
    /// <param name="runColor">A value indicating whether the current run should be terminated and added to the history. If <see
    /// langword="true"/>, the current run is finalized before counting patterns.</param>
    /// <param name="runLen">The length of the current run to be considered for termination and history update. Must be non-negative.</param>
    /// <param name="history">An array representing the history of run lengths. Cannot be null.</param>
    /// <returns>The total number of patterns detected in the updated history.</returns>
    private static int FinderTerminateAndCount(bool runColor, int runLen, int[] history)
    {
        if (runColor)
        {
            FinderAddHistory(runLen, history);
            runLen = 0;
        }

        runLen += history.Length;
        FinderAddHistory(runLen, history);

        return FinderCountPatterns(history);
    }

    /// <summary>
    /// Calculates the number of finder patterns detected in a given array of pattern segment lengths.
    /// </summary>
    /// <remarks>A finder pattern is identified based on specific relationships between the segment lengths in
    /// the array. The method checks for these relationships and counts the number of valid patterns that match the
    /// criteria.</remarks>
    /// <param name="h">An array of integers representing the lengths of consecutive segments in a scanned pattern. The array must
    /// contain at least seven elements, where each element corresponds to a specific segment in the pattern.</param>
    /// <returns>The number of finder patterns found in the input array. Returns 0 if no valid patterns are detected.</returns>
    private static int FinderCountPatterns(int[] h)
    {
        var n = h[1];

        if (n <= 0)
        {
            return 0;
        }

        var core =
            h[2] == n &&
            h[3] == 3 * n &&
            h[4] == n &&
            h[5] == n;

        var count = 0;

        if (core && h[0] >= 4 * n && h[6] >= n)
        {
            count++;
        }

        if (core && h[6] >= 4 * n && h[0] >= n)
        {
            count++;
        }

        return count;
    }

    /// <summary>
    /// Calculates the penalty score for 2x2 blocks of identical values in the matrix, according to penalty rule 2.
    /// </summary>
    /// <remarks>This method is typically used in QR code generation to assess the quality of the matrix by
    /// penalizing areas with uniform 2x2 blocks, which can negatively affect readability.</remarks>
    /// <returns>The total penalty score, where each 2x2 block of identical values increases the score by 3.</returns>
    private int PenaltyRule2()
    {
        var result = 0;

        for (var y = 0; y < Size - 1; y++)
        {
            for (var x = 0; x < Size - 1; x++)
            {
                var c = Matrix[y, x];

                if (Matrix[y, x + 1] == c &&
                    Matrix[y + 1, x] == c &&
                    Matrix[y + 1, x + 1] == c)
                {
                    result += 3; // PENALTY_N2
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Calculates the penalty score for the proportion of dark modules in the matrix according to rule 4 of the QR code
    /// specification.
    /// </summary>
    /// <remarks>This method is typically used during QR code generation to evaluate how closely the matrix's
    /// dark module ratio matches the recommended specification. A lower penalty indicates a better match to the ideal
    /// proportion, which improves QR code readability.</remarks>
    /// <returns>An integer representing the penalty score based on the deviation of dark module proportion from the ideal value.</returns>
    private int PenaltyRule4()
    {
        var dark = 0;
        var total = Size * Size;

        for (var y = 0; y < Size; y++)
        {
            for (var x = 0; x < Size; x++)
            {
                if (Matrix[y, x])
                {
                    dark++;
                }
            }
        }

        var k = Math.Abs(dark * 20 - total * 10);
        k = (k + total - 1) / total - 1;

        return k * 10; // PENALTY_N4
    }

    /// <summary>
    /// Applies the specified mask to the matrix, toggling the state of each applicable element according to the mask
    /// pattern.
    /// </summary>
    /// <remarks>Use this method to modify the matrix by applying a mask pattern, which can be useful for
    /// encoding or obfuscation scenarios where certain elements need to be conditionally toggled. The effect of the
    /// mask depends on the implementation of the IsMasked method and the value of maskId.</remarks>
    /// <param name="maskId">The identifier of the mask to apply. Determines which mask pattern is used to toggle elements in the matrix.</param>
    private void ApplyMask(int maskId)
    {
        for (var r = 0; r < Size; r++)
        {
            for (var c = 0; c < Size; c++)
            {
                if (_isReserved[r, c])
                {
                    continue;
                }

                if (IsMasked(maskId, r, c))
                {
                    Matrix[r, c] = !Matrix[r, c];
                }
            }
        }
    }

    /// <summary>
    /// Determines whether a cell at the specified row and column is masked according to the given mask pattern
    /// identifier.
    /// </summary>
    /// <remarks>Mask patterns are typically used in matrix-based encoding schemes, such as QR codes, to
    /// reduce undesirable patterns and improve readability. Each mask pattern applies a different rule based on the
    /// cell's row and column indices.</remarks>
    /// <param name="maskId">The identifier of the mask pattern to apply. Valid values are 0 through 7, each corresponding to a different
    /// masking rule.</param>
    /// <param name="r">The zero-based row index of the cell to evaluate.</param>
    /// <param name="c">The zero-based column index of the cell to evaluate.</param>
    /// <returns>true if the cell at the specified row and column is masked according to the selected mask pattern; otherwise,
    /// false.</returns>
    private static bool IsMasked(int maskId, int r, int c)
    {
        return maskId switch
        {
            0 => ((r + c) % 2) == 0,
            1 => (r % 2) == 0,
            2 => (c % 3) == 0,
            3 => ((r + c) % 3) == 0,
            4 => (((r / 2) + (c / 3)) % 2) == 0,
            5 => ((r * c) % 2 + (r * c) % 3) == 0,
            6 => (((r * c) % 2 + (r * c) % 3) % 2) == 0,
            7 => (((r + c) % 2 + (r * c) % 3) % 2) == 0,
            _ => false
        };
    }

    /// <summary>
    /// Places the provided codewords into the matrix according to the data placement rules.
    /// </summary>
    /// <remarks>This method arranges the bits of the codewords into the matrix, skipping reserved columns as
    /// necessary. If the codewords array contains fewer bits than required to fill the matrix, the placement stops when
    /// all codewords have been used.</remarks>
    /// <param name="codewords">The sequence of codewords to be placed into the matrix. Each byte represents 8 bits of encoded data. The array
    /// must not be null and should contain enough codewords to fill the matrix as required by the encoding
    /// specification.</param>
    private void PlaceData(byte[] codewords)
    {
        var size = Size;
        var bitIndex = 0;
        var cwIndex = 0;

        for (var right = size - 1; right >= 1; right -= 2)
        {
            if (right == 6)
            {
                right = 5;
            }

            for (var vert = 0; vert < size; vert++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var col = right - j;
                    var upward = ((right + 1) & 2) == 0;
                    var row = upward ? size - 1 - vert : vert;

                    if (_isReserved[row, col])
                    {
                        continue;
                    }

                    var bit = ((codewords[cwIndex] >> (7 - bitIndex)) & 1) != 0;
                    Matrix[row, col] = bit;
                    _isData[row, col] = true;

                    bitIndex++;

                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        cwIndex++;

                        if (cwIndex >= codewords.Length)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }
}
