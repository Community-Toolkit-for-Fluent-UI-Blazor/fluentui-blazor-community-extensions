import type { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Scheduler {
  export const enum SchedulerView {
    Day = 0,
    Week = 1,
    Month = 2,
    Timeline = 4
  }

  export const enum PointerMode {
    Drag = 0,
    Resize = 1
  }

  interface ActivePointer {
    dotNetRef: DotNet.DotNetObject;
    itemId: string | number;
    mode: PointerMode;
  }

  type MaybeElement = HTMLElement | string | { id?: string } | null | undefined;

  let _dotNet: DotNet.DotNetObject | null = null;
  let _container: HTMLElement | null = null;
  let _selector: string | null = null;
  let _resizeObserver: ResizeObserver | null = null;
  let _active: ActivePointer | null = null;

  function resolveElement(maybeElOrSelector: MaybeElement): HTMLElement | null {
    if (maybeElOrSelector && maybeElOrSelector instanceof HTMLElement) {
      return maybeElOrSelector;
    }

    if (typeof maybeElOrSelector === "string") {
      const bySelector = document.querySelector(maybeElOrSelector);
      if (bySelector instanceof HTMLElement) {
        return bySelector;
      }

      const byId = document.getElementById(maybeElOrSelector);
      if (byId) {
        return byId;
      }

      return null;
    }

    if (maybeElOrSelector && typeof maybeElOrSelector === "object") {
      const anyObj = maybeElOrSelector as { id?: string };
      if (anyObj.id && typeof anyObj.id === "string") {
        const byId = document.getElementById(anyObj.id);
        if (byId) {
          return byId;
        }
      }

      return null;
    }

    return null;
  }

  export function InitDrop(containerOrSelector: MaybeElement, selector?: string): void {
    const container = resolveElement(containerOrSelector) || document.body;
    _container = container;
    _selector = selector || ".scheduler-content";

    container.addEventListener("dragover", (ev: DragEvent) => {
      ev.preventDefault();
    });
  }

  export function DisposeDrop(): void {
    if (_container) {
      _container.removeEventListener("dragover", () => { /* noop */ });
    }
  }

  export function ObserveResize(
    containerOrSelector: MaybeElement,
    selector: string,
    dotNetRef: DotNet.DotNetObject
  ): void {
    const container = resolveElement(containerOrSelector) || document.body;
    _container = container;
    _selector = selector || ".scheduler-content";
    _dotNet = dotNetRef;

    const content =
      (container.querySelector(_selector) as HTMLElement | null) ?? container;

    if (!content) {
      return;
    }

    var win = window;

    if ("ResizeObserver" in win) {
      _resizeObserver = new ResizeObserver(() => {
        dotNetRef.invokeMethodAsync("Scheduler_OnContentResize").catch(() => { });
      });
      _resizeObserver.observe(content);
    } else {
      window.addEventListener("resize", () => {
        dotNetRef.invokeMethodAsync("Scheduler_OnContentResize").catch(() => { });
      });
    }
  }

  export function DisposeObserve(): void {
    if (_resizeObserver) {
      _resizeObserver.disconnect();
      _resizeObserver = null;
    }

    _dotNet = null;
    _container = null;
    _selector = null;
  }

  export function GetBoundingClientRect(containerOrElement: MaybeElement): DOMRect | null {
    const container = resolveElement(containerOrElement) || document.body;
    if (!container) return null;
    return container.getBoundingClientRect();
  }

  export function GetTimelineRect(containerOrElement: MaybeElement): DOMRect | null {
    const container = resolveElement(containerOrElement) || document.body;
    if (!container) return null;
    return container.getBoundingClientRect();
  }

  export function GetMonthGridRect(containerOrElement: MaybeElement): DOMRect | null {
    const container = resolveElement(containerOrElement) || document.body;
    if (!container) return null;

    const grid = container.querySelector(".scheduler-month-grid") as HTMLElement | null;
    if (!grid) return null;

    return grid.getBoundingClientRect();
  }

  export async function MeasureLayout(
    container: MaybeElement,
    selector: string,
    view: SchedulerView) {
    const first = await MeasureLayoutInternal(container, selector, view);

    await new Promise(r => requestAnimationFrame(r));

    const second = await MeasureLayoutInternal(container, selector, view);

    if (Math.abs(first.overlay.height - second.overlay.height) > 0.5 ||
      Math.abs(first.overlay.width - second.overlay.width) > 0.5) {
      return second;
    }

    return first;
  }

  async function MeasureLayoutInternal(
    containerOrElement: MaybeElement,
    selector: string,
    view: SchedulerView
  ): Promise<{
    overlay: { x: number; y: number; width: number; height: number };
    cellSize: { width: number; height: number };
    labelSize: { width: number; height: number };
    contentSize: { x: number; y: number; width: number; height: number };
    padding: { top: number; bottom: number; left: number; right: number };
    gap: number;
    headerHeight: number;
    usableHeight: number;
    local: { x: number; y: number };
  }> {
    return new Promise(resolve => {
      const container = resolveElement(containerOrElement) || document.body;
      const selectorStr = selector || ".scheduler-content";
      const content =
        (container.querySelector(selectorStr) as HTMLElement | null) ?? container;

      const contRect = container.getBoundingClientRect();
      const contScrollLeft = container.scrollLeft || 0;
      const contScrollTop = container.scrollTop || 0;

      let contentRect = content.getBoundingClientRect();

      const localLeft = contentRect.left - contRect.left + contScrollLeft;
      const localTop = contentRect.top - contRect.top + contScrollTop;

      let labelWidth = 0;
      let headerHeight = 0;
      let cellWidth = 0;
      let cellHeight = 0;
      let gap = 0;
      let labelHeight = 0;
      let usableHeight = 0;
      let paddingTop = 0;
      let paddingBottom = 0;
      let paddingLeft = 0;
      let paddingRight = 0;

      if (view === SchedulerView.Day) {
        labelWidth = 135;
      } else if (view === SchedulerView.Month) {
        const grid = container.querySelector(".scheduler-month-grid") as HTMLElement | null;
        const firstCell = grid?.querySelector(".scheduler-day-cell") as HTMLElement | null;
        const label = firstCell?.querySelector(".scheduler-day-label") as HTMLElement | null;

        if (grid && firstCell && label) {
          const gridStyle = window.getComputedStyle(grid);
          gap = parseFloat(gridStyle.gap || gridStyle.rowGap || "0");

          const cellRect = firstCell.getBoundingClientRect();
          const labelRect = label.getBoundingClientRect();
          const labelStyle = window.getComputedStyle(label);
          const cellStyle = window.getComputedStyle(firstCell);

          const marginTop = parseFloat(labelStyle.marginTop || "0");
          const marginBottom = parseFloat(labelStyle.marginBottom || "0");
          paddingTop = parseFloat(cellStyle.paddingTop || "0");
          paddingBottom = parseFloat(cellStyle.paddingBottom || "0");
          paddingLeft = parseFloat(cellStyle.paddingLeft || "0");
          paddingRight = parseFloat(cellStyle.paddingRight || "0");

          labelHeight = labelRect.height + marginTop + marginBottom + paddingTop;
          cellHeight = cellRect.height;
          usableHeight = cellHeight - labelHeight - paddingBottom;
          cellWidth = cellRect.width;
        }
      } else if (view === SchedulerView.Week) {
        const grid = container.querySelector(".scheduler-week-grid") as HTMLElement | null;
        const firstCell = grid?.querySelector(".scheduler-week-day") as HTMLElement | null;
        const labelCell = grid?.querySelector(".scheduler-week-label") as HTMLElement | null;
        const header = container.querySelector(".scheduler-week-header") as HTMLElement | null;

        if (grid && firstCell && labelCell && header) {
          const headerRect = header.getBoundingClientRect();
          const cellRect = firstCell.getBoundingClientRect();
          const labelRect = labelCell.getBoundingClientRect();
          const cellStyle = window.getComputedStyle(firstCell);

          paddingTop = parseFloat(cellStyle.paddingTop || "0");
          paddingBottom = parseFloat(cellStyle.paddingBottom || "0");
          paddingLeft = parseFloat(cellStyle.paddingLeft || "0");
          paddingRight = parseFloat(cellStyle.paddingRight || "0");

          labelWidth = labelRect.width * 2;
          cellWidth = cellRect.width;
          cellHeight = cellRect.height;
          headerHeight = headerRect.height;
        }
      } else if (view === SchedulerView.Timeline) {
        const header = container.querySelector(".scheduler-timeline-full-header") as HTMLElement | null;
        if (header) {
          const headerRect = header.getBoundingClientRect();
          headerHeight = headerRect.height;
        }

        labelWidth = 0;

        const firstCell = container.querySelector(".scheduler-timeline-hour") as HTMLElement | null;
        if (firstCell) {
          const cellRect = firstCell.getBoundingClientRect();
          cellWidth = cellRect.width;
          cellHeight = cellRect.height;
        }
      }

      requestAnimationFrame(() => {
        const body = container.querySelector(".scheduler-timeline") as HTMLElement | null;
        if (body) {
          const gridWidthAttr = parseFloat(body.style.width || "0");
          const gridScrollWidth = body.scrollWidth;
          const realGridWidth = Math.max(gridWidthAttr, gridScrollWidth);
          contentRect.width = realGridWidth;
        }

        const overlayLeft = view === SchedulerView.Timeline ? 0 : localLeft + labelWidth;
        const overlayTop = localTop + headerHeight;
        const overlayWidth = Math.max(0, contentRect.width - labelWidth);
        const overlayHeight = contentRect.height - headerHeight;

        resolve({
          overlay: {
            x: overlayLeft,
            y: overlayTop,
            width: overlayWidth,
            height: overlayHeight
          },
          cellSize: {
            width: cellWidth,
            height: cellHeight
          },
          labelSize: {
            width: labelWidth,
            height: labelHeight
          },
          contentSize: {
            x: contRect.left,
            y: contRect.top,
            width: contRect.width,
            height: contRect.height
          },
          padding: {
            top: paddingTop,
            bottom: paddingBottom,
            left: paddingLeft,
            right: paddingRight
          },
          gap,
          headerHeight,
          usableHeight,
          local: {
            x: localLeft,
            y: localTop
          }
        });
      });
    });
  }

  export function StartPointerTracking(
    dotNetRef: DotNet.DotNetObject,
    itemId: string | number,
    mode: PointerMode
  ): void {
    _active = { dotNetRef, itemId, mode };

    function moveHandler(e: PointerEvent) {
      if (!_active) return;

      e.preventDefault();

      _active.dotNetRef
        .invokeMethodAsync(
          "OnPointerMove",
          _active.itemId,
          _active.mode,
          e.clientX,
          e.clientY,
          e.pointerType
        )
        .catch(() => { });
    }

    function upHandler(e: PointerEvent) {
      if (!_active) return;

      _active.dotNetRef
        .invokeMethodAsync(
          "OnPointerUp",
          _active.itemId,
          _active.mode,
          e.pointerType
        )
        .catch(() => { });

      document.removeEventListener("pointermove", moveHandler);
      document.removeEventListener("pointerup", upHandler);
      _active = null;
    }

    document.addEventListener("pointermove", moveHandler);
    document.addEventListener("pointerup", upHandler);
  }

  export function StopPointerTracking(): void {
    _active = null;
  }
  export function GetScrollLeft(containerOrElement: MaybeElement): number {
    const container = resolveElement(containerOrElement) || document.body;
    if (!container) return 0;
    return container.scrollLeft || 0;
  }
}
