export namespace FluentUI.Blazor.Community.Signature {

  interface SurfaceCanvases {
    staticBack: CanvasRenderingContext2D;
    dynamic: CanvasRenderingContext2D;
    staticFront: CanvasRenderingContext2D;
    debug: CanvasRenderingContext2D;
    watermark: CanvasRenderingContext2D;
    hover: CanvasRenderingContext2D;
    target: CanvasRenderingContext2D;
  }

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
    dashArray: number[];
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

  interface StaticDirtyFlags {
    background: boolean;
    grid: boolean;
    axes: boolean;
    watermark: boolean;
    hover: boolean;
  }

  interface DynamicDirtyFlags {
    strokeLayer: boolean;
    dynamicStroke: boolean;
    selection: boolean;
    debug: boolean;
  }

  interface StaticFramePayload {
    view: ViewPayload;
    background?: BackgroundPayload | null;
    grid?: GridPayload | null;
    axes?: AxesPayload | null;
    watermark?: WatermarkPayload | null;
    hover?: HoverPayload | null;
    dirty: StaticDirtyFlags;
  }

  interface DynamicFramePayload {
    view: ViewPayload;
    strokeLayer?: StrokeLayerPayload | null;
    dynamicStroke?: DynamicStrokePayload | null;
    selection?: SelectionPayload | null;
    debug?: DebugPayload | null;
    dirty: DynamicDirtyFlags;
  }

  interface FramePayload {
    staticBack: StaticFramePayload;
    staticFront: StaticFramePayload;
    dynamic: DynamicFramePayload;
    debug: DynamicFramePayload;
    watermark: StaticFramePayload;
    hover: StaticFramePayload;
  }


  const surfacesCache = new Map<string, SurfaceCanvases>();
  const imageCache = new Map<string, HTMLImageElement>();
  const strokeLayerCache = new Map<string, HTMLCanvasElement>();
  const backgroundCache = new Map<string, HTMLCanvasElement>();
  const gridCache = new Map<string, HTMLCanvasElement>();
  const axesCache = new Map<string, HTMLCanvasElement>();
  const watermarkCache = new Map<string, HTMLCanvasElement>();

