import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.SlideshowImage {

  interface SlideshowImageInstance {
    id: string;
    dotNetHelper: DotNet.DotNetObject;
    element: HTMLImageElement;
    naturalWidth: number;
    naturalHeight: number;
  }

  const _instances = [] as SlideshowImageInstance[];

  function waitForSrc(element: HTMLImageElement): Promise<void> {
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
    const element = document.getElementById(id) as HTMLImageElement;

    if (!element) {
      return;
    }

    const instance: SlideshowImageInstance = {
      id,
      dotNetHelper,
      element,
      naturalWidth: 0,
      naturalHeight: 0
    };

    _instances.push(instance);

    waitForSrc(element).then(() => {
      element.decode()
        .then(() => {
          instance.naturalWidth = element.naturalWidth;
          instance.naturalHeight = element.naturalHeight;

          instance.dotNetHelper.invokeMethodAsync("OnImageMeasured", {
            id: instance.id,
            Width: instance.naturalWidth,
            Height: instance.naturalHeight
          });
        })
        .catch(() => {
          instance.naturalWidth = element.naturalWidth;
          instance.naturalHeight = element.naturalHeight;

          instance.dotNetHelper.invokeMethodAsync("OnImageMeasured", {
            id: instance.id,
            Width: instance.naturalWidth,
            Height: instance.naturalHeight
          });
        });

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
