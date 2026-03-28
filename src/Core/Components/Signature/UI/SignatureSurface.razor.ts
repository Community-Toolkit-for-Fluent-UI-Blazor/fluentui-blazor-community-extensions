// Note : this asynchronous function must be outside the namespace, otherwise Blazor will raise a TaskCanceledException
// when trying to call it from C#. The reason is not entirely clear, but it seems to be related to how Blazor handles async
// interop calls and the fact that functions inside namespaces may not be properly recognized as async by Blazor's interop
// mechanism. By placing the function outside the namespace, we ensure that Blazor can correctly identify it as an async
// function and handle it accordingly, allowing us to return a Promise that resolves to the base64 string of the image
export async function EncodeToImage(canvasId: string, mime: string, quality: number) {
  const canvas = document.getElementById(canvasId) as HTMLCanvasElement | null;

  if (!canvas) {
    throw new Error(`Canvas '${canvasId}' not found.`);
  }

  const blob: Blob = await new Promise((resolve, reject) => {
    canvas.toBlob(b => {
      if (!b) {
        reject(new Error("Canvas toBlob() returned null."));
        return;
      }

      if (b.type && mime && b.type !== mime) {
        reject(new Error(`Requested ${mime} but got ${b.type}`));
        return;
      }

      resolve(b);
    }, mime, quality);
  });

  const buffer = await blob.arrayBuffer();
  const bytes = new Uint8Array(buffer);

  let binary = "";

  for (let i = 0; i < bytes.length; i++) {
    binary += String.fromCharCode(bytes[i]);
  }

  const base64 = btoa(binary);

  return base64;
}

export namespace FluentUI.Blazor.Community.Signature {

  interface StrokePointPayload {
    x: number;
    y: number;
    p: number;
  }

  interface StrokePayload {
    id: string;
    points: StrokePointPayload[];
    blendMode: GlobalCompositeOperation;
    pen: PenPayload;
  }

  interface RectPayload {
    x: number;
    y: number;
    width: number;
    height: number;
  }

  interface ShadowPayload {
    enabled: boolean;
    color: string;
    opacity: number;
    blur: number;
    offsetX: number;
    offsetY: number;
  }

  interface PenPayload {
    color: string;
    opacity: number;
    width: number;
    lineCap: CanvasLineCap;
    lineJoin: CanvasLineJoin;
    dashArray: number[] | null;
    shadow: ShadowPayload;
  }

  interface ViewPayload {
    width: number;
    height: number;
    offsetX: number;
    offsetY: number;
    renderWidth: number;
    renderHeight: number;
    dpi: number;
    scale: number;
  }

  interface BackgroundPayload {
    color: string;
    opacity: number;
  }

  enum GridDisplayMode {
    None = 0,
    Lines = 1,
    Dots = 2
  }

  enum GridLayer {
    Background = 0,
    Foreground = 1
  }

  interface GridPayload {
    color: string;
    opacity: number;
    cellSize: number;
    pointRadius: number;
    strokeWidth: number;
    boldEvery: number;
    displayMode: GridDisplayMode;
    layer: GridLayer;
  }

  interface AxesPayload {
    strokeWidth: number;
    color: string;
    opacity: number;
    dashArray: number[] | null;
    layer: GridLayer;
  }

  interface StrokeLayerPayload {
    id: string;
    strokes: StrokePayload[];
  }

  interface DynamicStrokePayload {
    pen: PenPayload;
    strokes: StrokePayload[];
    selected: string[];
    selectionRect?: RectPayload | null;
  }

  enum WatermarkMode {
    Text,
    Image,
    Both
  }

  enum WatermarkHorizontalAlignment {
    Left,
    Center,
    Right
  }

  enum WatermarkVerticalAlignment {
    Top,
    Center,
    Bottom
  }

  enum EraserShape {
    Circle,
    Square
  }

  enum EraserMode {
    Pixel,
    Stroke,
    Hybrid
  }

  interface EraserHoverPayload {
    x: number;
    y: number;
    size: number;
    radius: number;
    shape: EraserShape;
    softEdges: boolean;
    softEdgeRadius: number;
    mode: EraserMode;
    tolerance: number;
  }

  interface WatermarkPayload {
    repeat: boolean;
    color: string;
    mode: WatermarkMode;
    text: string;
    imageUrl: string;
    opacity: number;
    textOpacity: number;
    imageOpacity: number;
    fontSize: number;
    fontFamily: string;
    fontWeight: string;
    letterSpacing: number;
    scale: number;
    rotation: number;
    positionX: number;
    positionY: number;
    horizontalAlignment: WatermarkHorizontalAlignment;
    verticalAlignment: WatermarkVerticalAlignment;
    repeatSpacingX: number;
    repeatSpacingY: number;
    visualBias: number;
  }

