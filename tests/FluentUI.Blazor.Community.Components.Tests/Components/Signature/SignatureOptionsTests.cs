namespace FluentUI.Blazor.Community.Components.Tests.Components.Signature;

public class SignatureOptionsTests
{
    [Fact]
    public void Constructor_InitializesAllProperties()
    {
        // Act
        var options = new SignatureOptions();

        // Assert
        Assert.NotNull(options.Pen);
        Assert.NotNull(options.Eraser);
        Assert.NotNull(options.Grid);
        Assert.NotNull(options.Watermark);
        Assert.NotNull(options.Export);
    }

    [Fact]
    public void CanSetAndGetPenProperty()
    {
        var options = new SignatureOptions();
        var pen = new SignaturePenOptions();
        options.Pen = pen;
        Assert.Same(pen, options.Pen);
    }

    [Fact]
    public void CanSetAndGetEraserProperty()
    {
        var options = new SignatureOptions();
        var eraser = new SignatureEraserOptions();
        options.Eraser = eraser;
        Assert.Same(eraser, options.Eraser);
    }

    [Fact]
    public void CanSetAndGetGridProperty()
    {
        var options = new SignatureOptions();
        var grid = new SignatureGridOptions();
        options.Grid = grid;
        Assert.Same(grid, options.Grid);
    }

    [Fact]
    public void CanSetAndGetWatermarkProperty()
    {
        var options = new SignatureOptions();
        var watermark = new SignatureWatermarkOptions();
        options.Watermark = watermark;
        Assert.Same(watermark, options.Watermark);
    }

    [Fact]
    public void CanSetAndGetExportProperty()
    {
        var options = new SignatureOptions();
        var export = new SignatureExportOptions();
        options.Export = export;
        Assert.Same(export, options.Export);
    }
}
