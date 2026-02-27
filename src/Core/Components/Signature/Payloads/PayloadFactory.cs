namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// 
/// </summary>
internal static class PayloadFactory
{
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

    public static BackgroundPayload? Create(SignatureBackgroundOptions? opt)
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

    public static GridPayload? Create(SignatureGridOptions? opt)
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
            DashArray = SignatureMathUtils.ToDashArray(opt.DashArray),
            PointRadius = opt.PointRadius,
            Layer = opt.Layer
        };
    }

    public static AxesPayload? Create(SignatureAxesOptions? opt)
    {
        if (opt is null || !opt.Show)
        {
            return null;
        }

        return new AxesPayload
        {
            Color = opt.Color,
            Opacity = opt.Opacity,
            StrokeWidth = opt.StrokeWidth,
            DashArray = SignatureMathUtils.ToDashArray(opt.DashArray),
            Layer = opt.Layer
        };
    }

    public static WatermarkPayload? Create(SignatureWatermarkOptions? opt)
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

    public static PenPayload Create(SignatureStrokeRenderingStyle style, double width)
    {
        return new PenPayload
        {
            Color = style.Color,
            Opacity = style.Opacity * 0.01,
            Width = width,
            LineCap = style.LineCap,
            LineJoin = style.LineJoin,
            DashArray = SignatureMathUtils.ToDashArray(style.DashArray),
            Shadow = Create(style.Shadow)
        };
    }

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

    public static List<StrokePayload> Create(IReadOnlyList<SignatureStroke> strokes)
    {
        return [.. strokes.Select(Create)];
    }

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

    public static DebugTextPayload Create(SignatureDebugTextOptions opt)
    {
        return new DebugTextPayload
        {
            FontFamily = opt.FontFamily,
            FontSize = opt.FontSize,
            Color = opt.Color
        };
    }

    public static DebugPayload CreateDebug(
        double dpi,
        int strokeCount,
        int pointCount,
        SignatureDebugOptions options)
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

    public static HoverPayload? CreateHover(SignatureStroke? stroke, SignatureHoverRenderingOptions hover)
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
}