  interface SelectionPayload {
    strokeIds: string[];
    color: string;
    opacity: number;
    width: number;
    highlight: boolean;
  }

  interface DebugTextPayload {
    color: string;
    fontSize: number;
    fontFamily: string;
  }

  interface DebugPayload {
    dpi: number;
    strokeCount: number;
    pointCount: number;
    background: BackgroundPayload | null;
    text: DebugTextPayload | null;
  }

  interface HoverPayload {
    id: string;
    color: string;
    width: number;
    opacity: number;
    points: StrokePointPayload[];
  }

  interface CanvasFramePayload {
    view: ViewPayload;
    layers: Record<string, any>;
  }

  interface SurfaceContext {
    target: CanvasRenderingContext2D;
    eraser: CanvasRenderingContext2D;
  }

  const surfacesCache = new Map<string, SurfaceContext>();
  const imageCache = new Map<string, HTMLImageElement>();
  const strokeLayerCache = new Map<string, HTMLCanvasElement>();
  const backgroundCache = new Map<string, HTMLCanvasElement>();
  const gridCache = new Map<string, HTMLCanvasElement>();
  const axesCache = new Map<string, HTMLCanvasElement>();
  const watermarkCache = new Map<string, HTMLCanvasElement>();

  export function GetPointerPosition(id: string, clientX: number, clientY: number, view: ViewPayload) {
    const surfaces = surfacesCache.get(id);

    if (!surfaces || !surfaces.target || !view) {
      return { x: clientX, y: clientY };
    }

    const rect = surfaces.target.canvas.getBoundingClientRect();

    if (!rect || isNaN(rect.left) || isNaN(rect.top)) {
      return { x: clientX, y: clientY };
    }

    const xCanvas = clientX - rect.left;
    const yCanvas = clientY - rect.top;

    const scale = view.scale || 1;
    const offsetX = view.offsetX || 0;
    const offsetY = view.offsetY || 0;

    if (!scale || scale === 0) {
      return { x: clientX, y: clientY };
    }

    const xSurface = (xCanvas - offsetX) / scale;
    const ySurface = (yCanvas - offsetY) / scale;

    return {
      x: Number.isFinite(xSurface) ? xSurface : clientX,
      y: Number.isFinite(ySurface) ? ySurface : clientY
    };
  }

  export function GetDPI() {
    return (window.devicePixelRatio || 1) * 96;
  }

  export function GetCanvasSize(canvasId: string) {
    const surfaces = surfacesCache.get(canvasId);

    if (!surfaces || !surfaces.target) {
      return { width: 0, height: 0 };
    }

    const canvas = surfaces.target.canvas;

    return { width: canvas.width, height: canvas.height };
  }

  export function ResizeCanvas(id: string, w: number, h: number) {
    const surfaces = surfacesCache.get(id);
    if (!surfaces) return;

    surfaces.target.canvas.width = w;
    surfaces.target.canvas.height = h;
  }

  export function RegisterCanvas(canvasId: string, isOffscreen: boolean, w: number | null, h: number | null) {
    let canvas: HTMLCanvasElement;

    if (isOffscreen) {
      canvas = document.createElement("canvas");
      canvas.id = canvasId;
      canvas.style.display = "none";
      canvas.width = w!;
      canvas.height = h!;
      document.body.appendChild(canvas);
    } else {
      canvas = document.getElementById(canvasId) as HTMLCanvasElement;

      if (canvas) {
        const rect = canvas.getBoundingClientRect();
        canvas.width = rect.width;
        canvas.height = rect.height;
      }
    }

    if (!canvas) {
      console.error("Canvas not found:", canvasId);
      return;
    }

    const targetCtx = canvas.getContext("2d", { alpha: !isOffscreen })!;
    targetCtx.fillStyle = isOffscreen ? "#fff" : "transparent";

    if (isOffscreen) {
      targetCtx.fillRect(0, 0, canvas.width, canvas.height);
    }

    targetCtx.setTransform(1, 0, 0, 1, 0, 0);

    const eraserCanvas = document.createElement("canvas");
    eraserCanvas.id = canvasId + "_eraser";
    eraserCanvas.width = canvas.width;
    eraserCanvas.height = canvas.height;
    eraserCanvas.style.display = "none";
    document.body.appendChild(eraserCanvas);

    const eraserCtx = eraserCanvas.getContext("2d")!;

    surfacesCache.set(canvasId, {
      target: targetCtx,
      eraser: eraserCtx
    });
  }