  export function GetPointerPosition(
    id: string,
    clientX: number,
    clientY: number,
    view: ViewPayload) {
    const surfaces = surfacesCache.get(id);

    if (!surfaces || !surfaces.target) {
      return { x: clientX, y: clientY };
    }

    const rect = surfaces.target.canvas.getBoundingClientRect();
    const xCanvas = clientX - rect.left;
    const yCanvas = clientY - rect.top;

    const xSurface = (xCanvas - view.offsetX) / view.scale;
    const ySurface = (yCanvas - view.offsetY) / view.scale;

    return {
      x: xSurface,
      y: ySurface
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

  export function ResizeOffscreens(id: string, w: number, h: number) {
    const surfaces = surfacesCache.get(id);
    if (!surfaces) return;

    setOffscreenSize(surfaces.staticBack, w, h);
    setOffscreenSize(surfaces.staticFront, w, h);
    setOffscreenSize(surfaces.dynamic, w, h);
    setOffscreenSize(surfaces.debug, w, h);
    setOffscreenSize(surfaces.watermark, w, h);
    setOffscreenSize(surfaces.hover, w, h);
  }

  export function RegisterCanvas(canvasId: string, isOffscreen: boolean) {
    let canvas: HTMLCanvasElement;

    if (isOffscreen) {
      canvas = document.createElement("canvas");
    }
    else {
      canvas = document.getElementById(canvasId) as HTMLCanvasElement;
    }

    if (!canvas) {
      console.error("Canvas not found:", canvasId);
      return;
    }

    const rect = canvas.getBoundingClientRect();
    canvas.width = rect.width;
    canvas.height = rect.height;

    const targetCtx = canvas.getContext("2d")!;
    targetCtx.transform(1, 0, 0, 1, 0, 0);

    const makeOffscreen = () => {
      const off = document.createElement("canvas");
      off.width = canvas.width;
      off.height = canvas.height;
      const ctx = off.getContext("2d")!;
      targetCtx.transform(1, 0, 0, 1, 0, 0);
      return ctx;
    };

    surfacesCache.set(canvasId, {
      target: targetCtx,
      staticBack: makeOffscreen(),
      staticFront: makeOffscreen(),
      dynamic: makeOffscreen(),
      debug: makeOffscreen(),
      watermark: makeOffscreen(),
      hover: makeOffscreen()
    });
  }
  function drawHover(canvasId: string, payload: HoverPayload | null, view: ViewPayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.hover;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);
    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    ctx.clearRect(0, 0, view.renderWidth, view.renderHeight);

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


  export function DrawStrokeSegment(
    canvasId: string,
    payload: {
      p1: { x: number; y: number, width: number };
      p2: { x: number; y: number, width: number };
      pen: PenPayload;
      blendMode: GlobalCompositeOperation;
    },
    view: ViewPayload
  ) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.dynamic;
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

    composeFinal(canvasId);
  }

  function setOffscreenSize(ctx: CanvasRenderingContext2D, w: number, h: number) {
    ctx.canvas.width = w;
    ctx.canvas.height = h;
  }

  function getSurfaceSize(ctx: CanvasRenderingContext2D) {
    return {
      width: ctx.canvas.width,
      height: ctx.canvas.height
    };
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
    layer: StrokeLayerPayload
  ): string {
    const scale = view.scale;

    const w = Math.round(view.renderWidth * scale);
    const h = Math.round(view.renderHeight * scale);

    if (w <= 0 || h <= 0) return "__invalid__";

    const hashParts = layer.strokes.map(s =>
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

    return [
      canvasId,
      payload.color,
      payload.opacity,
      payload.strokeWidth,
      payload.dashArray.join(","),
      w,
      h
    ].join("|");
  }

  function drawBackground(canvasId: string, view: ViewPayload, payload: BackgroundPayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.staticBack;
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

    ctx.drawImage(offscreen, view.offsetX, view.offsetY);
  }

  function drawGrid(canvasId: string, view: ViewPayload, payload: GridPayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx =
      payload.layer === GridLayer.Background
        ? surfaces.staticBack
        : surfaces.staticFront;

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

    ctx.drawImage(offscreen, view.offsetX, view.offsetY);
  }

  function drawAxes(canvasId: string, view: ViewPayload, payload: AxesPayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const key = makeAxesKey(canvasId, view, payload);
    if (key === "__invalid__") return;

    const ctx =
      payload.layer === GridLayer.Background
        ? surfaces.staticBack
        : surfaces.staticFront;

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

      if (payload.dashArray.length > 0) {
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

    ctx.drawImage(offscreen, view.offsetX, view.offsetY);
  }

  async function drawWatermark(canvasId: string, view: ViewPayload, payload: WatermarkPayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.watermark;
    const { width, height } = getSurfaceSize(ctx);

    const key = makeWatermarkKey(canvasId, view, payload, width, height);
    if (key === "__invalid__") return;

    let offscreen = watermarkCache.get(key);

    if (!offscreen) {
      offscreen = createWatermarkOffscreen(width, height, payload, view, ctx);
      watermarkCache.set(key, offscreen);
    }

    ctx.drawImage(offscreen, view.offsetX, view.offsetY);
  }

  function createWatermarkOffscreen(width: number, height: number, payload: WatermarkPayload, view: ViewPayload, frontCtx: CanvasRenderingContext2D) {
    const offscreen = document.createElement("canvas");
    offscreen.width = width;
    offscreen.height = height;

    const ctx = offscreen.getContext("2d")!;
    ctx.save();

    renderWatermarkToContext(ctx, payload, view, frontCtx, width, height);

    ctx.restore();

    return offscreen;
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

  function renderGridFrame(canvasId: string, frame: StaticFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) {
      return;
    }

    if (frame.grid) {
      drawGrid(canvasId, frame.view, frame.grid);
    }
  }

  function renderAxesFrame(canvasId: string, frame: StaticFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) {
      return;
    }

    if (frame.axes) {
      drawAxes(canvasId, frame.view, frame.axes);
    }
  }

  function renderStrokeLayerCached(
    canvasId: string,
    view: ViewPayload,
    layer: StrokeLayerPayload,
    ctx: CanvasRenderingContext2D
  ) {
    const key = makeStrokeLayerKey(canvasId, view, layer);
    if (key === "__invalid__") return;

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

      for (const stroke of layer.strokes) {
        renderDynamicStroke(offctx, stroke, scale);
      }

      offctx.restore();

      strokeLayerCache.set(key, offscreen);
    }

    ctx.drawImage(offscreen, 0, 0);
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
  function renderDynamicFrame(
    canvasId: string,
    frame: DynamicFramePayload
  ) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.dynamic;

    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);

    const view = frame.view;
    const scale = view.scale;

    ctx.save();
    ctx.translate(view.offsetX, view.offsetY);

    ctx.beginPath();
    ctx.rect(0, 0, view.renderWidth, view.renderHeight);
    ctx.clip();

    if (frame.strokeLayer) {
      renderStrokeLayerCached(canvasId, view, frame.strokeLayer, ctx);
    }

    if (frame.selection) {
      for (const stroke of frame.strokeLayer?.strokes ?? []) {
        if (frame.selection.strokeIds.includes(stroke.id)) {
          renderSelectedStrokeHighlight(ctx, stroke, scale, frame.selection);
        }
      }
    }
    
    if (frame.dynamicStroke) {
      for (const stroke of frame.dynamicStroke.strokes) {
        renderDynamicStroke(ctx, stroke, scale);
      }
    }

    if (frame.selection && frame.dynamicStroke?.selectionRect) {
      renderSelectionRect(
        ctx,
        frame.dynamicStroke.selectionRect,
        scale,
        frame.selection
      );
    }

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


  function renderStaticBackFrame(
    canvasId: string,
    frame: StaticFramePayload
  ) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.staticBack;

    const mustRedraw =
      (frame.dirty.background && frame.background) ||
      (frame.dirty.grid && frame.grid && frame.grid.layer === GridLayer.Background) ||
      (frame.dirty.axes && frame.axes && frame.axes.layer === GridLayer.Background);

    if (mustRedraw) {
      ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }

    const view = frame.view;

    if (frame.dirty.background && frame.background) {
      drawBackground(canvasId, view, frame.background);
    }

    if (frame.dirty.grid && frame.grid && frame.grid.layer === GridLayer.Background) {
      renderGridFrame(canvasId, frame);
    }

    if (frame.dirty.axes && frame.axes && frame.axes.layer === GridLayer.Background) {
      renderAxesFrame(canvasId, frame);
    }
  }

