using FluentUI.Blazor.Community.Components.Helpers.Pdf417;
using Xunit;

namespace Components.Tests.Components.Barcodes.Helpers.Pdf417;

public class Pdf417ColumnOptimizerTests
{
    [Fact]
    public void ChooseColumns_ClampsToValidRange()
    {
        Assert.Equal(2, Pdf417ColumnOptimizer.ChooseColumns(1));
        Assert.Equal(30, Pdf417ColumnOptimizer.ChooseColumns(5000));
    }

    [Fact]
    public void ChooseRows_ComputesCeiling()
    {
        Assert.Equal(3, Pdf417ColumnOptimizer.ChooseRows(10, 4));
    }
}
