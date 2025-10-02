namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class GridDisplayModeTests
{
    [Fact]
    public void GridDisplayMode_ShouldContain_None_Lines_And_Dots()
    {
        var values = Enum.GetValues<GridDisplayMode>();

        Assert.Contains(GridDisplayMode.None, values);
        Assert.Contains(GridDisplayMode.Lines, values);
        Assert.Contains(GridDisplayMode.Dots, values);
    }
}