  export function ResizeOffscreens(id: string, w: number, h: number) {
    const surfaces = surfacesCache.get(id);
    if (!surfaces) return;

    surfaces.target.canvas.width = w;
    surfaces.target.canvas.height = h;

    const clearCacheForCanvas = (cache: Map<string, HTMLCanvasElement>) => {
      for (const key of cache.keys()) {
        if (key.startsWith(id + "§") || key.startsWith(id + "|")) {
          cache.delete(key);
        }
      }
    };

    clearCacheForCanvas(backgroundCache);
    clearCacheForCanvas(gridCache);
    clearCacheForCanvas(axesCache);
    clearCacheForCanvas(strokeLayerCache);
    clearCacheForCanvas(watermarkCache);
  }

  function getSurfaceSize(ctx: CanvasRenderingContext2D) {
    return {
      width: ctx.canvas.width,
      height: ctx.canvas.height
    };
  }

  export function DrawStrokeSegment(
    canvasId: string,
    payload: {
      p1: { x: number; y: number; width: number };
      p2: { x: number; y: number; width: number };
      pen: PenPayload;
      blendMode: GlobalCompositeOperation;
    },
    view: ViewPayload
  ) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.target;
    const scaleTotal = view.scale;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    ctx.globalCompositeOperation = payload.blendMode;
    ctx.strokeStyle = payload.pen.color;
    ctx.globalAlpha = payload.pen.opacity;

    const w = (payload.p1.width + payload.p2.width) / 2;
    ctx.lineWidth = w * scaleTotal;

    ctx.lineCap = payload.pen.lineCap;
    ctx.lineJoin = payload.pen.lineJoin;

    if (payload.pen.dashArray) {
      ctx.setLineDash(payload.pen.dashArray.map(v => v * scaleTotal));
    } else {
      ctx.setLineDash([]);
    }

    if (payload.pen.shadow.enabled) {
      ctx.shadowColor = payload.pen.shadow.color;
      ctx.shadowBlur = payload.pen.shadow.blur * scaleTotal;
      ctx.shadowOffsetX = payload.pen.shadow.offsetX * scaleTotal;
      ctx.shadowOffsetY = payload.pen.shadow.offsetY * scaleTotal;
    } else {
      ctx.shadowBlur = 0;
      ctx.shadowOffsetX = 0;
      ctx.shadowOffsetY = 0;
    }

    ctx.beginPath();
    ctx.moveTo(payload.p1.x * scaleTotal, payload.p1.y * scaleTotal);
    ctx.lineTo(payload.p2.x * scaleTotal, payload.p2.y * scaleTotal);
    ctx.stroke();

