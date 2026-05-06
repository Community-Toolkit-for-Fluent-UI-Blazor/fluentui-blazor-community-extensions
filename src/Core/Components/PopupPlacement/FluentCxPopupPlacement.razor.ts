import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.PopupPlacement {

  export interface Size {
    width: number;
    height: number;
  }

  export interface Point {
    x: number;
    y: number;
  }

  export interface PlacementResult {
    position: Point;
    placement: PopupPlacement;
    popupSize: Size;
    anchorSize: Size;
  }

  enum PopupPlacement {
    Auto = 0,
    TopLeft = 1,
    TopCenter = 2,
    TopRight = 3,
    BottomLeft = 4,
    BottomCenter = 5,
    BottomRight = 6,
    LeftTop = 7,
    LeftCenter = 8,
    LeftBottom = 9,
    RightTop = 10,
    RightCenter = 11,
    RightBottom = 12,
  }

  enum AnchorLogicalPosition {
    TopLeft = 0,
    TopCenter = 1,
    TopRight = 2,
    MiddleLeft = 3,
    MiddleCenter = 4,
    MiddleRight = 5,
    BottomLeft = 6,
    BottomCenter = 7,
    BottomRight = 8,
  }

  enum PreferredPopupDirection {
    Default = 0,
    Left = 1,
    Right = 2,
    Up = 3,
    Down = 4,
  }

  interface PopupInstance {
    id: string;
    anchorId: string;
    popupId: string;

    placement: PopupPlacement;
    direction?: PreferredPopupDirection;
    anchorPosition?: AnchorLogicalPosition;

    dotNetHelper: DotNet.DotNetObject;

    resizeObserver: ResizeObserver | null;
    scrollHandler: ((e: Event) => void) | null;
    resizeHandler: ((e: UIEvent) => void) | null;

    boundaryRect?: Rect | null;
    gap: string;

    isRadial: boolean;
  }

  interface Rect {
    x: number;
    y: number;
    width: number;
    height: number;
  }

  const _instances: PopupInstance[] = [];
  let _dirtyInstances: PopupInstance[] = [];
  let _rafId: number | null = null;

  function scheduleUpdate(instance: PopupInstance): void {
    if (!_dirtyInstances.includes(instance)) {
      _dirtyInstances.push(instance);
    }

    if (_rafId === null) {
      _rafId = requestAnimationFrame(flushUpdates);
    }
  }

  function flushUpdates() {
    _rafId = null;
    const list = _dirtyInstances;
    _dirtyInstances = [];

    for (const inst of list) {
      doUpdatePosition(inst);
    }
  }

  function doUpdatePosition(instance: PopupInstance): void {
    const result = computePosition(instance);
    instance.dotNetHelper.invokeMethodAsync("OnPositionChanged", result);
  }

  export function Initialize(
    id: string,
    anchorId: string,
    popupId: string,
    placement: number,
    dotNetHelper: DotNet.DotNetObject,
    gap: string,
    isRadial: boolean
  ): void {

    const anchor = document.getElementById(anchorId);
    const popup = document.getElementById(popupId);
    if (!anchor || !popup) return;

    const instance: PopupInstance = {
      id,
      anchorId,
      popupId,
      placement: placement as PopupPlacement,
      dotNetHelper,
      resizeObserver: null,
      scrollHandler: null,
      resizeHandler: null,
      boundaryRect: null,
      gap,
      isRadial
    };

    _instances.push(instance);

    requestAnimationFrame(() => scheduleUpdate(instance));

    const ro = new ResizeObserver(() => scheduleUpdate(instance));
    ro.observe(anchor);
    ro.observe(popup);
    instance.resizeObserver = ro;

    const scrollHandler = () => scheduleUpdate(instance);
    const resizeHandler = () => scheduleUpdate(instance);

    window.addEventListener("scroll", scrollHandler, true);
    window.addEventListener("resize", resizeHandler);

    instance.scrollHandler = scrollHandler;
    instance.resizeHandler = resizeHandler;
  }

  export function Dispose(id: string): void {
    for (let i = _instances.length - 1; i >= 0; i--) {
      const inst = _instances[i];
      if (inst.id !== id) continue;

      if (inst.resizeObserver) inst.resizeObserver.disconnect();
      if (inst.scrollHandler)
        window.removeEventListener("scroll", inst.scrollHandler, true);
      if (inst.resizeHandler)
        window.removeEventListener("resize", inst.resizeHandler);

      _instances.splice(i, 1);
    }
  }

  export function UpdatePosition(id: string): void {
    const inst = _instances.find((x) => x.id === id);
    if (!inst) return;
    scheduleUpdate(inst);
  }

  export function UpdateDirection(id: string, direction: number): void {
    const inst = _instances.find((x) => x.id === id);
    if (!inst) return;

    inst.direction = direction as PreferredPopupDirection;
    scheduleUpdate(inst);
  }

  export function UpdateAnchorPosition(id: string, anchorPosition: number): void {
    const inst = _instances.find((x) => x.id === id);
    if (!inst) return;

    inst.anchorPosition = anchorPosition as AnchorLogicalPosition;
    scheduleUpdate(inst);
  }

  export function UpdatePlacement(id: string, placement: number): void {
    const inst = _instances.find((x) => x.id === id);
    if (!inst) return;

    inst.placement = placement as PopupPlacement;
    scheduleUpdate(inst);
  }

  export function UpdateRestrictedArea(id: string, restricted: boolean): void {
    const inst = _instances.find((x) => x.id === id);
    if (!inst) return;

    if (!restricted) {
      inst.boundaryRect = null;
    } else {
      const popup = document.getElementById(inst.popupId);
      inst.boundaryRect = popup?.parentElement?.getBoundingClientRect() ?? null;
    }

    scheduleUpdate(inst);
  }

  export function UpdateGap(id: string, gap: string): void {
    const inst = _instances.find(x => x.id === id);
    if (!inst) return;

    inst.gap = gap;
    scheduleUpdate(inst);
  }

  export function UpdateRadialMode(id: string, isRadial: boolean): void {
    const inst = _instances.find(x => x.id === id);
    if (!inst) return;

    inst.isRadial = isRadial;
    scheduleUpdate(inst);
  }

  function getRect(id: string): Rect {
    const el = document.getElementById(id);
    if (!el) return { x: 0, y: 0, width: 0, height: 0 };

    const r = el.getBoundingClientRect();
    return { x: r.left, y: r.top, width: r.width, height: r.height };
  }

  function parseOffset(offset: any): number {
    if (offset === null || offset === undefined) return 0;

    // If it's already a number → return it directly
    if (typeof offset === "number") {
      return isNaN(offset) ? 0 : offset;
    }

    // If it's not a string → convert to string
    if (typeof offset !== "string") {
      offset = String(offset);
    }

    const trimmed = offset.trim().toLowerCase();

    if (trimmed.length === 0)
      return 0;

    const match = trimmed.match(/^-?\d*\.?\d+/);
    if (!match)
      return 0;

    const value = parseFloat(match[0]);

    if (isNaN(value))
      return 0;

    const unit = trimmed.substring(match[0].length).trim();

    switch (unit) {
      case "":
      case "px":
        return value;

      case "rem":
      case "em":
        return value * 16;

      case "cm":
        return value * 37.7952755906;

      case "mm":
        return value * 3.77952755906;

      case "in":
        return value * 96;

      case "pt":
        return value * (96 / 72);

      case "pc":
        return value * 16;

      case "%":
        return value;

      default:
        return value;
    }
  }
  function computePlacementFromAnchorPosition(
    pos: AnchorLogicalPosition
  ): PopupPlacement {
    switch (pos) {
      case AnchorLogicalPosition.TopLeft:
      case AnchorLogicalPosition.TopCenter:
      case AnchorLogicalPosition.TopRight:
        return PopupPlacement.BottomCenter;

      case AnchorLogicalPosition.MiddleLeft:
        return PopupPlacement.RightCenter;

      case AnchorLogicalPosition.MiddleCenter:
        return PopupPlacement.TopCenter;

      case AnchorLogicalPosition.MiddleRight:
        return PopupPlacement.LeftCenter;

      case AnchorLogicalPosition.BottomLeft:
      case AnchorLogicalPosition.BottomCenter:
      case AnchorLogicalPosition.BottomRight:
        return PopupPlacement.TopCenter;
    }
    return PopupPlacement.BottomCenter;
  }

  function mapDirectionToPlacement(
    dir: PreferredPopupDirection
  ): PopupPlacement {
    switch (dir) {
      case PreferredPopupDirection.Left:
        return PopupPlacement.LeftCenter;
      case PreferredPopupDirection.Right:
        return PopupPlacement.RightCenter;
      case PreferredPopupDirection.Up:
        return PopupPlacement.TopCenter;
      case PreferredPopupDirection.Down:
        return PopupPlacement.BottomCenter;
    }
    return PopupPlacement.BottomCenter;
  }

  /*function computeRadialPosition(
    anchor: Rect,
    popupWidth: number,
    popupHeight: number
  ): Point {

    const centerX = anchor.x + anchor.width / 2;
    const centerY = anchor.y + anchor.height / 2;

    return {
      x: Math.round(centerX - popupWidth / 2),
      y: Math.round(centerY - popupHeight / 2)
    };
  }*/

  function computeRadialPosition(
    anchor: Rect
  ): Point {

    const centerX = anchor.x + anchor.width / 2;
    const centerY = anchor.y + anchor.height / 2;

    return {
      x: Math.round(centerX),
      y: Math.round(centerY)
    };
  }


  function getOppositePlacement(p: PopupPlacement): PopupPlacement {
    switch (p) {
      case PopupPlacement.TopCenter:
        return PopupPlacement.BottomCenter;
      case PopupPlacement.BottomCenter:
        return PopupPlacement.TopCenter;
      case PopupPlacement.LeftCenter:
        return PopupPlacement.RightCenter;
      case PopupPlacement.RightCenter:
        return PopupPlacement.LeftCenter;
      default:
        return p;
    }
  }

  function getPerpendicularPlacements(p: PopupPlacement): PopupPlacement[] {
    switch (p) {
      case PopupPlacement.TopCenter:
      case PopupPlacement.BottomCenter:
        return [PopupPlacement.LeftCenter, PopupPlacement.RightCenter];

      case PopupPlacement.LeftCenter:
      case PopupPlacement.RightCenter:
        return [PopupPlacement.TopCenter, PopupPlacement.BottomCenter];

      default:
        return [PopupPlacement.TopCenter, PopupPlacement.BottomCenter];
    }
  }

  function canPlace(
    anchor: Rect,
    w: number,
    h: number,
    placement: PopupPlacement,
    boundary: Rect | undefined,
    gapPx: number
  ): boolean {
    const pos = computePrimaryPlacement(anchor, w, h, placement, gapPx);

    const bx = boundary?.x ?? 0;
    const by = boundary?.y ?? 0;
    const bw = boundary?.width ?? window.innerWidth;
    const bh = boundary?.height ?? window.innerHeight;

    return (
      pos.x >= bx && pos.y >= by && pos.x + w <= bx + bw && pos.y + h <= by + bh
    );
  }

  function computeFallbackPlacement(
    anchor: Rect,
    w: number,
    h: number,
    anchorPos: AnchorLogicalPosition | undefined,
    boundary: Rect | undefined,
    gapPx: number
  ): PopupPlacement {
    let optimal: PopupPlacement;

    if (anchorPos !== undefined) {
      optimal = computePlacementFromAnchorPosition(anchorPos);
    } else {
      optimal = PopupPlacement.BottomCenter;
    }

    if (canPlace(anchor, w, h, optimal, boundary, gapPx)) {
      return optimal;
    }

    const candidates = [
      PopupPlacement.TopCenter,
      PopupPlacement.BottomCenter,
      PopupPlacement.LeftCenter,
      PopupPlacement.RightCenter,
    ];

    for (const p of candidates) {
      if (canPlace(anchor, w, h, p, boundary, gapPx)) return p;
    }

    return PopupPlacement.BottomCenter;
  }

  function computeFlipPlacement(
    anchor: Rect,
    w: number,
    h: number,
    desired: PopupPlacement,
    anchorPos: AnchorLogicalPosition | undefined,
    boundary: Rect | undefined,
    gapPx: number
  ): PopupPlacement {
    if (canPlace(anchor, w, h, desired, boundary, gapPx)) {
      return desired;
    }

    const opposite = getOppositePlacement(desired);

    if (canPlace(anchor, w, h, opposite, boundary, gapPx)) {
      return opposite;
    }

    const perpendiculars = getPerpendicularPlacements(desired);

    for (const p of perpendiculars) {
      if (canPlace(anchor, w, h, p, boundary, gapPx)) {
        return p;
      }
    }

    return computeFallbackPlacement(anchor, w, h, anchorPos, boundary, gapPx);
  }

  function computePrimaryPlacement(
    anchor: Rect,
    w: number,
    h: number,
    placement: PopupPlacement,
    gapPx: number
  ): Point {
    const pad = gapPx;

    switch (placement) {
      case PopupPlacement.TopLeft:
        return { x: anchor.x, y: anchor.y - h - pad };

      case PopupPlacement.TopCenter:
        return {
          x: anchor.x + anchor.width / 2 - w / 2,
          y: anchor.y - h - pad,
        };

      case PopupPlacement.TopRight:
        return {
          x: anchor.x + anchor.width - w,
          y: anchor.y - h - pad
        };

      case PopupPlacement.BottomLeft:
        return { x: anchor.x, y: anchor.y + anchor.height + pad };

      case PopupPlacement.BottomCenter:
        return {
          x: anchor.x + anchor.width / 2 - w / 2,
          y: anchor.y + anchor.height + pad,
        };

      case PopupPlacement.BottomRight:
        return {
          x: anchor.x + anchor.width - w,
          y: anchor.y + anchor.height + pad,
        };

      case PopupPlacement.LeftTop:
        return { x: anchor.x - w - pad, y: anchor.y };

      case PopupPlacement.LeftCenter:
        return {
          x: anchor.x - w - pad,
          y: anchor.y + anchor.height / 2 - h / 2,
        };

      case PopupPlacement.LeftBottom:
        return {
          x: anchor.x - w - pad,
          y: anchor.y + anchor.height - h,
        };

      case PopupPlacement.RightTop:
        return { x: anchor.x + anchor.width + pad, y: anchor.y };

      case PopupPlacement.RightCenter:
        return {
          x: anchor.x + anchor.width + pad,
          y: anchor.y + anchor.height / 2 - h / 2,
        };

      case PopupPlacement.RightBottom:
        return {
          x: anchor.x + anchor.width + pad,
          y: anchor.y + anchor.height - h,
        };
    }

    return { x: anchor.x, y: anchor.y };
  }

  function computePosition(instance: PopupInstance): PlacementResult {
    const anchor = getRect(instance.anchorId);
    const popupRect = getRect(instance.popupId);

    const w = popupRect.width;
    const h = popupRect.height;

    const gapPx = parseOffset(instance.gap);

    const boundary = instance.boundaryRect ?? undefined;

    if (instance.isRadial) {
      const pos = computeRadialPosition(anchor);

      return {
        position: pos,
        placement: PopupPlacement.Auto,
        popupSize: { width: w, height: h },
        anchorSize: { width: anchor.width, height: anchor.height }
      };
    }

    let placement: PopupPlacement;

    if (
      instance.direction !== undefined &&
      instance.direction !== PreferredPopupDirection.Default
    ) {
      const desired = mapDirectionToPlacement(instance.direction);
      placement = computeFlipPlacement(
        anchor,
        w,
        h,
        desired,
        instance.anchorPosition,
        boundary,
        gapPx
      );
    } else if (instance.placement === PopupPlacement.Auto) {
      const desired =
        instance.anchorPosition !== undefined
          ? computePlacementFromAnchorPosition(instance.anchorPosition)
          : PopupPlacement.BottomCenter;

      placement = computeFlipPlacement(
        anchor,
        w,
        h,
        desired,
        instance.anchorPosition,
        boundary,
        gapPx
      );
    } else {
      const desired = instance.placement;
      placement = computeFlipPlacement(
        anchor,
        w,
        h,
        desired,
        instance.anchorPosition,
        boundary,
        gapPx
      );
    }

    const pos = computePrimaryPlacement(anchor, w, h, placement, gapPx);

    return {
      position: { x: Math.round(pos.x), y: Math.round(pos.y) },
      placement,
      popupSize: { width: w, height: h },
      anchorSize: { width: anchor.width, height: anchor.height },
    };
  }
}
