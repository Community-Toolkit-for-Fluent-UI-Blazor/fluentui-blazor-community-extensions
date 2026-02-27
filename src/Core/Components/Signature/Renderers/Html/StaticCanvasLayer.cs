namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a sealed implementation of a static canvas rendering layer that manages and flushes drawing payloads to a
/// JavaScript canvas surface using interop.
/// </summary>
/// <remarks>This class is intended for internal use to coordinate the rendering of static visual elements such as
/// background, grid, axes, and watermark on a canvas element. It encapsulates the payload state and communicates with
/// JavaScript via the provided module and method. Thread safety is not guaranteed; concurrent access should be managed
/// externally if required.</remarks>
internal sealed class StaticCanvasLayer : IStaticSurfaceRender
{
    /// <summary>
    /// Represents the static frame payload associated with this instance.
    /// </summary>
    private readonly StaticFramePayload _frame = new();

    /// <summary>
    /// Holds the last known hover payload.
    /// </summary>
    private HoverPayload? _lastHover;

    /// <summary>
    /// Holds the last known background payload for this layer.
    /// </summary>
    private BackgroundPayload? _lastBackground;

    /// <summary>
    /// Holds the last known grid payload for this layer.
    /// </summary>
    private GridPayload? _lastGrid;

    /// <summary>
    /// Holds the last known axes payload for this layer.
    /// </summary>
    private AxesPayload? _lastAxes;

    /// <summary>
    /// Holds the last known watermark payload for this layer.
    /// </summary>
    private WatermarkPayload? _lastWatermark;

    /// <inheritdoc/>
    public StaticFramePayload Frame => _frame;

    /// <inheritdoc />
    public void SetView(ViewPayload view) => _frame.View = view;

    /// <inheritdoc />
    public void SetBackground(BackgroundPayload? background)
    {
        if (PayloadComparer.AreEqual(_lastBackground, background))
        {
            return;
        }

        _lastBackground = background;
        _frame.Background = background;
        _frame.Dirty.Background = true;
    }

    /// <inheritdoc />
    public void SetGrid(GridPayload? grid)
    {
        if (PayloadComparer.AreEqual(_lastGrid, grid))
        {
            return;
        }

        _lastGrid = grid;
        _frame.Grid = grid;
        _frame.Dirty.Grid = true;
    }

    /// <inheritdoc />
    public void SetAxes(AxesPayload? axes)
    {
        if (PayloadComparer.AreEqual(_lastAxes, axes))
        {
            return;
        }

        _lastAxes = axes;
        _frame.Axes = axes;
        _frame.Dirty.Axes = true;
    }

    /// <inheritdoc />
    public void SetWatermark(WatermarkPayload? watermark)
    {
        if (PayloadComparer.AreEqual(_lastWatermark, watermark))
        {
            return;
        }

        _lastWatermark = watermark;
        _frame.Watermark = watermark;
        _frame.Dirty.Watermark = true;
    }

    /// <inheritdoc />
    public void SetHover(HoverPayload? payload)
    {
        if (PayloadComparer.AreEqual(_lastHover, payload))
        {
            return;
        }

        _lastHover = payload;
        _frame.Hover = payload;
        _frame.Dirty.Hover = true;
    }
}

