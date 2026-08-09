import { DotNet } from "../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Motion {

  interface FpsInstance {
    Fps: number;
    IsRunning: boolean;
    DotNet: DotNet.DotNetObject;
    ResizeObserver?: ResizeObserver;
  }

  const _instances: Map<string, FpsInstance> = new Map();

  export function Start(
    id: string,
    dotNetHelper: DotNet.DotNetObject,
    fps: number): void {
    const instance = document.getElementById(id);

    if (!instance) {
      return;
    }

    _instances.set(id, { Fps: fps, IsRunning: true, DotNet: dotNetHelper });

    const i = _instances.get(id);

    let last = performance.now();

    const loop = (now: number) => {
      if (!i?.IsRunning) {
        return;
      }

      const delta = now - last;

      if (i.Fps < 1 || delta >= (1000 / i.Fps)) {
        last = now;
        dotNetHelper.invokeMethodAsync("OnAnimationFrame", delta);
      }

      requestAnimationFrame(loop);
    };

    requestAnimationFrame(loop);
  }

  export function UpdateFps(id: string, fps: number) {
    const instance = _instances.get(id);

    if (!instance) {
      return;
    }

    instance.Fps = fps;
  }

  export function Stop(id: string) {
    const instance = _instances.get(id);

    if (!instance) {
      return;
    }

    instance.IsRunning = false;
    _instances.delete(id);
  }

  export function SetStyle(id: string, style: string) {
    const element = document.getElementById(id);

    if (!element) {
      return;
    }

    element.style.cssText = style;
  }

  function onVisibilityChange() {
    const hidden = document.hidden;

    _instances.forEach((instance) => {
      instance.IsRunning = !hidden;
    });    
  }

  document.addEventListener("visibilitychange", onVisibilityChange);

  window.addEventListener("beforeunload", () => {
    document.removeEventListener("visibilitychange", onVisibilityChange);
  });

  export function RegisterResizeObserver(parentId: string, id: string) {
    const instance = _instances.get(parentId);

    if (!instance) {
      return;
    }

    const element = document.getElementById(id);

    if (!element) {
      return;
    }

    const ro = new ResizeObserver(entries => {
      for (const entry of entries) {
        const { width, height } = entry.contentRect;
        instance.DotNet.invokeMethodAsync("OnMotionGroupSizeChanged", id, Math.ceil(width), Math.ceil(height));
      }
    });

    instance.ResizeObserver = ro;
    ro.observe(element);
  }

  export function UnregisterResizeObserver(parentId: string, id: string) {
    const instance = _instances.get(parentId);

    if (!instance) {
      return;
    }

    const element = document.getElementById(id);

    if (!element) {
      return;
    }

    if (instance.ResizeObserver) {
      instance.ResizeObserver.unobserve(element);
    }
  }
}
