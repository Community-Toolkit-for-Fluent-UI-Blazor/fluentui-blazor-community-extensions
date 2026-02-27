namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Orchestrates input processing, stroke management, selection, erasing and undo/redo
/// for the signature component.
/// </summary>
public sealed class SignatureEngine : IDisposable
{
    /// <summary>
    /// Provides access to the collection of strokes managed by the current instance.
    /// </summary>
    private readonly StrokeManager _strokes;

    /// <summary>
    /// Provides access to the history management functionality used by the component.
    /// </summary>
    /// <remarks>This field is intended for internal use to track and manage historical state changes. It
    /// should not be accessed directly by consumers of the component.</remarks>
    private readonly HistoryManager _history;

    /// <summary>
    /// Contains the configuration options for the signature engine.
    /// </summary>
    private readonly SignatureEngineOptions _engineOptions;

    /// <summary>
    /// Contains the rendering options used for displaying signatures.
    /// </summary>
    private readonly SignatureRenderingOptions _renderingOptions;

    /// <summary>
    /// Represents the pixel eraser used to perform erase operations on pixel data.
    /// </summary>
    private readonly PixelEraser _pixelEraser;

    /// <summary>
    /// Represents the eraser used to remove strokes from a drawing surface.
    /// </summary>
    private readonly StrokeEraser _strokeEraser;

    /// <summary>
    /// Represents the hybrid eraser instance used for erasing operations.
    /// </summary>
    private readonly HybridEraser _hybridEraser;

    /// <summary>
    /// Provides stroke hit testing functionality for determining whether a point or region intersects with a stroke.
    /// </summary>
    private readonly StrokeHitTester _strokeHitTester;

    /// <summary>
    /// Provides hit testing functionality for rectangles within the component.
    /// </summary>
    /// <remarks>This field is used internally to determine whether a given point or area intersects with
    /// defined rectangles. It is not intended for direct access by consumers of the API.</remarks>
    private readonly RectangleHitTester _rectangleHitTester;

    /// <summary>
    /// Represents the selection manager used to track and manage selection state within the component.
    /// </summary>
    private readonly SelectionManager _selectionManager;

    /// <summary>
    /// Indicates whether a selection operation is currently in progress, along with the starting and ending pointer samples of the selection.
    /// </summary>
    private bool _isSelecting;

    /// <summary>
    /// Represents the pointer sample where the selection operation started, or null if no selection has begun.
    /// </summary>
    private PointerSample? _selectionStart;

    /// <summary>
    /// Represents the pointer sample marking the end of a selection, or null if no selection end is set.
    /// </summary>
    private PointerSample? _selectionEnd;

    /// <summary>
    /// Represents the minimum distance, in pixels, that a pointer must move to initiate a selection drag operation.
    /// </summary>
    /// <remarks>This threshold helps prevent accidental selection drags from minor pointer movements. Adjust
    /// this value to fine-tune the sensitivity of selection drag initiation.</remarks>
    private const double SelectionDragThreshold = 4.0;

    /// <summary>
    /// Represents the rectangular area currently selected, or null if no selection is active.
    /// </summary>
    /// <remarks>This field is typically used to track the user's selection region within a component. The
    /// value is null when no selection has been made.</remarks>
    private RectD? _selectionRect;

    /// <summary>
    /// Represents the input pipeline used for processing or transforming input data within the component.
    /// </summary>
    private InputPipeline _inputPipeline = null!;

    /// <summary>
    /// Represents the collection of stroke processors that are applied to signature strokes during processing.
    /// </summary>
    private readonly List<IStrokeProcessor> _strokeProcessors = [];

