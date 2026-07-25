import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Slideshow {

  const HORIZONTAL = 0;
  const MOVE_NEXT = 0;
  const MOVE_PREVIOUS = 1;

  interface TouchHandlers {
    startHandler: (e: TouchEvent) => void;
    endHandler: (e: TouchEvent) => void;
    moveHandler: (e: TouchEvent) => void;
  }

  interface SlideshowInstance {
    id: string;
    element: HTMLElement;
    itemsContainer: HTMLElement;
    dotnetReference: DotNet.DotNetObject;
    startX: number;
    startY: number;
    items: HTMLElement[];
    touchHandlers: TouchHandlers | null;
    resizeObserver: ResizeObserver | null;
  }

  const _instances: SlideshowInstance[] = [];

  function getInstance(id: string): SlideshowInstance | null {
    return _instances.find(x => x.id === id) ?? null;
  }

  export function Initialize(id: string, dotnetReference: DotNet.DotNetObject) {
    const element = document.getElementById(id) as HTMLElement | null;
    const itemsContainer = document.getElementById(`slide-show-items-container-${id}`) as HTMLElement | null;

    if (!element || !itemsContainer)
      return;

    const instance: SlideshowInstance = {
      id,
      element,
      itemsContainer,
      dotnetReference,
      startX: 0,
      startY: 0,
      items: [],
      touchHandlers: null,
      resizeObserver: null
    };

    instance.resizeObserver = new ResizeObserver(entries => {
      for (const entry of entries) {
        const rect = entry.contentRect;

        if (rect.width === 0 || rect.height === 0)
          continue;

        instance.dotnetReference.invokeMethodAsync("OnSlideshowResized", {
          id: instance.id,
          width: rect.width,
          height: rect.height
        });
      }
    });

    instance.resizeObserver.observe(element);

    _instances.push(instance);

    requestAnimationFrame(() =>
    {
      const rect = element.getBoundingClientRect();
      if (rect.width > 0 && rect.height > 0)
      {
        instance.dotnetReference.invokeMethodAsync("OnSlideshowResized", { id: instance.id, width: rect.width, height: rect.height });
      }
    });

    return {
      dispose: () => Destroy(id)
    };
  }

  export function StoreItems(id: string, idCollection: string[]) {
    const instance = getInstance(id);
    if (!instance) return;

    instance.items = [];

    for (const itemId of idCollection) {
      const item = document.getElementById(itemId) as HTMLElement | null;
      if (item) instance.items.push(item);
    }
  }

  export function RestoreItems(id: string) {
    const instance = getInstance(id);
    if (!instance) return;

    const container = instance.itemsContainer;

    while (container.firstChild)
      container.removeChild(container.firstChild);

    for (const item of instance.items)
      container.appendChild(item);
  }

  export function ClearTransition(id: string) {
    const instance = getInstance(id);
    if (!instance) return;

    instance.itemsContainer.style.transition = "";
    instance.itemsContainer.style.transform = "";
  }

  export function InfiniteLoopMoveNext(id: string, orientation: number) {
    const instance = getInstance(id);
    if (!instance) return;

    const container = instance.itemsContainer;
    const first = container.firstElementChild as HTMLElement | null;
    if (!first) return;

    const isHorizontal = orientation === HORIZONTAL;

    container.style.transition = "transform var(--slideshow-duration) ease-in-out";
    container.style.transform = isHorizontal
      ? "translateX(calc(-100% / var(--slideshow-item-count)))"
      : "translateY(calc(-100% / var(--slideshow-item-count)))";

    container.addEventListener("transitionend", () => {
      container.style.transition = "none";
      container.append(first);
      container.style.transform = isHorizontal ? "translateX(0)" : "translateY(0)";
    }, { once: true });
  }

  export function InfiniteLoopMovePrevious(id: string, orientation: number) {
    const instance = getInstance(id);
    if (!instance) return;

    const container = instance.itemsContainer;
    const last = container.lastElementChild as HTMLElement | null;
    if (!last) return;

    const isHorizontal = orientation === HORIZONTAL;

    container.style.transition = "none";
    container.prepend(last);
    container.style.transform = isHorizontal
      ? "translateX(calc(-100% / var(--slideshow-item-count)))"
      : "translateY(calc(-100% / var(--slideshow-item-count)))";

    requestAnimationFrame(() => {
      container.style.transition = "transform var(--slideshow-duration) ease-in-out";
      container.style.transform = isHorizontal ? "translateX(0)" : "translateY(0)";
    });
  }

  function onTouchStart(instance: SlideshowInstance, e: TouchEvent) {
    instance.startX = e.touches[0].clientX;
    instance.startY = e.touches[0].clientY;
  }

  function onTouchEnd(instance: SlideshowInstance, e: TouchEvent, threshold: number) {
    const endX = e.changedTouches[0].clientX;
    const endY = e.changedTouches[0].clientY;

    const deltaX = endX - instance.startX;
    const deltaY = endY - instance.startY;

    if (Math.abs(deltaX) > Math.abs(deltaY)) {
      if (Math.abs(deltaX) > threshold) {
        instance.dotnetReference.invokeMethodAsync("onTouchSwipe",
          deltaX > 0 ? MOVE_PREVIOUS : MOVE_NEXT);
      }
    } else {
      if (Math.abs(deltaY) > threshold) {
        instance.dotnetReference.invokeMethodAsync("onTouchSwipe",
          deltaY > 0 ? MOVE_PREVIOUS : MOVE_NEXT);
      }
    }
  }

  function onTouchMove(e: TouchEvent) {
    e.preventDefault();
  }

  export function DisableOrEnableTouch(id: string, enabled: boolean, threshold: number) {
    const instance = getInstance(id);
    if (!instance) return;

    const startHandler = (e: TouchEvent) => onTouchStart(instance, e);
    const endHandler = (e: TouchEvent) => onTouchEnd(instance, e, threshold);
    const moveHandler = (e: TouchEvent) => onTouchMove(e);

    if (enabled) {
      instance.element.addEventListener("touchstart", startHandler);
      instance.element.addEventListener("touchend", endHandler);
      instance.element.addEventListener("touchmove", moveHandler, { passive: false });

      instance.touchHandlers = { startHandler, endHandler, moveHandler };
    } else if (instance.touchHandlers) {
      instance.element.removeEventListener("touchstart", instance.touchHandlers.startHandler);
      instance.element.removeEventListener("touchend", instance.touchHandlers.endHandler);
      instance.element.removeEventListener("touchmove", instance.touchHandlers.moveHandler);

      instance.touchHandlers = null;
    }
  }

  export function InsideDialog(id: string) {
    const slideshow = document.getElementById(id);

    if (!slideshow) {
      return;
    }

    const body = slideshow.closest("fluent-dialog-body") as HTMLElement | null;

    if (!body) {
      return;
    }

    if (!body) {
      return;
    }

    const shadow = body.shadowRoot;

    if (!shadow) {
      return;
    }

    const content = shadow.querySelector('[part="content"]') as HTMLElement | null;

    if (!content) {
      return;
    }

    const slot = content.querySelector('slot') as HTMLSlotElement | null;

    if (!slot) {
      return;
    }

    const nodes = slot.assignedElements ? slot.assignedElements() : slot.assignedNodes();

    if (!nodes || nodes.length === 0) {
      return;
    }

    const wrapper = nodes[0] as HTMLElement;

    if (!wrapper) {
      return;
    }

    wrapper.style.display = 'flex';
    wrapper.style.height = '100%';
    wrapper.style.minHeight = '0';
  }

  export function Destroy(id: string) {
    const index = _instances.findIndex(x => x.id === id);
    if (index === -1) return;

    const instance = _instances[index];

    if (instance.touchHandlers) {
      instance.element.removeEventListener("touchstart", instance.touchHandlers.startHandler);
      instance.element.removeEventListener("touchend", instance.touchHandlers.endHandler);
      instance.element.removeEventListener("touchmove", instance.touchHandlers.moveHandler);
    }

    if (instance.resizeObserver) {
      instance.resizeObserver.disconnect();
    }

    _instances.splice(index, 1);
  }
}
