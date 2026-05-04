namespace FluentUI.Blazor.Community.Components.Helpers;

/// <summary>
/// Provides helper methods for creating and restoring two-dimensional Boolean matrices.
/// </summary>
internal class MatrixHelper
{
    /// <summary>
    /// Creates a new two-dimensional Boolean array that is a copy of the specified square matrix.
    /// </summary>
    /// <remarks>The returned array is a deep copy of the input matrix. Changes to the returned array do not
    /// affect the original matrix.</remarks>
    /// <param name="matrix">The source two-dimensional Boolean array to clone. Must be a square matrix with dimensions equal to <paramref
    /// name="size"/>.</param>
    /// <param name="size">The number of rows and columns in the square matrix to clone.</param>
    /// <returns>A new two-dimensional Boolean array containing the same values as the specified matrix.</returns>
    internal static bool[,] CloneMatrix(bool[,] matrix, int size)
    {
        var clone = new bool[size, size];
        Array.Copy(matrix, clone, matrix.Length);

        return clone;
    }

    /// <summary>
    /// Restores the values of the internal matrix from the specified source array.
    /// </summary>
    /// <param name="src">A two-dimensional array of nullable Boolean values containing the matrix data to restore. The array must have
    /// the same dimensions as the internal matrix.</param>
    /// <param name="dest">The internal matrix to be updated with the values from the source array.</param>
    internal static void RestoreMatrix(bool[,] dest, bool[,] src)
    {
        Array.Copy(src, dest, src.Length);
    }

    /// <summary>
    /// Optimizes a two-dimensional boolean matrix into a minimal set of barcode rectangles representing contiguous true
    /// regions.
    /// </summary>
    /// <remarks>This method first groups adjacent true values horizontally, then merges vertically stacked
    /// rectangles to minimize the total number of rectangles. The result can be used for efficient barcode rendering or
    /// processing.</remarks>
    /// <param name="matrix">A two-dimensional boolean matrix where each element indicates whether the corresponding cell is part of a
    /// barcode region to be optimized.</param>
    /// <returns>A list of BarcodeRectangle objects, each representing a contiguous rectangular region of true values in the
    /// input matrix. The list will be empty if no such regions are found.</returns>
    public static List<BarcodeRectangle> Optimize(bool[,] matrix)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);

        // Horizontal merging : create rectangles for contiguous true values in each row.
        var horizontal = BuildHorizontalRects(matrix, rows, cols);

        // Vertical merging : merge rectangles that are vertically adjacent and have the same X and Width.
        var merged = MergeVerticalRects(horizontal);

        return merged;
    }

    /// <summary>
    /// Identifies and constructs horizontal rectangles representing contiguous sequences of set cells in each row of
    /// the specified matrix.
    /// </summary>
    /// <remarks>Each rectangle in the result corresponds to a contiguous sequence of <see langword="true"/>
    /// values in a single row, with a height of 1. This method is typically used in barcode rendering or similar
    /// scenarios where horizontal runs of set cells need to be identified.</remarks>
    /// <param name="matrix">A two-dimensional Boolean array representing the matrix to scan, where a value of <see langword="true"/>
    /// indicates a set cell.</param>
    /// <param name="rows">The number of rows in the matrix to process.</param>
    /// <param name="cols">The number of columns in the matrix to process.</param>
    /// <returns>A list of <see cref="BarcodeRectangle"/> objects, each representing a horizontal run of set cells in the matrix.
    /// The list is empty if no such runs are found.</returns>
    private static List<BarcodeRectangle> BuildHorizontalRects(
        bool[,] matrix,
        int rows,
        int cols)
    {
        var rects = new List<BarcodeRectangle>();

        for (var y = 0; y < rows; y++)
        {
            var x = 0;

            while (x < cols)
            {
                if (!matrix[y, x])
                {
                    x++;
                    continue;
                }

                var start = x;

                while (x < cols && matrix[y, x])
                {
                    x++;
                }

                rects.Add(new BarcodeRectangle(start, y, x - start, 1));
            }
        }

        return rects;
    }

    /// <summary>
    /// Merges vertically adjacent rectangles from the specified list when they share the same X coordinate and width.
    /// </summary>
    /// <remarks>Rectangles are merged only if they are directly adjacent vertically, have the same X
    /// coordinate, and the same width. The input list is sorted to ensure correct merging order. The original list is
    /// not modified.</remarks>
    /// <param name="rects">The list of rectangles to merge. Each rectangle is evaluated for possible vertical merging with others in the
    /// list.</param>
    /// <returns>A new list of rectangles where vertically adjacent rectangles with matching X coordinate and width have been
    /// merged into single rectangles.</returns>
    private static List<BarcodeRectangle> MergeVerticalRects(List<BarcodeRectangle> rects)
    {
        // Tri pour faciliter la fusion
        rects.Sort((a, b) =>
        {
            var cmp = a.X.CompareTo(b.X);

            if (cmp != 0)
            {
                return cmp;
            }

            return a.Y.CompareTo(b.Y);
        });

        var merged = new List<BarcodeRectangle>();

        foreach (var rect in rects)
        {
            // Search a merged rectangle that :
            // - has the same X coordinate.
            // - has the same width.
            // - is just above (Y + Height == rect.Y)
            var last = merged.LastOrDefault(m =>
                m.X == rect.X &&
                m.Width == rect.Width &&
                m.Y + m.Height == rect.Y);

            if (last != null)
            {
                // Merge with the last rectangle.
                last.Height += rect.Height;
            }
            else
            {
                // New rectangle, add to the list.
                merged.Add(new BarcodeRectangle(rect.X, rect.Y, rect.Width, rect.Height));
            }
        }

        return merged;
    }
}