    /// <summary>
    /// Initializes a new instance of the SignatureEngine class with the specified engine and rendering options.
    /// </summary>
    /// <param name="engineOptions">Engine  options that define the behavior and features of the signature engine. Cannot be null.</param>
    /// <param name="renderingOptions">Rendering options that specify the visual appearance of the signature strokes. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Occurs when either engineOptions or renderingOptions is null.</exception>
    public SignatureEngine(SignatureEngineOptions engineOptions, SignatureRenderingOptions renderingOptions)
    {
        _engineOptions = engineOptions ?? throw new ArgumentNullException(nameof(engineOptions));
        _renderingOptions = renderingOptions ?? throw new ArgumentNullException(nameof(renderingOptions));

        _strokes = new StrokeManager();
        _history = new HistoryManager(_engineOptions.UndoRedo);

        _history.OnChanged += NotifyChanged;
        _strokes.OnChanged += NotifyChanged;

        InitializePipelines();

        _pixelEraser = new PixelEraser(_strokes, _history, _engineOptions.Eraser);
        _strokeEraser = new StrokeEraser(_strokes, _history, _engineOptions.Eraser);
        _hybridEraser = new HybridEraser(_pixelEraser, _strokeEraser, _strokes, _engineOptions.Eraser);

        _strokeHitTester = new StrokeHitTester(_strokes);
        _rectangleHitTester = new RectangleHitTester(_strokes);
        _selectionManager = new SelectionManager(_strokeHitTester, _rectangleHitTester, _engineOptions.Selection);
        CurrentStyle = BuildStyleFromOptions();
        CurrentTool = SignatureStrokeTool.Pointer;
    }

    /// <summary>
    /// Gets the selection manager that provides access to selection-related operations and state.
    /// </summary>
    /// <remarks>Use this property to interact with the current selection, such as querying selected items or
    /// modifying the selection state. The returned manager exposes methods and properties for managing selection within
    /// the component.</remarks>
    public SelectionManager SelectionManager => _selectionManager;

    /// <summary>
    /// Gets the manager responsible for handling stroke operations.
    /// </summary>
    public StrokeManager StrokeManager => _strokes;

    /// <summary>
    /// Gets the rectangle that defines the current selection area, if any.
    /// </summary>
    public RectD? CurrentSelectionRect => _selectionRect;

    /// <summary>
    /// Gets the current stroke style applied to the signature.
    /// </summary>
    public SignatureStrokeStyle CurrentStyle { get; private set; }

    /// <summary>
    /// Gets the currently selected tool for drawing signature strokes.
    /// </summary>
    public SignatureStrokeTool CurrentTool { get; private set; } = SignatureStrokeTool.Pen;

    /// <summary>
    /// Gets the collection of signature strokes that are currently selected.
    /// </summary>
    public IReadOnlyList<SignatureStroke> SelectedStrokes { get; private set; } = [];

    /// <summary>
    /// Gets the collection of signature strokes currently present in the signature pad.
    /// </summary>
    /// <remarks>The returned collection is read-only and reflects the current state of the signature. Each
    /// stroke represents a continuous line drawn by the user. The collection is empty if no strokes have been
    /// added.</remarks>
    public IReadOnlyList<SignatureStroke> Strokes => _strokes.Strokes;

    /// <summary>
    /// Gets a value indicating whether an undo operation can be performed.
    /// </summary>
    /// <remarks>Use this property to determine if there is a previous state available to revert to before
    /// calling an undo method. This is typically used to enable or disable undo functionality in user
    /// interfaces.</remarks>
    public bool CanUndo => _history.CanUndo;

    /// <summary>
    /// Gets a value indicating whether a redo operation can be performed.
    /// </summary>
    public bool CanRedo => _history.CanRedo;

    /// <summary>
    /// Occurs when the associated value or state changes.
    /// </summary>
    /// <remarks>Subscribe to this event to be notified when the value or state represented by this component
    /// changes. The event handler receives standard event arguments.</remarks>
    public event EventHandler? OnChanged;

    /// <summary>
    /// Occurs when a new stroke segment is added to the current stroke, providing the points and style of the segment.
    /// </summary>
    public event EventHandler<StrokeSegmentEventArgs>? OnStrokeSegmentAdded;

    /// <summary>
    /// Gets the signature stroke that is currently being hovered by the pointer, if any.
    /// </summary>
    public SignatureStroke? HoverStroke { get; private set; }

    /// <summary>
    /// Raises the event to notify listeners that a new stroke segment has been added with the specified points and
    /// style.
    /// </summary>
    /// <param name="p1">The starting point of the stroke segment.</param>
    /// <param name="p2">The ending point of the stroke segment.</param>
    /// <param name="style">The style to apply to the stroke segment.</param>
    private void RaiseStrokeSegment(
        SignaturePoint p1,
        SignaturePoint p2,
        SignatureStrokeStyle style) => OnStrokeSegmentAdded?.Invoke(this, new StrokeSegmentEventArgs(p1, p2, style));

