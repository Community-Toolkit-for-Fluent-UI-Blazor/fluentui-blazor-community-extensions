import { DotNet } from "../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Charts {
  interface Chart {
    resizeObserver: ResizeObserver;
    intersectionObserver: IntersectionObserver;
    element: HTMLElement;
    dotNetHelper: DotNet.DotNetObject;
    handlers: {
      pointerenter: (e: PointerEvent) => void;
      pointerleave: (e: PointerEvent) => void;
      pointermove: (e: PointerEvent) => void;
      pointerdown: (e: PointerEvent) => void;
      pointerup: (e: PointerEvent) => void;
      click: (e: PointerEvent) => void;
    };
  }

  const _instances = new Map<string, Chart>();

  function findSizedParent(el: HTMLElement): HTMLElement {
    let current: HTMLElement | null = el;

    while (current) {
      const style = getComputedStyle(current);

      const hasExplicitHeight =
        current.style.height ||
        current.style.minHeight ||
        current.style.maxHeight;

      const isFlexChild =
        style.display === "flex" && style.flexDirection !== "row";

      const isGridChild =
        style.display === "grid";

      const isScrollable =
        style.overflow !== "visible";

      if (hasExplicitHeight || isFlexChild || isGridChild || isScrollable) {
        return current;
      }

      current = current.parentElement;
    }

    return el;
  }


  export function Initialize(id: string, visibilityThreshold: number, dotNetHelper: DotNet.DotNetObject) {
    const element = document.getElementById(id);
    if (!element) return;

    const resizeObserver = new ResizeObserver(entries => {
      for (let entry of entries) {
        const width = entry.contentRect.width;
        const height = entry.contentRect.height;
        dotNetHelper.invokeMethodAsync("OnResize", { width, height });
      }
    });

    const container = findSizedParent(element);
    resizeObserver.observe(container);

    const intersectionObserver = new IntersectionObserver(entries => {
      for (let entry of entries) {
        const visible = entry.isIntersecting;
        dotNetHelper.invokeMethodAsync("OnVisibilityChanged", visible);
      }
    }, { root: null, threshold: visibilityThreshold });

    intersectionObserver.observe(element);

    const makeHandler = (method: string, root: HTMLElement) => (e: PointerEvent) => {
      const path = e.composedPath() as Array<EventTarget>;
      const target = path.find((el): el is Element => el instanceof Element && el.hasAttribute("data-id"));

      if (!target) {
        dotNetHelper.invokeMethodAsync("OnPointerLeave");
        return;
      }

      const el = target as Element;
      const groupId = el.getAttribute("data-group");
      const id = el.getAttribute("data-id");

      if (!groupId || !id) {
        dotNetHelper.invokeMethodAsync("OnPointerLeave");
        return;
      }

      const svg = root.querySelector("svg");
      if (!svg) return;
      const rect = svg.getBoundingClientRect();

      dotNetHelper.invokeMethodAsync(method, {
        groupId,
        id,
        position: { x: e.clientX - rect.left, y: e.clientY - rect.top }
      });
    };

    const makeChartLeaveHandler = (dotNetHelper: DotNet.DotNetObject, root: HTMLElement) =>
      (e: PointerEvent) => {
        const related = e.relatedTarget as Element | null;
        if (related && root.contains(related)) {
          const targetWithId = related.closest("[data-id]");
          if (targetWithId) return;
        }
        dotNetHelper.invokeMethodAsync("OnPointerLeave");
      };

    const handlers = {
      pointerenter: makeHandler("OnPointerEnter", element),
      pointerleave: makeChartLeaveHandler(dotNetHelper, element),
      pointermove: makeHandler("OnPointerMove", element),
      pointerdown: makeHandler("OnPointerDown", element),
      pointerup: makeHandler("OnPointerUp", element),
      click: makeHandler("OnClick", element)
    };

    element.addEventListener("pointerover", handlers.pointerenter);
    element.addEventListener("pointerout", handlers.pointerleave);
    element.addEventListener("pointermove", handlers.pointermove);
    element.addEventListener("pointerdown", handlers.pointerdown);
    element.addEventListener("pointerup", handlers.pointerup);
    element.addEventListener("click", handlers.click);

    _instances.set(id, {
      intersectionObserver,
      resizeObserver,
      element,
      handlers,
      dotNetHelper
    });

    requestAnimationFrame(() => {
      const rect = container.getBoundingClientRect();
      dotNetHelper.invokeMethodAsync("OnResize", { width: rect.width, height: rect.height });
    });
  }

  export function Dispose(id: string) {
    const inst = _instances.get(id);
    if (!inst) return;

    if (inst.resizeObserver) {
      inst.resizeObserver.disconnect();
    }

    if (inst.intersectionObserver) {
      inst.intersectionObserver.disconnect();
    }

    const el = inst.element;

    if (el && inst.handlers) {
      el.removeEventListener("pointerover", inst.handlers.pointerenter);
      el.removeEventListener("pointerout", inst.handlers.pointerleave);
      el.removeEventListener("pointermove", inst.handlers.pointermove);
      el.removeEventListener("pointerdown", inst.handlers.pointerdown);
      el.removeEventListener("pointerup", inst.handlers.pointerup);
      el.removeEventListener("click", inst.handlers.click);
    }

    _instances.delete(id);
  }
}
