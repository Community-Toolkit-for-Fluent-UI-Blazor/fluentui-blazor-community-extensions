import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.SlideshowVideo {

  interface SlideshowVideoInstance {
    id: string;
    dotNetHelper: DotNet.DotNetObject;
    element: HTMLVideoElement;
    videoWidth: number;
    videoHeight: number;
  }

  const _instances = [] as SlideshowVideoInstance[];

  function waitForSrc(element: HTMLVideoElement): Promise<void> {
    return new Promise(resolve => {
      if (element.src && element.src.length > 0) {
        resolve();
        return;
      }

      const observer = new MutationObserver(() => {
        if (element.src && element.src.length > 0) {
          observer.disconnect();
          resolve();
        }
      });

      observer.observe(element, { attributes: true, attributeFilter: ["src"] });
    });
  }

  export function Initialize(id: string, dotNetHelper: DotNet.DotNetObject): void {
    const element = document.getElementById(id) as HTMLVideoElement;

    if (!element) return;

    const instance: SlideshowVideoInstance = {
      id,
      dotNetHelper,
      element,
      videoWidth: 0,
      videoHeight: 0
    };

    _instances.push(instance);

    waitForSrc(element).then(() => {
      element.addEventListener("loadedmetadata", () => {
        instance.videoWidth = element.videoWidth;
        instance.videoHeight = element.videoHeight;

        instance.dotNetHelper.invokeMethodAsync("OnVideoMeasured", {
          id: instance.id,
          width: instance.videoWidth,
          height: instance.videoHeight
        });
      }, { once: true });
    });
  }

  export function Dispose(id: string): void {
    for (let i = _instances.length - 1; i >= 0; i--) {
      if (_instances[i].id === id) {
        _instances.splice(i, 1);
      }
    }
  }
}