    /// <summary>
    /// Creates a new instance of the SignatureStrokeStyle class based on the current pen and rendering options.
    /// </summary>
    /// <remarks>The returned style reflects both the logical pen settings and the visual rendering options,
    /// ensuring that stroke appearance matches the configured parameters.</remarks>
    /// <returns>A SignatureStrokeStyle object configured with the properties specified in the current options.</returns>
    private SignatureStrokeStyle BuildStyleFromOptions()
    {
        return new SignatureStrokeStyle
        {
            Engine = new SignatureStrokeEngineStyle
            {
                BaseWidth = _engineOptions.Pen.BaseWidth,
                Pressure = _engineOptions.Pressure.Clone(),
                Smoothing = _engineOptions.Smoothing.Clone(),
                Stabilization = _engineOptions.Stabilization.Clone(),
                Interpolation = _engineOptions.Interpolation.Clone(),
                Pen = _engineOptions.Pen.Clone()
            },

            Rendering = new SignatureStrokeRenderingStyle
            {
                Color = _renderingOptions.Pen.Color,
                Opacity = _renderingOptions.Pen.Opacity,
                BlendMode = _renderingOptions.Pen.BlendMode,
                LineCap = _renderingOptions.Pen.LineCap,
                LineJoin = _renderingOptions.Pen.LineJoin,
                DashArray = _renderingOptions.Pen.DashArray,
                Shadow = _renderingOptions.Pen.Shadow.Clone()
            }
        };
    }

    /// <summary>
    /// Raises the change notification event to signal that the state has changed.
    /// </summary>
    /// <param name="sender">The source of the event. This parameter is not used.</param>
    /// <param name="e">An object that contains the event data. This parameter is not used.</param>
    private void NotifyChanged(object? sender, EventArgs e) => NotifyChanged();

