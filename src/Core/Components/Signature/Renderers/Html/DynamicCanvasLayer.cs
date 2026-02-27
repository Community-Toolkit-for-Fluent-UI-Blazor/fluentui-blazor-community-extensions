namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dynamic rendering layer for a canvas element that manages and synchronizes drawing state with a
/// JavaScript interop module.
/// </summary>
/// <remarks>This class is intended for internal use to facilitate dynamic updates to a canvas surface in Blazor
/// applications using JavaScript interop. It encapsulates the state required for rendering and provides methods to
/// update various aspects of the drawing payload before flushing changes to the associated JavaScript method.</remarks>
internal sealed class DynamicCanvasLayer : IDynamicSurfaceRender
{
    /// <summary>
    /// Stores the payload data for the dynamic frame associated with this instance.
    /// </summary>
    private readonly DynamicFramePayload _frame = new();

    /// <summary>
    /// Holds the last known stroke layer payload.
    /// </summary>
    private StrokeLayerPayload? _lastStrokeLayer;

    /// <summary>
    /// Holds the last known dynamic stroke payload.
    /// </summary>
    private DynamicStrokePayload? _lastDynamicStroke;

    /// <summary>
    /// Holds the last known selection payload.
    /// </summary>
    private SelectionPayload? _lastSelection;

    /// <summary>
    /// Holds the last known debug payload.
    /// </summary>
    private DebugPayload? _lastDebug;

    /// <inheritdoc/>
    public DynamicFramePayload Frame => _frame;

    /// <inheritdoc />
    public void SetView(ViewPayload view) => _frame.View = view;

    /// <inheritdoc />
    public void SetStrokeLayer(StrokeLayerPayload? strokeLayer)
    {
        if (PayloadComparer.AreEqual(_lastStrokeLayer, strokeLayer))
        {
            return;
        }

        _lastStrokeLayer = strokeLayer;
        _frame.StrokeLayer = strokeLayer;
        _frame.Dirty.StrokeLayer = true;
    }

    /// <inheritdoc />
    public void SetDynamicStroke(DynamicStrokePayload? dynamicStroke)
    {
        if (PayloadComparer.AreEqual(_lastDynamicStroke, dynamicStroke))
        {
            return;
        }

        _lastDynamicStroke = dynamicStroke;
        _frame.DynamicStroke = dynamicStroke;
        _frame.Dirty.DynamicStroke = true;
    }

    /// <inheritdoc />
    public void SetSelection(SelectionPayload? selection)
    {
        if (PayloadComparer.AreEqual(_lastSelection, selection))
        {
            return;
        }

        _lastSelection = selection;
        _frame.Selection = selection;
        _frame.Dirty.Selection = true;
    }

    /// <inheritdoc />
    public void SetDebug(DebugPayload? debug)
    {
        if (PayloadComparer.AreEqual(_lastDebug, debug))
        {
            return;
        }

        _lastDebug = debug;
        _frame.Debug = debug;
        _frame.Dirty.Debug = true;
    }
}
