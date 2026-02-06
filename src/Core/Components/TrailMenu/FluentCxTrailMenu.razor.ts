import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.TrailMenu {

  interface TrailMenuInstance {
    id: string;
    element: HTMLElement;
    itemContainer: HTMLElement;
    dotNetHelper: DotNet.DotNetObject;
    resizeObserver: ResizeObserver | null;
    mutationObserver: MutationObserver | null;
  }

  const _instances: Record<string, TrailMenuInstance> = {};

  export function Initialize(
    id: string,
    dotNetHelper: DotNet.DotNetObject,
    mutationConfiguration?: MutationObserverInit
  ): void {

    const element = document.getElementById(id) as HTMLElement | null;
    const itemContainer = document.getElementById(`fluentcx-trail-menu-container-${id}`) as HTMLElement | null;

    if (!element || !itemContainer) {
      return;
    }

    // Resize Observer
    const resizeObserver = new ResizeObserver((entries) => {
      const width = entries[0].contentRect.width;
      dotNetHelper.invokeMethodAsync("OnResize", width);
    });

    resizeObserver.observe(itemContainer);

    // Mutation Observer
    const mutationObserver = new MutationObserver(() => {
      dotNetHelper.invokeMethodAsync("OnMutated");
    });

    mutationObserver.observe(itemContainer, mutationConfiguration ?? {
      attributes: true,
      childList: true,
      subtree: false,
      attributeFilter: ["class", "style"]
    });

    const instance: TrailMenuInstance = {
      id,
      element,
      itemContainer,
      dotNetHelper,
      resizeObserver,
      mutationObserver
    };

    _instances[id] = instance;
  }

  export async function GetWidth(id: string): Promise<number> {
    await new Promise(requestAnimationFrame);

    const el = document.getElementById(id) as HTMLElement | null;

    if (!el) {
      return 0;
    }

    const rect = el.getBoundingClientRect();
    return rect.width;
  }

  export function Dispose(id: string): void {
    const instance = _instances[id];
    if (!instance) {
      return;
    }

    if (instance.resizeObserver) {
      instance.resizeObserver.disconnect();
      instance.resizeObserver = null;
    }

    if (instance.mutationObserver) {
      instance.mutationObserver.disconnect();
      instance.mutationObserver = null;
    }

    delete _instances[id];
  }
}
