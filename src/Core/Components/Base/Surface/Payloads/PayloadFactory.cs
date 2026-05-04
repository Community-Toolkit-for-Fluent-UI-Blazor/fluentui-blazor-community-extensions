using FluentUI.Blazor.Community.Components.Surface.Payloads;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static factory methods for creating payload objects used in signature rendering and manipulation scenarios.
/// </summary>
/// <remarks>The PayloadFactory class centralizes the creation of various payload types, such as view, background,
/// grid, axes, watermark, pen, stroke, selection, and debug payloads. Each method constructs and initializes a specific
/// payload object based on the provided options or data models, ensuring consistent mapping between domain models and
/// their payload representations. This class is intended for internal use within the signature rendering infrastructure
/// and is not intended to be instantiated.</remarks>
internal static class PayloadFactory
{
    /// <summary>
    /// Creates a new view payload with the specified dimensions, DPI, scale, and offsets.
    /// </summary>
    /// <param name="width">The width of the view, in device-independent units.</param>
    /// <param name="height">The height of the view, in device-independent units.</param>
    /// <param name="dpi">The dots per inch (DPI) value to use for the view.</param>
    /// <param name="scale">The scale factor to apply to the view.</param>
    /// <param name="offsetX">The horizontal offset to apply to the view, in device-independent units.</param>
    /// <param name="offsetY">The vertical offset to apply to the view, in device-independent units.</param>
    /// <returns>A new instance of the ViewPayload class initialized with the specified parameters.</returns>
    public static ViewPayload CreateView(
        double width,
        double height,
        double dpi,
        double scale,
        double offsetX,
        double offsetY)
    {
        return new ViewPayload
        {
            Width = width,
            Height = height,
            Dpi = dpi,
            Scale = scale,
            OffsetX = offsetX,
            OffsetY = offsetY
        };
    }

    /// <summary>
    /// Creates a new instance of the BackgroundPayload class based on the specified signature background options.
    /// </summary>
    /// <param name="opt">The options used to configure the background payload. If null or if the Show property is false, no payload is
    /// created.</param>
    /// <returns>A BackgroundPayload instance initialized with the specified options, or null if the options are null or not set
    /// to show the background.</returns>
    public static BackgroundPayload? Create(SurfaceBackgroundOptions? opt)
    {
        if (opt is null || !opt.Show)
        {
            return null;
        }

        return new BackgroundPayload
        {
            Color = opt.Color,
            Opacity = opt.Opacity
        };
    }

    /// <summary>
    /// Creates a new instance of the GridPayload class based on the specified grid options.
    /// </summary>
    /// <param name="opt">The options used to configure the grid payload. If null or if the display mode is set to None, no payload is
    /// created.</param>
    /// <returns>A new GridPayload instance configured according to the provided options, or null if the options are null or
    /// specify a display mode of None.</returns>
    public static GridPayload? Create(SurfaceGridOptions? opt)
    {
        if (opt is null || opt.DisplayMode == GridDisplayMode.None)
        {
            return null;
        }

        return new GridPayload
        {
            DisplayMode = opt.DisplayMode,
            CellSize = opt.CellSize,
            Color = opt.Color,
            Opacity = opt.Opacity,
            BoldEvery = opt.BoldEvery,
            StrokeWidth = opt.StrokeWidth,
            DashArray = SurfaceMathUtils.ToDashArray(opt.DashArray),
            PointRadius = opt.PointRadius,
            Layer = opt.Layer,
            HorizontalLines = [],
            VerticalLines = [],
            Points = []
        };
    }

    /// <summary>
    /// Creates a new instance of the AxesPayload class based on the specified signature axes options.
    /// </summary>
    /// <param name="opt">The options used to configure the axes payload. If null or if the Show property is false, no payload is created.</param>
    /// <returns>An AxesPayload instance configured according to the provided options, or null if the options are null or not set
    /// to show the axes.</returns>
    public static AxisPayload? Create(SurfaceAxesOptions? opt)
    {
        if (opt is null || !opt.Show)
        {
            return null;
        }

        return new AxisPayload
        {
            Color = opt.Color,
            Opacity = opt.Opacity,
            StrokeWidth = opt.StrokeWidth,
            DashArray = SurfaceMathUtils.ToDashArray(opt.DashArray),
            Layer = opt.Layer
        };
    }

