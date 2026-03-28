using FluentUI.Blazor.Community.Components.Helpers;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers;

public class MatrixHelperTests
{
    [Fact]
    public void CloneMatrix_CreatesDeepCopy()
    {
        var matrix = new bool[2, 2];
        matrix[0, 0] = true;

        var clone = MatrixHelper.CloneMatrix(matrix, 2);
        matrix[0, 0] = false;

        Assert.True(clone[0, 0]);
        Assert.False(matrix[0, 0]);
    }

    [Fact]
    public void RestoreMatrix_CopiesValues()
    {
        var source = new bool[1, 2];
        source[0, 1] = true;
        var dest = new bool[1, 2];

        MatrixHelper.RestoreMatrix(dest, source);

        Assert.True(dest[0, 1]);
    }

    [Fact]
    public void Optimize_MergesContiguousRegions()
    {
        var matrix = new bool[2, 3]
        {
            { true, true, false },
            { true, true, false }
        };

        var rects = MatrixHelper.Optimize(matrix);

        Assert.Single(rects);
        Assert.Equal(0, rects[0].X);
        Assert.Equal(0, rects[0].Y);
        Assert.Equal(2, rects[0].Width);
        Assert.Equal(2, rects[0].Height);
    }
}