    ctx.restore();
  }

  function computeAlignedPosition(
    payload: WatermarkPayload,
    width: number,
    height: number
  ) {
    let x = 0;
    let y = 0;

    switch (payload.horizontalAlignment) {
      case WatermarkHorizontalAlignment.Left: x = 0; break;
      case WatermarkHorizontalAlignment.Center: x = width / 2; break;
      case WatermarkHorizontalAlignment.Right: x = width; break;
    }

    switch (payload.verticalAlignment) {
      case WatermarkVerticalAlignment.Top: y = 0; break;
      case WatermarkVerticalAlignment.Center: y = height / 2; break;
      case WatermarkVerticalAlignment.Bottom: y = height; break;
    }

    return { x, y };
  }

  function loadImage(url: string): Promise<HTMLImageElement> {
    return new Promise((resolve, reject) => {
      if (imageCache.has(url)) {
        resolve(imageCache.get(url)!);
        return;
      }

      const img = new Image();
      img.onload = () => {
        imageCache.set(url, img);
        resolve(img);
      };
      img.onerror = reject;
      img.src = url;
    });
  }

  function makeStrokeLayerKey(
    canvasId: string,
    view: ViewPayload,
    layer: StrokeLayerPayload): string {
    const scale = view.scale;

    const w = Math.round(view.renderWidth * scale);
    const h = Math.round(view.renderHeight * scale);

    if (w <= 0 || h <= 0) return "__invalid__";

    const strokes = Array.isArray(layer?.strokes) ? layer.strokes : [];

    const hashParts = strokes.map(s =>
      `${s.id}:${s.points.length}:${s.pen.width}:${s.pen.color}:${s.blendMode}`
    );

    const hash = hashParts.join("|");

    return [
      canvasId,
      view.scale,
      w,
      h,
      hash
    ].join("§");
  }

  function makeBackgroundKey(
    canvasId: string,
    view: ViewPayload,
    payload: BackgroundPayload
  ): string {
    const w = view.renderWidth;
    const h = view.renderHeight;

    if (w <= 0 || h <= 0) return "__invalid__";

    return [
      canvasId,
      payload.color,
      payload.opacity,
      w,
      h
    ].join("|");
  }

  function makeGridKey(
    canvasId: string,
    view: ViewPayload,
    payload: GridPayload
  ): string {
    const w = view.renderWidth;
    const h = view.renderHeight;

    if (w <= 0 || h <= 0) return "__invalid__";

    return [
      canvasId,
      payload.color,
      payload.opacity,
      payload.cellSize,
      payload.pointRadius,
      payload.strokeWidth,
      payload.boldEvery,
      payload.displayMode,
      w,
      h
    ].join("|");
  }

  function makeWatermarkKey(
    canvasId: string,
    view: ViewPayload,
    payload: WatermarkPayload,
    width: number,
    height: number
  ): string {
    if (width <= 0 || height <= 0) return "__invalid__";

    return [
      canvasId,
      view.offsetX,
      view.offsetY,
      payload.text,
      payload.color,
      payload.fontSize,
      payload.fontFamily,
      payload.fontWeight,
      payload.scale,
      payload.rotation,
      payload.repeat,
      payload.repeatSpacingX,
      payload.repeatSpacingY,
      payload.positionX,
      payload.positionY,
      width,
      height,
      payload.visualBias
    ].join("|");
  }

  function makeAxesKey(
    canvasId: string,
    view: ViewPayload,
    payload: AxesPayload
  ): string {
    const w = view.renderWidth;
    const h = view.renderHeight;

    if (w <= 0 || h <= 0) return "__invalid__";

    const dash = payload.dashArray?.join(",") ?? "";

    return [
      canvasId,
      payload.color,
      payload.opacity,
      payload.strokeWidth,
      dash,
      w,
      h
    ].join("|");
  }

  function renderBackgroundLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: BackgroundPayload
  ) {
    const { width, height } = getSurfaceSize(ctx);

    const key = makeBackgroundKey(canvasId, view, payload);
    if (key === "__invalid__") return;

    let offscreen = backgroundCache.get(key);

    if (!offscreen) {
      offscreen = document.createElement("canvas");
      offscreen.width = width;
      offscreen.height = height;

      const bgctx = offscreen.getContext("2d")!;
      bgctx.save();

      bgctx.fillStyle = payload.color;
      bgctx.globalAlpha = payload.opacity;
      bgctx.fillRect(0, 0, width, height);

      bgctx.restore();
      backgroundCache.set(key, offscreen);
    }

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.drawImage(offscreen, 0, 0);
    ctx.restore();
  }

  function renderGridLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: GridPayload
  ) {
    const { width, height } = getSurfaceSize(ctx);

    const key = makeGridKey(canvasId, view, payload);
    if (key === "__invalid__") return;

    let offscreen = gridCache.get(key);

    if (!offscreen) {
      offscreen = document.createElement("canvas");
      offscreen.width = width;
      offscreen.height = height;

      const gctx = offscreen.getContext("2d")!;
      gctx.save();

      gctx.strokeStyle = payload.color;
      gctx.globalAlpha = payload.opacity;
      gctx.lineWidth = payload.strokeWidth;

      const cell = payload.cellSize;

      if (payload.displayMode === GridDisplayMode.Lines) {
        gctx.beginPath();

        for (let x = 0; x <= width; x += cell) {
          gctx.moveTo(x, 0);
          gctx.lineTo(x, height);
        }

        for (let y = 0; y <= height; y += cell) {
          gctx.moveTo(0, y);
          gctx.lineTo(width, y);
        }

        gctx.stroke();
      }

      if (payload.displayMode === GridDisplayMode.Dots) {
        const r = payload.pointRadius;

        for (let x = 0; x <= width; x += cell) {
          for (let y = 0; y <= height; y += cell) {
            gctx.beginPath();
            gctx.arc(x, y, r, 0, Math.PI * 2);
            gctx.fill();
          }
        }
      }

      gctx.restore();
      gridCache.set(key, offscreen);
    }

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.drawImage(offscreen, 0, 0);
    ctx.restore();
  }

  function renderAxesLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: AxesPayload
  ) {
    const key = makeAxesKey(canvasId, view, payload);
    if (key === "__invalid__") return;

    const { width, height } = getSurfaceSize(ctx);

    let offscreen = axesCache.get(key);

    if (!offscreen) {
      offscreen = document.createElement("canvas");
      offscreen.width = width;
      offscreen.height = height;

      const axctx = offscreen.getContext("2d");
      if (!axctx) return;

      axctx.save();

      axctx.strokeStyle = payload.color;
      axctx.globalAlpha = payload.opacity;
      axctx.lineWidth = payload.strokeWidth;

      if (payload.dashArray && payload.dashArray.length > 0) {
        axctx.setLineDash(payload.dashArray);
      }

      axctx.beginPath();

      const cx = width / 2;
      const cy = height / 2;

      axctx.moveTo(cx, 0);
      axctx.lineTo(cx, height);

      axctx.moveTo(0, cy);
      axctx.lineTo(width, cy);

      axctx.stroke();
      axctx.restore();

      axesCache.set(key, offscreen);
    }

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.drawImage(offscreen, 0, 0);
    ctx.restore();
  }

  function renderDynamicStroke(
    ctx: CanvasRenderingContext2D,
    stroke: StrokePayload,
    scaleTotal: number
  ) {
    ctx.save();

    ctx.globalCompositeOperation = stroke.blendMode;

    ctx.strokeStyle = stroke.pen.color;
    ctx.globalAlpha = stroke.pen.opacity;
    ctx.lineWidth = stroke.pen.width * scaleTotal;
    ctx.lineCap = stroke.pen.lineCap;
    ctx.lineJoin = stroke.pen.lineJoin;

    if (stroke.pen.dashArray) {
      ctx.setLineDash(stroke.pen.dashArray.map(v => v * scaleTotal));
    } else {
      ctx.setLineDash([]);
    }

    if (stroke.pen.shadow.enabled) {
      ctx.shadowColor = stroke.pen.shadow.color;
      ctx.shadowBlur = stroke.pen.shadow.blur * scaleTotal;
      ctx.shadowOffsetX = stroke.pen.shadow.offsetX * scaleTotal;
      ctx.shadowOffsetY = stroke.pen.shadow.offsetY * scaleTotal;
    } else {
      ctx.shadowBlur = 0;
      ctx.shadowOffsetX = 0;
      ctx.shadowOffsetY = 0;
    }

    ctx.beginPath();
    const pts = stroke.points;

    if (pts.length > 0) {
      ctx.moveTo(pts[0].x * scaleTotal, pts[0].y * scaleTotal);

      for (let i = 1; i < pts.length; i++) {
        ctx.lineTo(pts[i].x * scaleTotal, pts[i].y * scaleTotal);
      }
    }

    ctx.stroke();
    ctx.restore();
  }

  function renderStrokeLayerCached(
    canvasId: string,
    view: ViewPayload,
    layer: StrokeLayerPayload,
    ctx: CanvasRenderingContext2D
  ) {
    const key = makeStrokeLayerKey(canvasId, view, layer);

    if (key === "__invalid__") {
      return;
    }

    let offscreen = strokeLayerCache.get(key);

    if (!offscreen) {
      const scale = view.scale;
      const w = Math.round(view.renderWidth * scale);
      const h = Math.round(view.renderHeight * scale);

      offscreen = document.createElement("canvas");
      offscreen.width = w;
      offscreen.height = h;

      const offctx = offscreen.getContext("2d");
      if (!offctx) return;

      offctx.save();

      const strokes = Array.isArray(layer?.strokes) ? layer.strokes : [];

      for (const stroke of strokes) {
        renderDynamicStroke(offctx, stroke, scale);
      }

      offctx.restore();

      strokeLayerCache.set(key, offscreen);
    }

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.drawImage(offscreen, 0, 0);
    ctx.restore();
  }

  function renderStrokeLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: StrokeLayerPayload
  ) {
    renderStrokeLayerCached(canvasId, view, payload, ctx);
  }

  function renderSelectedStrokeHighlight(
    ctx: CanvasRenderingContext2D,
    stroke: StrokePayload,
    scaleTotal: number,
    selection: SelectionPayload
  ) {
    if (!selection.highlight) return;

    ctx.save();

    ctx.strokeStyle = selection.color;
    ctx.globalAlpha = selection.opacity;
    ctx.lineWidth = (stroke.pen.width + selection.width) * scaleTotal;
    ctx.lineCap = stroke.pen.lineCap;
    ctx.lineJoin = stroke.pen.lineJoin;
    ctx.setLineDash([]);

    ctx.beginPath();
    const pts = stroke.points;

    if (pts.length > 0) {
      ctx.moveTo(pts[0].x * scaleTotal, pts[0].y * scaleTotal);

      for (let i = 1; i < pts.length; i++) {
        ctx.lineTo(pts[i].x * scaleTotal, pts[i].y * scaleTotal);
      }
    }

    ctx.stroke();
    ctx.restore();
  }

  function renderSelectionRect(
    ctx: CanvasRenderingContext2D,
    rect: RectPayload,
    scaleTotal: number,
    selection: SelectionPayload
  ) {
    ctx.save();

    ctx.strokeStyle = selection.color;
    ctx.globalAlpha = selection.opacity;
    ctx.lineWidth = selection.width * scaleTotal;
    ctx.setLineDash([4 * scaleTotal, 4 * scaleTotal]);

    ctx.strokeRect(
      rect.x * scaleTotal,
      rect.y * scaleTotal,
      rect.width * scaleTotal,
      rect.height * scaleTotal
    );

    ctx.restore();
  }

  function renderSelectionLayer(
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: {
      selection: SelectionPayload;
      strokeLayer?: StrokeLayerPayload | null;
      dynamicStroke?: DynamicStrokePayload | null;
    }) {
    const { selection, strokeLayer, dynamicStroke } = payload;
    const scale = view.scale;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    if (strokeLayer && selection) {
      const strokes = Array.isArray(strokeLayer?.strokes) ? strokeLayer.strokes : [];

      for (const stroke of strokes) {
        if (selection.strokeIds.includes(stroke.id)) {
          renderSelectedStrokeHighlight(ctx, stroke, scale, selection);
        }
      }
    }

    if (selection && dynamicStroke?.selectionRect) {
      renderSelectionRect(ctx, dynamicStroke.selectionRect, scale, selection);
    }

    ctx.restore();
  }

  function renderDynamicStrokeLayer(
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: DynamicStrokePayload
  ) {
    const scale = view.scale;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    for (const stroke of payload.strokes) {
      renderDynamicStroke(ctx, stroke, scale);
    }

    ctx.restore();
  }

  function drawHover(
    ctx: CanvasRenderingContext2D,
    payload: HoverPayload | null,
    view: ViewPayload
  ) {
    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    if (!payload) {
      ctx.restore();
      return;
    }

    const scale = view.scale;

    ctx.strokeStyle = payload.color;
    ctx.globalAlpha = payload.opacity;
    ctx.lineWidth = payload.width * scale;
    ctx.lineCap = "round";
    ctx.lineJoin = "round";

    const pts = payload.points;

    ctx.beginPath();
    ctx.moveTo(pts[0].x * scale, pts[0].y * scale);

    for (let i = 1; i < pts.length; i++) {
      ctx.lineTo(pts[i].x * scale, pts[i].y * scale);
    }

    ctx.stroke();
    ctx.restore();
  }

  function renderHoverLayer(
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: HoverPayload | null
  ) {
    drawHover(ctx, payload, view);
  }

  async function renderWatermarkToContext(
    ctx: CanvasRenderingContext2D,
    payload: WatermarkPayload,
    view: ViewPayload,
    frontCtx: CanvasRenderingContext2D,
    width: number,
    height: number
  ): Promise<void> {

    if (payload.mode === WatermarkMode.Text) {
      if (payload.repeat)
        renderTextRepeat(ctx, payload, view, frontCtx, width, height);
      else
        renderTextSingle(ctx, payload, view, frontCtx, width, height);
      return;
    }

    if (payload.mode === WatermarkMode.Image) {
      if (payload.repeat)
        await renderImageRepeat(ctx, payload, view, width, height);
      else
        await renderImageSingle(ctx, payload, view, width, height);
      return;
    }

    if (payload.mode === WatermarkMode.Both) {
      if (payload.repeat) {
        await renderImageRepeat(ctx, payload, view, width, height);
        renderTextRepeat(ctx, payload, view, frontCtx, width, height);
      } else {
        await renderImageSingle(ctx, payload, view, width, height);
        renderTextSingle(ctx, payload, view, frontCtx, width, height);
      }
    }
  }

  function renderTextSingle(
    ctx: CanvasRenderingContext2D,
    payload: WatermarkPayload,
    view: ViewPayload,
    frontCtx: CanvasRenderingContext2D,
    width: number,
    height: number
  ): void {

    ctx.save();
    ctx.globalAlpha = payload.opacity ?? 1;

    ctx.font = `${payload.fontWeight} ${payload.fontSize}px ${payload.fontFamily}`;
    ctx.fillStyle = payload.color;
    ctx.textAlign = "center";
    ctx.textBaseline = "alphabetic";

    const { x, y } = computeAlignedPosition(payload, width, height);

    const metrics = frontCtx.measureText(payload.text);
    const ascent = metrics.actualBoundingBoxAscent;
    const descent = metrics.actualBoundingBoxDescent;
    const textHeight = ascent + descent;

    let globalX = x;
    let globalY = y + (textHeight / 2) - descent;

    globalY += -payload.fontSize * payload.visualBias;

    const localX = globalX - view.offsetX + payload.positionX;
    const localY = globalY - view.offsetY + payload.positionY;

    ctx.translate(localX, localY);
    ctx.rotate(payload.rotation * Math.PI / 180);
    ctx.scale(payload.scale, payload.scale);

    ctx.fillText(payload.text, 0, 0);

    ctx.restore();
  }

  function renderTextRepeat(
    ctx: CanvasRenderingContext2D,
    payload: WatermarkPayload,
    view: ViewPayload,
    frontCtx: CanvasRenderingContext2D,
    width: number,
    height: number
  ): void {

    ctx.save();
    ctx.globalAlpha = payload.opacity ?? 1;

    ctx.beginPath();
    ctx.rect(0, 0, width, height);
    ctx.clip();

    ctx.font = `${payload.fontWeight} ${payload.fontSize}px ${payload.fontFamily}`;
    ctx.fillStyle = payload.color;
    ctx.textAlign = "center";
    ctx.textBaseline = "alphabetic";

    const { x, y } = computeAlignedPosition(payload, width, height);

    const metrics = frontCtx.measureText(payload.text);
    const ascent = metrics.actualBoundingBoxAscent;
    const descent = metrics.actualBoundingBoxDescent;
    const textHeight = ascent + descent;

    let baseLocalX = x;
    let baseLocalY = y + (textHeight / 2) - descent;
    baseLocalY += -payload.fontSize * payload.visualBias;

    baseLocalX += payload.positionX;
    baseLocalY += payload.positionY;

    const stepX = payload.repeatSpacingX;
    const stepY = payload.repeatSpacingY;
    const startLocalX = baseLocalX % stepX - width;
    const startLocalY = baseLocalY % stepY - height;

    for (let lx = startLocalX; lx < width + stepX; lx += stepX) {
      for (let ly = startLocalY; ly < height + stepY; ly += stepY) {

        ctx.save();
        ctx.translate(lx, ly);
        ctx.rotate(payload.rotation * Math.PI / 180);
        ctx.scale(payload.scale, payload.scale);
        ctx.fillText(payload.text, 0, 0);
        ctx.restore();
      }
    }

    ctx.restore();
  }

  async function renderImageSingle(
    ctx: CanvasRenderingContext2D,
    payload: WatermarkPayload,
    view: ViewPayload,
    width: number,
    height: number
  ): Promise<void> {

    if (!payload.imageUrl) return;

    const img = await loadImage(payload.imageUrl);

    const { x, y } = computeAlignedPosition(payload, width, height);

    const localX = x - view.offsetX + payload.positionX;
    const localY = y - view.offsetY + payload.positionY;

    ctx.save();
    ctx.globalAlpha = payload.imageOpacity ?? payload.opacity ?? 1;

    ctx.translate(localX, localY);
    ctx.rotate(payload.rotation * Math.PI / 180);
    ctx.scale(payload.scale, payload.scale);

    ctx.drawImage(img, -img.width / 2, -img.height / 2);

    ctx.restore();
  }

  async function renderImageRepeat(
    ctx: CanvasRenderingContext2D,
    payload: WatermarkPayload,
    view: ViewPayload,
    width: number,
    height: number
  ): Promise<void> {

    if (!payload.imageUrl) return;

    const img = await loadImage(payload.imageUrl);

    const { x, y } = computeAlignedPosition(payload, width, height);

    const baseLocalX = x - view.offsetX + payload.positionX;
    const baseLocalY = y - view.offsetY + payload.positionY;

    for (let dx = 0; dx < width; dx += payload.repeatSpacingX) {
      for (let dy = 0; dy < height; dy += payload.repeatSpacingY) {

        const localX = baseLocalX + dx;
        const localY = baseLocalY + dy;

        ctx.save();
        ctx.globalAlpha = payload.imageOpacity ?? payload.opacity ?? 1;

        ctx.translate(localX, localY);
        ctx.rotate(payload.rotation * Math.PI / 180);
        ctx.scale(payload.scale, payload.scale);

        ctx.drawImage(img, -img.width / 2, -img.height / 2);

        ctx.restore();
      }
    }
  }

  function createWatermarkOffscreen(
    width: number,
    height: number,
    payload: WatermarkPayload,
    view: ViewPayload,
    frontCtx: CanvasRenderingContext2D
  ) {
    const offscreen = document.createElement("canvas");
    offscreen.width = width;
    offscreen.height = height;

    const ctx = offscreen.getContext("2d")!;
    ctx.save();

    renderWatermarkToContext(ctx, payload, view, frontCtx, width, height);

    ctx.restore();

    return offscreen;
  }

  async function renderWatermarkLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: WatermarkPayload
  ) {
    const { width, height } = getSurfaceSize(ctx);

    const key = makeWatermarkKey(canvasId, view, payload, width, height);
    if (key === "__invalid__") return;

    let offscreen = watermarkCache.get(key);

    if (!offscreen) {
      // On utilise ctx comme frontCtx pour measureText
      offscreen = createWatermarkOffscreen(width, height, payload, view, ctx);
      watermarkCache.set(key, offscreen);
    }

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.drawImage(offscreen, 0, 0);
    ctx.restore();
  }

  function renderDebugOverlay(
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    debug: DebugPayload,
    scaleTotal: number
  ) {
    ctx.save();

    const defaultFontFamily = "monospace";
    const defaultColor = "white";
    const defaultBgColor = "rgba(0, 0, 0, 0.5)";
    const defaultBgOpacity = 1;
    const fontFamily = debug.text?.fontFamily ?? defaultFontFamily;
    const fontSize = (debug.text?.fontSize ?? 12) * scaleTotal;
    const textColor = debug.text?.color ?? defaultColor;
    const bgColor = debug.background?.color ?? defaultBgColor;
    const bgOpacity = debug.background?.opacity ?? defaultBgOpacity;

    const padding = 8 * scaleTotal;

    const lines = [
      `DPI: ${debug.dpi}`,
      `Strokes: ${debug.strokeCount}`,
      `Points: ${debug.pointCount}`
    ];

    ctx.font = `${fontSize}px ${fontFamily}`;
    ctx.textBaseline = "top";

    let maxWidth = 0;

    for (const line of lines) {
      const w = ctx.measureText(line).width;
      if (w > maxWidth) maxWidth = w;
    }

    const lineSpacing = 4 * scaleTotal;
    const boxWidth = maxWidth + padding * 2;
    const boxHeight = lines.length * (fontSize + lineSpacing) + padding * 2;

    const x = view.offsetX + padding;
    const y = view.offsetY + padding;

    ctx.save();
    ctx.globalAlpha = bgOpacity;
    ctx.fillStyle = bgColor;
    ctx.fillRect(x, y, boxWidth, boxHeight);
    ctx.restore();

    ctx.fillStyle = textColor;

    let ty = y + padding;
    for (const line of lines) {
      ctx.fillText(line, x + padding, ty);
      ty += fontSize + lineSpacing;
    }

    ctx.restore();
  }

  function renderDebugLayer(
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: DebugPayload
  ) {
    renderDebugOverlay(ctx, view, payload, view.scale);
  }

  function renderEraserLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    view: ViewPayload,
    payload: EraserHoverPayload | null
  ) {
    if (!payload) return;

    const scale = view.scale;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);

    const x = payload.x * scale;
    const y = payload.y * scale;
    const r = payload.radius * scale;

    ctx.lineWidth = 1 * scale;
    ctx.strokeStyle = "rgba(0,0,0,0.6)";
    ctx.fillStyle = "rgba(0,0,0,0.1)";

    if (payload.softEdges) {
      ctx.shadowColor = "rgba(0,0,0,0.4)";
      ctx.shadowBlur = payload.softEdgeRadius * scale;
    }

    ctx.beginPath();
    if (payload.shape === EraserShape.Circle) {
      ctx.arc(x, y, r, 0, Math.PI * 2);
    } else {
      ctx.rect(x - r, y - r, r * 2, r * 2);
    }

    ctx.stroke();
    ctx.restore();
  }

  function renderLayer(
    canvasId: string,
    ctx: CanvasRenderingContext2D,
    key: string,
    payload: any,
    view: ViewPayload
  ) {
    switch (key) {
      case "background":
        return renderBackgroundLayer(canvasId, ctx, view, payload as BackgroundPayload);
      case "grid":
        return renderGridLayer(canvasId, ctx, view, payload as GridPayload);
      case "axes":
        return renderAxesLayer(canvasId, ctx, view, payload as AxesPayload);
      case "strokes":
        return renderStrokeLayer(canvasId, ctx, view, payload as StrokeLayerPayload);
      case "dynamic-stroke":
        return renderDynamicStrokeLayer(ctx, view, payload as DynamicStrokePayload);
      case "hover":
        return renderHoverLayer(ctx, view, payload as HoverPayload | null);
      case "selection":
        return renderSelectionLayer(ctx, view, payload as {
          selection: SelectionPayload;
          strokeLayer?: StrokeLayerPayload | null;
          dynamicStroke?: DynamicStrokePayload | null;
        });
      case "watermark":
        return renderWatermarkLayer(canvasId, ctx, view, payload as WatermarkPayload);
      case "debug":
        return renderDebugLayer(ctx, view, payload as DebugPayload);
      case "eraser":
        return renderEraserLayer(canvasId, ctx, view, payload as EraserHoverPayload);
    }
  }

  export function RenderCanvasFrame(canvasId: string, frame: CanvasFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.target;

    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);

    const view = frame.view;

    for (const key in frame.layers) {
      const payload = frame.layers[key];
      renderLayer(canvasId, ctx, key, payload, view);
    }
  }
}