    /// <summary>
    /// Creates a new instance of the WatermarkPayload class based on the specified watermark options.
    /// </summary>
    /// <param name="opt">The options used to configure the watermark. If null or not enabled, no payload is created.</param>
    /// <returns>A WatermarkPayload instance configured according to the provided options, or null if the options are null or not
    /// enabled.</returns>
    public static WatermarkPayload? Create(SurfaceWatermarkOptions? opt)
    {
        if (opt is null || !opt.Enabled)
        {
            return null;
        }

        return new WatermarkPayload
        {
            Text = opt.Text,
            ImageUrl = opt.ImageUrl,
            Mode = opt.Mode,
            Opacity = opt.Opacity,
            TextOpacity = opt.TextOpacity,
            ImageOpacity = opt.ImageOpacity,
            FontFamily = opt.FontFamily,
            FontSize = opt.FontSize,
            FontWeight = opt.FontWeight,
            Color = opt.Color,
            Rotation = opt.Rotation,
            PositionX = opt.Position.X,
            PositionY = opt.Position.Y,
            Repeat = opt.Repeat,
            RepeatSpacingX = opt.RepeatSpacingX,
            RepeatSpacingY = opt.RepeatSpacingY,
            HorizontalAlignment = (int)opt.HorizontalAlignment,
            VerticalAlignment = (int)opt.VerticalAlignment,
            LetterSpacing = opt.LetterSpacing,
            Scale = opt.Scale,
            VisualBias = opt.VisualBias
        };
    }

    /// <summary>
    /// Creates a new instance of the PenPayload class using the specified rendering style and stroke width.
    /// </summary>
    /// <param name="style">The rendering style to apply to the pen, including color, opacity, line cap, line join, dash pattern, and shadow
    /// settings.</param>
    /// <param name="width">The width of the pen stroke, in device-independent units.</param>
    /// <returns>A PenPayload object configured with the specified style and width.</returns>
    public static PenPayload Create(SignatureStrokeRenderingStyle style, double width)
    {
        return new PenPayload
        {
            Color = style.Color,
            Opacity = style.Opacity * 0.01,
            Width = width,
            LineCap = style.LineCap,
            LineJoin = style.LineJoin,
            DashArray = SurfaceMathUtils.ToDashArray(style.DashArray),
            Shadow = Create(style.Shadow)
        };
    }

    /// <summary>
    /// Creates a new instance of the ShadowPayload class using the specified shadow options.
    /// </summary>
    /// <param name="opt">The options that define the shadow's enabled state, color, opacity, blur, and offset values. Cannot be null.</param>
    /// <returns>A ShadowPayload instance initialized with the values from the specified options.</returns>
    public static ShadowPayload Create(SignatureShadowOptions opt)
    {
        return new ShadowPayload
        {
            Enabled = opt.Enabled,
            Color = opt.Color,
            Opacity = opt.Opacity,
            Blur = opt.Blur,
            OffsetX = opt.OffsetX,
            OffsetY = opt.OffsetY
        };
    }

    /// <summary>
    /// Creates a new instance of the StrokePointPayload class from the specified signature point.
    /// </summary>
    /// <param name="p">The signature point containing the coordinates, pressure, and width information to use for the payload. Cannot
    /// be null.</param>
    /// <returns>A StrokePointPayload object initialized with the values from the specified signature point.</returns>
    public static StrokePointPayload Create(SignaturePoint p)
    {
        return new StrokePointPayload
        {
            X = p.X,
            Y = p.Y,
            P = p.Pressure,
            W = p.Width
        };
    }

    /// <summary>
    /// Creates a new instance of the StrokePayload class from the specified signature stroke.
    /// </summary>
    /// <param name="stroke">The signature stroke to convert into a StrokePayload. Cannot be null.</param>
    /// <returns>A StrokePayload object representing the provided signature stroke.</returns>
    public static StrokePayload Create(SignatureStroke stroke)
    {
        var width = stroke.Points.Count > 0
            ? stroke.Points[^1].Width
            : stroke.Style.Engine.BaseWidth;

        return new StrokePayload
        {
            Id = stroke.Id.ToString(),
            BlendMode = MapBlendMode(stroke.Style.Rendering.BlendMode),
            Points = [.. stroke.Points.Select(Create)],
            Pen = Create(stroke.Style.Rendering, width)
        };
    }

