namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines methods for configuring and managing dynamic rendering surfaces, including setting debug information, stroke
/// parameters, selection state, and stroke layers, as well as flushing buffered data asynchronously.
/// </summary>
/// <remarks>Implementations of this interface enable dynamic updates to rendering surfaces, supporting scenarios
/// such as real-time drawing, diagnostics, and selection management. Methods allow for granular control over rendering
/// state and provide asynchronous flushing to ensure data consistency with underlying storage or output
/// targets.</remarks>
public interface IDynamicSurfaceRender
{
    /// <summary>
    /// Gets the payload associated with the current dynamic frame.
    /// </summary>
    DynamicFramePayload Frame { get; }

    /// <summary>
    /// Sets the debug payload used for diagnostic or troubleshooting purposes.
    /// </summary>
    /// <param name="debug">The debug payload to apply. Specify a value to enable debugging features, or null to disable debugging.</param>
    void SetDebug(DebugPayload? debug);

    /// <summary>
    /// Sets the dynamic stroke configuration using the specified payload.
    /// </summary>
    /// <param name="dynamicStroke">The payload containing stroke parameters to apply. Cannot be null.</param>
    void SetDynamicStroke(DynamicStrokePayload dynamicStroke);

    /// <summary>
    /// Sets the current selection state using the specified selection payload.
    /// </summary>
    /// <param name="selection">The selection payload to apply. If null, the selection will be cleared.</param>
    void SetSelection(SelectionPayload? selection);

    /// <summary>
    /// Sets the stroke layer configuration for the current context.
    /// </summary>
    /// <param name="strokeLayer">The stroke layer payload to apply. If null, the stroke layer will be cleared or reset to its default state.</param>
    void SetStrokeLayer(StrokeLayerPayload? strokeLayer);
}