    /// <summary>
    /// Raises the change notification event.
    /// </summary>
    private void NotifyChanged()
    {
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Initializes the input and stroke processing pipelines with the required processors.
    /// </summary>
    /// <remarks>Call this method to set up the necessary input and stroke processors before handling input
    /// data. This method should be invoked during component or service initialization to ensure that all processing
    /// pipelines are correctly configured.</remarks>
    private void InitializePipelines()
    {
        _inputPipeline = new InputPipeline(
            new VelocityInputProcessor(),
            new PressureInputProcessor(),
            new PenWidthProcessor(),
            new SmoothingInputProcessor(),
            new StabilizerInputProcessor()
        );

        var surface = _engineOptions.Surface;

        _strokeProcessors.Add(new StrokeClippingProcessor(surface.Width, surface.Height));
    }

    /// <summary>
    /// Reverts the most recent change in the history, if possible.
    /// </summary>
    /// <remarks>Use this method to step back one change in the history. If there are no changes to undo, the
    /// method returns false and no state is modified.</remarks>
    /// <returns>true if the undo operation was successful; otherwise, false.</returns>
    public bool Undo()
    {
        var result = _history.Undo();

        if (result)
        {
            NotifyChanged();
        }

        return result;
    }

    /// <summary>
    /// Attempts to reapply the most recently undone action in the history stack.
    /// </summary>
    /// <remarks>Use this method to restore a previously undone change. If there are no actions available to
    /// redo, the method returns false.</remarks>
    /// <returns>true if the redo operation was successful; otherwise, false.</returns>
    public bool Redo()
    {
        var result = _history.Redo();

        if (result)
        {
            NotifyChanged();
        }

        return result;
    }

    /// <summary>
    /// Clears all entries from the history collection.
    /// </summary>
    public void ClearHistory() => _history.Clear();

    /// <summary>
    /// Selects the pen tool for signature input and applies the specified stroke style if provided.
    /// </summary>
    /// <remarks>Use this method to switch to pen mode for drawing signatures. If a custom style is provided,
    /// it will be cloned and applied; otherwise, the default style is used. Calling this method resets the tool
    /// state.</remarks>
    /// <param name="style">The stroke style to apply to the pen tool. If null, a default style based on engine options is used.</param>
    public void UsePen(SignatureStrokeRenderingStyle? style = null)
    {
        CurrentTool = SignatureStrokeTool.Pen;

        if (style is not null)
        {
            CurrentStyle = BuildStyleFromOptions();
            CurrentStyle.Rendering = style.Clone();
        }
        else
        {
            CurrentStyle = BuildStyleFromOptions();
        }

        ResetToolState();
    }

    /// <summary>
    /// Sets the current tool to the highlighter and applies the specified stroke style, or a default highlighter style
    /// if none is provided.
    /// </summary>
    /// <remarks>The highlighter tool uses a semi-transparent stroke with a multiply blend mode to simulate
    /// typical highlighter behavior. Calling this method resets the tool state and updates the current style.</remarks>
    /// <param name="style">The stroke style to use for the highlighter tool. If null, a default highlighter style is applied.</param>
    public void UseHighlighter(SignatureStrokeRenderingStyle? style = null)
    {
        CurrentTool = SignatureStrokeTool.Highlighter;

        if (style is not null)
        {
            CurrentStyle = BuildStyleFromOptions();
            CurrentStyle.Rendering = style.Clone();
        }
        else
        {
            CurrentStyle = BuildStyleFromOptions();
            CurrentStyle.Rendering.Opacity = 35;
            CurrentStyle.Rendering.BlendMode = StrokeBlendMode.Multiply;
        }

        ResetToolState();
    }

    /// <summary>
    /// Activates the eraser tool for signature stroke editing.
    /// </summary>
    /// <remarks>Calling this method switches the current tool to the eraser, allowing removal of existing
    /// strokes. Any previous tool state is reset to ensure consistent eraser behavior.</remarks>
    public void UseEraser()
    {
        CurrentTool = SignatureStrokeTool.Eraser;

        ResetToolState();
    }

    /// <summary>
    /// Activates the selection tool for signature stroke editing.
    /// </summary>
    /// <remarks>Call this method to switch the current tool to selection mode, enabling manipulation of
    /// existing strokes. This method resets any tool-specific state to ensure consistent behavior when changing
    /// tools.</remarks>
    public void UseSelection()
    {
        CurrentTool = SignatureStrokeTool.Selection;

        ResetToolState();
    }

    /// <summary>
    /// Switches the current tool to the pointer tool, allowing selection or manipulation of existing elements.
    /// </summary>
    public void UsePointer()
    {
        CurrentTool = SignatureStrokeTool.Pointer;

        ResetToolState();
    }

    /// <summary>
    /// Resets the tool state to its default values, clearing any current selection and eraser stroke.
    /// </summary>
    /// <remarks>Call this method to ensure the tool is ready for a new operation, such as starting a new
    /// selection or erasing. This method also notifies listeners of the state change.</remarks>
    private void ResetToolState()
    {
        _isSelecting = false;
        _selectionStart = null;
        _selectionEnd = null;
        SelectedStrokes = [];

        NotifyChanged();
    }

    /// <summary>
    /// Transforms the coordinates of the specified pointer sample from the current viewport to surface coordinates.
    /// </summary>
    /// <remarks>Use this method to convert pointer positions from the viewport's zoomed and panned space back
    /// to the underlying surface space. This is useful when interpreting user input relative to the untransformed
    /// surface.</remarks>
    /// <param name="sample">The pointer sample whose coordinates are to be transformed.</param>
    /// <returns>A new PointerSample instance with coordinates adjusted to the surface coordinate system.</returns>
    private PointerSample TransformToSurface(PointerSample sample)
    {
        var zoom = _engineOptions.Viewport.Zoom;
        var panX = _engineOptions.Viewport.PanX;
        var panY = _engineOptions.Viewport.PanY;

        return sample with
        {
            X = (sample.X - panX) / zoom,
            Y = (sample.Y - panY) / zoom
        };
    }

    /// <summary>
    /// Begins a new stroke operation based on the current tool and the provided pointer sample.
    /// </summary>
    /// <remarks>The behavior of the stroke depends on the currently selected tool. This method initiates
    /// drawing, erasing, or selection actions as appropriate.</remarks>
    /// <param name="sample">The pointer sample representing the initial input position and state for the stroke. Must be in device
    /// coordinates.</param>
    public void BeginStroke(PointerSample sample)
    {
        sample = TransformToSurface(sample);

        if (CurrentTool == SignatureStrokeTool.Pointer)
        {
            UsePen();
        }

        CurrentStyle = BuildStyleFromOptions();

        switch (CurrentTool)
        {
            case SignatureStrokeTool.Pen:
            case SignatureStrokeTool.Highlighter:
                BeginDrawingStroke(sample);
                break;

            case SignatureStrokeTool.Eraser:
                BeginErasingStroke(sample);
                break;

            case SignatureStrokeTool.Selection:
                BeginSelectionStroke(sample);
                break;
        }
    }

    /// <summary>
    /// Updates the current stroke based on the provided pointer sample and the selected tool.
    /// </summary>
    /// <remarks>The behavior of this method depends on the currently selected tool. It updates drawing,
    /// erasing, or selection strokes accordingly.</remarks>
    /// <param name="sample">The pointer sample containing the latest input data to be processed. Must represent a valid position on the
    /// input surface.</param>
    public void UpdateStroke(PointerSample sample)
    {
        sample = TransformToSurface(sample);

        switch (CurrentTool)
        {
            case SignatureStrokeTool.Pen:
            case SignatureStrokeTool.Highlighter:
                UpdateDrawingStroke(sample);
                break;

            case SignatureStrokeTool.Eraser:
                UpdateErasingStroke(sample);
                break;

            case SignatureStrokeTool.Selection:
                UpdateSelectionStroke(sample);
                break;

            case SignatureStrokeTool.Pointer:
                UpdatePointerHover(sample);
                break;
        }
    }

    /// <summary>
    /// Completes the current stroke operation using the specified pointer sample, finalizing drawing, erasing, or
    /// selection actions based on the active tool.
    /// </summary>
    /// <remarks>The behavior of this method depends on the currently selected tool. It finalizes drawing for
    /// pen or highlighter tools, completes erasing for the eraser tool, or ends a selection operation for the selection
    /// tool.</remarks>
    /// <param name="sample">The pointer sample representing the input data at the end of the stroke. This sample is used to determine the
    /// final state of the stroke operation.</param>
    public void EndStroke(PointerSample sample)
    {
        sample = TransformToSurface(sample);

        switch (CurrentTool)
        {
            case SignatureStrokeTool.Pen:
            case SignatureStrokeTool.Highlighter:
                EndDrawingStroke(sample);
                break;

            case SignatureStrokeTool.Eraser:
                EndErasingStroke(sample);
                break;

            case SignatureStrokeTool.Selection:
                EndSelectionStroke(sample);
                break;
        }

        if (CurrentTool == SignatureStrokeTool.Pen)
        {
            UsePointer();
        }
    }

    /// <summary>
    /// Begins a new drawing stroke using the specified pointer sample and the current drawing style.
    /// </summary>
    /// <param name="sample">The pointer sample that provides input data for initializing the new stroke.</param>
    private void BeginDrawingStroke(PointerSample sample)
    {
        _strokes.BeginStroke(CurrentStyle.Clone());

        var points = _inputPipeline.Process(sample, CurrentStyle.Engine);
        _strokes.AddPoints(points);
    }

    /// <summary>
    /// Updates the current drawing stroke with points derived from the specified pointer sample and the current drawing
    /// style.
    /// </summary>
    /// <param name="sample">The pointer sample containing input data used to generate new points for the drawing stroke.</param>
    private void UpdateDrawingStroke(PointerSample sample)
    {
        var points = _inputPipeline.Process(sample, CurrentStyle.Engine);
        _strokes.AddPoints(points);

        var current = _strokes.CurrentStroke;

        if (current is null || current.Points.Count < 2)
        {
            return;
        }

        var pts = current.Points;
        var p2 = pts[^1];
        var p1 = pts[^2];

        RaiseStrokeSegment(p1, p2, CurrentStyle);
    }

    /// <summary>
    /// Completes the current drawing stroke using the provided pointer sample and processes it through the configured
    /// stroke processors.
    /// </summary>
    /// <remarks>If any stroke processor returns null during processing, the stroke is not added to the
    /// collection. This method also updates the stroke history and notifies listeners of changes.</remarks>
    /// <param name="sample">The pointer sample containing the input data used to finalize the current stroke.</param>
    private void EndDrawingStroke(PointerSample sample)
    {
        var points = _inputPipeline.Process(sample, CurrentStyle.Engine);
        _strokes.AddPoints(points);

        var stroke = _strokes.EndStroke();

        if (stroke is null)
        {
            return;
        }

        foreach (var processor in _strokeProcessors)
        {
            var processed = processor.Process([stroke], _engineOptions).FirstOrDefault();

            if (processed is null)
            {
                stroke = null;
                break;
            }

            stroke = processed;
        }

        if (stroke is null)
        {
            return;
        }

        _history.Execute(new AddStrokeAction(_strokes, stroke));
        NotifyChanged();
    }

    /// <summary>
    /// Updates the hover state based on the specified pointer sample and notifies listeners if the hovered stroke
    /// changes.
    /// </summary>
    /// <remarks>This method performs a hit test at the pointer location to identify the topmost stroke under
    /// the pointer. If the hovered stroke changes, the method triggers the OnHoverStrokeChanged event to notify
    /// subscribers.</remarks>
    /// <param name="sample">The pointer sample containing the X and Y coordinates used to determine the current hover state.</param>
    private void UpdatePointerHover(PointerSample sample)
    {
        HoverStroke = _strokeHitTester.HitTestTopMost(sample.X, sample.Y, _engineOptions.Selection.Tolerance);
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Begins the process of erasing a stroke using the specified pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample that provides the input data for initiating the erasing operation.</param>
    private void BeginErasingStroke(PointerSample sample)
    {
        GetEraser().Begin(sample);
    }

    /// <summary>
    /// Updates the current erasing stroke with the specified pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample containing the latest input data used to update the erasing stroke.</param>
    private void UpdateErasingStroke(PointerSample sample)
    {
        GetEraser().Update(sample);
        NotifyChanged();
    }

    /// <summary>
    /// Completes the erasing operation for the current stroke using the specified pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample that provides the final input data for the erasing operation.</param>
    private void EndErasingStroke(PointerSample sample)
    {
        GetEraser().End(sample);
        NotifyChanged();
    }

    /// <summary>
    /// Selects and returns the appropriate eraser implementation based on the current eraser mode option.
    /// </summary>
    /// <remarks>If the eraser mode is not recognized, the pixel eraser is returned by default.</remarks>
    /// <returns>An instance of an eraser that matches the configured eraser mode.</returns>
    private IEraser GetEraser() =>
        _engineOptions.Eraser.Mode switch
        {
            EraserMode.Pixel => _pixelEraser,
            EraserMode.Stroke => _strokeEraser,
            EraserMode.Hybrid => _hybridEraser,
            _ => _pixelEraser
        };

    /// <summary>
    /// Begins a new selection stroke using the specified pointer sample as the starting point.
    /// </summary>
    /// <param name="sample">The pointer sample that defines the initial position of the selection stroke.</param>
    private void BeginSelectionStroke(PointerSample sample)
    {
        _isSelecting = true;
        _selectionStart = sample;
        _selectionEnd = sample;
    }

    /// <summary>
    /// Updates the selection stroke based on the provided pointer sample.
    /// </summary>
    /// <param name="sample">The pointer sample representing the current position or state of the pointer used to update the selection
    /// stroke.</param>
    private void UpdateSelectionStroke(PointerSample sample)
    {
        if (!_isSelecting)
        {
            return;
        }

        _selectionEnd = sample;
        _selectionRect = BuildSelectionRect(_selectionStart!, sample);
        NotifyChanged();
    }

    private void EndSelectionStroke(PointerSample sample)
    {
        if (!_isSelecting)
        {
            return;
        }

        _isSelecting = false;
        _selectionEnd = sample;
        var dx = _selectionEnd.X - _selectionStart!.X;
        var dy = _selectionEnd.Y - _selectionStart!.Y;

        var distSq = dx * dx + dy * dy;
        var additive = sample.CtrlKey;

        if (distSq <= SelectionDragThreshold * SelectionDragThreshold)
        {
            _selectionManager.SelectStrokeAt(sample.X, sample.Y, additive);
        }
        else
        {
            var rect = BuildSelectionRect(_selectionStart, _selectionEnd);
            _selectionManager.SelectRectangle(rect, additive);
        }

        _selectionRect = null;
        NotifyChanged();
    }

    /// <summary>
    /// Creates a rectangle that encompasses the area defined by two pointer samples.
    /// </summary>
    /// <param name="start">The starting pointer sample that defines one corner of the selection area.</param>
    /// <param name="end">The ending pointer sample that defines the opposite corner of the selection area.</param>
    /// <returns>A rectangle representing the area between the two pointer samples.</returns>
    private static RectD BuildSelectionRect(PointerSample start, PointerSample end)
    {
        var x = Math.Min(start.X, end.X);
        var y = Math.Min(start.Y, end.Y);
        var w = Math.Abs(end.X - start.X);
        var h = Math.Abs(end.Y - start.Y);

        return new RectD(x, y, w, h);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _history.OnChanged -= NotifyChanged;
        _strokes.OnChanged -= NotifyChanged;
    }
}