    /// <summary>
    /// Creates a list of stroke payloads from the specified collection of signature strokes.
    /// </summary>
    /// <param name="strokes">The collection of signature strokes to convert. Cannot be null.</param>
    /// <returns>A list of stroke payloads corresponding to the provided signature strokes. The list will be empty if no strokes
    /// are provided.</returns>
    public static List<StrokePayload> Create(IReadOnlyList<SignatureStroke> strokes)
    {
        return [.. strokes.Select(Create)];
    }

    /// <summary>
    /// Creates a new stroke layer payload with the specified identifier and collection of signature strokes.
    /// </summary>
    /// <param name="layerId">The unique identifier to assign to the stroke layer.</param>
    /// <param name="strokes">The collection of signature strokes to include in the layer. Cannot be null.</param>
    /// <returns>A new instance of StrokeLayerPayload containing the specified identifier and strokes.</returns>
    public static StrokeLayerPayload CreateStrokeLayer(
        string layerId,
        IReadOnlyList<SignatureStroke> strokes)
    {
        return new StrokeLayerPayload
        {
            Id = layerId,
            Strokes = Create(strokes)
        };
    }

    /// <summary>
    /// Creates a new dynamic stroke payload based on the current stroke, selected strokes, and an optional selection
    /// rectangle.
    /// </summary>
    /// <param name="current">The current signature stroke to use as the basis for the dynamic stroke. If null, an empty payload is returned.</param>
    /// <param name="selected">A read-only list of signature strokes that are currently selected. The identifiers of these strokes are included
    /// in the payload.</param>
    /// <param name="selectionRect">An optional rectangle representing the current selection area. If null, no selection rectangle is included in
    /// the payload.</param>
    /// <returns>A new instance of DynamicStrokePayload containing the dynamic stroke data. If the current stroke is null,
    /// returns an empty payload.</returns>
    public static DynamicStrokePayload CreateDynamicStroke(
        SignatureStroke? current,
        IReadOnlyList<SignatureStroke> selected,
        RectD? selectionRect)
    {
        if (current is null)
        {
            return new DynamicStrokePayload();
        }

        return new DynamicStrokePayload
        {
            Pen = Create(current.Style.Rendering, current.Points[^1].Width),
            Strokes = [Create(current)],
            Selected = [.. selected.Select(s => s.Id.ToString())],
            SelectionRect = selectionRect is null ? null : Create(selectionRect.Value)
        };
    }

    /// <summary>
    /// Creates a new selection payload based on the specified rendering options and selected signature strokes.
    /// </summary>
    /// <param name="opt">The rendering options to apply to the selection, including color, opacity, width, and highlight state.</param>
    /// <param name="selected">The collection of signature strokes to include in the selection. Each stroke's identifier will be added to the
    /// payload.</param>
    /// <returns>A new instance of SelectionPayload containing the identifiers of the selected strokes and the specified
    /// rendering options.</returns>
    public static SelectionPayload CreateSelection(SignatureSelectionRenderingOptions opt, IReadOnlyList<SignatureStroke> selected)
    {
        return new SelectionPayload
        {
            StrokeIds = [.. selected.Select(s => s.Id.ToString())],
            Color = opt.Color,
            Opacity = opt.Opacity,
            Width = opt.Width,
            Highlight = opt.Highlight
        };
    }

    /// <summary>
    /// Creates a new instance of the DebugTextPayload class using the specified signature debug text options.
    /// </summary>
    /// <param name="opt">The options that specify the font family, font size, and color to use for the debug text. Cannot be null.</param>
    /// <returns>A new DebugTextPayload instance initialized with the font family, font size, and color from the provided
    /// options.</returns>
    public static DebugTextPayload Create(SurfaceDebugTextOptions opt)
    {
        return new DebugTextPayload
        {
            FontFamily = opt.FontFamily,
            FontSize = opt.FontSize,
            Color = opt.Color
        };
    }

