namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a renderer that draws selection strokes on a signature surface.
/// </summary>
/// <param name="getSelectedStrokes">A function that returns the collection of currently selected signature strokes to be rendered as selection overlays.</param>
/// <param name="getTool">A function that returns the current signature stroke tool, which may influence the appearance of the selection rendering.</param>
/// <remarks>This renderer is intended for use with signature surfaces that support selection visualization. It
/// implements the ISignatureSurfaceRenderer interface to enable custom rendering of selection overlays.</remarks>
internal sealed class SelectionStrokeRenderer(
    Func<SignatureStrokeTool> getTool,
    Func<IReadOnlyList<SignatureStroke>> getSelectedStrokes) : ISignatureSurfaceRenderer
{
    /// <inheritdoc />
    public void Render(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        if (getTool() != SignatureStrokeTool.Selection)
        {
            return;
        }

        if (!options.Selection.Enabled ||
            !options.Selection.Highlight)
        {
            return;
        }

        var selected = getSelectedStrokes();

        if (selected is null || selected.Count == 0)
        {
            return;
        }

        var opt = options.Selection;
        var payload = PayloadFactory.CreateSelection(opt, selected);

        target.DynamicLayer.SetSelection(payload);
    }

    /// <inheritdoc />
    public ValueTask RenderAsync(
        ISurfaceRenderTarget target,
        SignatureRenderingOptions options)
    {
        Render(target, options);

        return ValueTask.CompletedTask;
    }
}
