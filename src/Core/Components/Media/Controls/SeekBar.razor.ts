import { DotNet } from "../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.SeekBar {

  interface SeekBarInstance {
    id: string;
    element: HTMLElement;
    dotNetHelper: DotNet.DotNetObject;
    resizeObserver: ResizeObserver;
  }

  const _instances: Record<string, SeekBarInstance> = {};

  export function GetWidth(id: string): number {
    const instance = _instances[id];
    if (!instance || !instance.element) return 1;

    return instance.element.getBoundingClientRect().width;
  }

  export function Initialize(id: string, dotNetHelper: DotNet.DotNetObject): void {
    const element = document.getElementById(id);
    if (!element) return;

    const resizeObserver = new ResizeObserver(entries => {
      for (const entry of entries) {
        if (entry.target.id === id) {
          dotNetHelper.invokeMethodAsync("onResize", entry.contentRect.width);
        }
      }
    });

    resizeObserver.observe(element);

    _instances[id] = {
      id,
      element,
      dotNetHelper,
      resizeObserver
    };
  }

  export function GetOffsetX(id: string, clientX: number): number {
    const instance = _instances[id];
    if (!instance || !instance.element) return 0;

    const rect = instance.element.getBoundingClientRect();
    return clientX - rect.left;
  }

  export function GetThumbnail(
    id: string,
    videoElement: HTMLVideoElement | null,
    canvasElement: HTMLCanvasElement | null,
    thumbnailElement: HTMLImageElement | null
  ): void {

    if (!videoElement || !canvasElement || !thumbnailElement) return;

    const ctx = canvasElement.getContext("2d");
    if (!ctx) return;

    ctx.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
    const dataUrl = canvasElement.toDataURL("image/png");

    thumbnailElement.src = dataUrl;
  }

  export function Dispose(id: string): void {
    const instance = _instances[id];
    if (!instance) return;

    instance.resizeObserver.disconnect();
    instance.dotNetHelper.dispose();

    delete _instances[id];
  }
}