    /// <summary>
    /// Creates a new instance of the DebugPayload class using the specified DPI, stroke count, point count, and debug
    /// options.
    /// </summary>
    /// <param name="dpi">The dots per inch (DPI) value to use for rendering debug information.</param>
    /// <param name="strokeCount">The number of strokes to include in the debug payload.</param>
    /// <param name="pointCount">The number of points to include in the debug payload.</param>
    /// <param name="options">The options that specify how the debug payload should be constructed, including background and text settings.
    /// Cannot be null.</param>
    /// <returns>A DebugPayload object initialized with the provided parameters and options.</returns>
    public static DebugPayload CreateDebug(
        double dpi,
        int strokeCount,
        int pointCount,
        SurfaceDebugOptions options)
    {
        return new DebugPayload
        {
            Dpi = dpi,
            StrokeCount = strokeCount,
            PointCount = pointCount,
            Background = Create(options.Background),
            Text = Create(options.TextOptions)
        };
    }

    /// <summary>
    /// Creates a new instance of the RectPayload structure from the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle whose position and size are used to initialize the payload.</param>
    /// <returns>A RectPayload instance with properties set to match the specified rectangle.</returns>
    public static RectPayload Create(RectD rect)
    {
        return new RectPayload
        {
            X = rect.X,
            Y = rect.Y,
            Width = rect.Width,
            Height = rect.Height
        };
    }

    /// <summary>
    /// Maps a specified stroke blend mode to its corresponding canvas blend mode string.
    /// </summary>
    /// <remarks>If the provided blend mode is not recognized, the method returns "source-over" as the default
    /// value.</remarks>
    /// <param name="mode">The stroke blend mode to convert.</param>
    /// <returns>A string representing the canvas blend mode that corresponds to the specified stroke blend mode.</returns>
    public static string MapBlendMode(StrokeBlendMode mode)
    {
        return mode switch
        {
            StrokeBlendMode.Normal => "source-over",
            StrokeBlendMode.Multiply => "multiply",
            StrokeBlendMode.Additive => "lighter",
            StrokeBlendMode.Screen => "screen",
            StrokeBlendMode.Overlay => "overlay",
            _ => "source-over"
        };
    }

    /// <summary>
    /// Creates a new hover payload based on the specified signature stroke and hover rendering options.
    /// </summary>
    /// <param name="stroke">The signature stroke to use as the basis for the hover payload. Can be null.</param>
    /// <param name="hover">The rendering options that determine how the hover effect is displayed. The hover must be enabled for a payload
    /// to be created.</param>
    /// <returns>A new HoverPayload instance if the hover is enabled and the stroke is not null; otherwise, null.</returns>
    public static HoverPayload? CreateHover(
        SignatureStroke? stroke,
        SignatureHoverRenderingOptions hover)
    {
        if (!hover.Enabled || stroke is null)
        {
            return null;
        }

        return new HoverPayload
        {
            Id = stroke.Id,
            Color = hover.Color,
            Width = hover.Width,
            Opacity = hover.Opacity,
            Points = [.. stroke.Points.Select(Create)]
        };
    }

    /// <summary>
    /// Creates a new eraser payload based on the specified pointer sample and eraser options.
    /// </summary>
    /// <param name="sample">The pointer sample containing the X and Y coordinates to use for the eraser payload.</param>
    /// <param name="options">The options that define the eraser's size, radius, shape, soft edges, mode, and tolerance.</param>
    /// <returns>An instance of EraserPayload initialized with the provided sample and options, or null if the payload could not
    /// be created.</returns>
    public static EraserPayload? CreateEraser(
        PointerSample sample,
        SignatureEraserOptions options)
    {
        return new EraserPayload
        {
            X = sample.X,
            Y = sample.Y,
            Size = options.Size,
            Radius = options.Radius,
            Shape = (int)options.Shape,
            SoftEdges = options.SoftEdges,
            SoftEdgeRadius = options.SoftEdgeRadius,
            Mode = (int)options.Mode,
            Tolerance = options.Tolerance
        };
    }
}