  function renderStaticFrontFrame(
    canvasId: string,
    frame: StaticFramePayload
  ) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.staticFront;

    const mustRedraw =
      (frame.dirty.grid && frame.grid && frame.grid.layer === GridLayer.Foreground) ||
      (frame.dirty.axes && frame.axes && frame.axes.layer === GridLayer.Foreground) ||
      (frame.dirty.watermark && frame.watermark);

    if (mustRedraw) {
      ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }


    const view = frame.view;

    if (frame.dirty.grid && frame.grid && frame.grid.layer === GridLayer.Foreground) {
      drawGrid(canvasId, view, frame.grid);
    }

    if (frame.dirty.axes && frame.axes && frame.axes.layer === GridLayer.Foreground) {
      renderAxesFrame(canvasId, frame);
    }
  }

  function renderDebugFrame(canvasId: string, frame: DynamicFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.debug;
    if (!ctx) return;

    const { width, height } = getSurfaceSize(ctx);
    ctx.clearRect(0, 0, width, height);

    if (frame.debug) {
      renderDebugOverlay(ctx, frame.view, frame.debug, frame.view.scale);
    }
  }

  function renderHoverFrame(canvasId: string, frame: StaticFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.watermark;

    const mustRedraw = frame.dirty.hover;

    if (mustRedraw) {
      ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    }

    const view = frame.view;

    if (frame.dirty.hover) {
      drawHover(canvasId, frame.hover ?? null, view);
    }
  }

  function renderWatermarkFrame(canvasId: string, frame: StaticFramePayload) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const ctx = surfaces.watermark;

    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);

    if (frame.watermark) {
      drawWatermark(canvasId, frame.view, frame.watermark);
    }
  }

  function composeFinal(canvasId: string) {
    const surfaces = surfacesCache.get(canvasId);
    if (!surfaces) return;

    const target = surfaces.target;
    const ctx = target;

    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
    ctx.drawImage(surfaces.staticBack.canvas, 0, 0);
    ctx.drawImage(surfaces.dynamic.canvas, 0, 0);
    ctx.drawImage(surfaces.staticFront.canvas, 0, 0);
    ctx.drawImage(surfaces.hover.canvas, 0, 0);
    ctx.drawImage(surfaces.watermark.canvas, 0, 0);
    ctx.drawImage(surfaces.debug.canvas, 0, 0);
  }

  export function RenderFrame(canvasId: string, frame: FramePayload) {
    renderStaticBackFrame(canvasId, frame.staticBack);
    renderDynamicFrame(canvasId, frame.dynamic);
    renderStaticFrontFrame(canvasId, frame.staticFront);
    renderHoverFrame(canvasId, frame.hover);
    renderWatermarkFrame(canvasId, frame.watermark);
    renderDebugFrame(canvasId, frame.debug);
    composeFinal(canvasId);
  }
}
