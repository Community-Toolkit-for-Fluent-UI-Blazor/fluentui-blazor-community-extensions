import { DotNet } from "../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Video {
  interface VideoInstance {
    id: string;
    element: HTMLVideoElement;
    dotNetHelper: DotNet.DotNetObject;
  }

  const _instances = new Map<string, VideoInstance>();

  export function Initialize(id: string, element: HTMLVideoElement, dotNetRef: DotNet.DotNetObject): void {
    if (!element) return;

    const instance: VideoInstance = {
      id,
      element,
      dotNetHelper: dotNetRef
    };

    _instances.set(id, instance);

    element.addEventListener("loadedmetadata", () => {
      dotNetRef.invokeMethodAsync("setDuration", element.duration);
    });

    element.addEventListener("ended", () => {
      dotNetRef.invokeMethodAsync("onTrackEnded");
    });

    element.addEventListener("seeked", () => {
      dotNetRef.invokeMethodAsync("setSeek", element.currentTime);
    });

    element.addEventListener("timeupdate", () => {
      dotNetRef.invokeMethodAsync("updateElapsedTime", element.currentTime);
    });
  }

  export function PictureInPicture(id: string): void {
    const instance = _instances.get(id);
    if (!instance) return;

    const video = instance.element;

    if (document.pictureInPictureElement) {
      document.exitPictureInPicture();
    } else {
      video.requestPictureInPicture?.();
    }
  }

  export function Fullscreen(id: string): void {
    const container = document.getElementById(id);
    if (!container) return;

    if (!document.fullscreenElement) {
      container.requestFullscreen?.();
      (container as any).webkitRequestFullscreen?.();
      (container as any).msRequestFullscreen?.();
    } else {
      document.exitFullscreen();
    }
  }

  export function Play(id: string): void {
    const instance = _instances.get(id);
    instance?.element.play();
  }

  export function Pause(id: string): void {
    const instance = _instances.get(id);
    instance?.element.pause();
  }

  export function Stop(id: string): void {
    const instance = _instances.get(id);
    if (!instance) return;

    instance.element.pause();
    instance.element.currentTime = 0;
  }

  export function TogglePlayPause(id: string): void {
    const instance = _instances.get(id);
    if (!instance) return;

    const video = instance.element;
    video.paused ? video.play() : video.pause();
  }

  export function Seek(id: string, time: number): void {
    const instance = _instances.get(id);
    if (!instance) return;

    instance.element.currentTime = time;
    instance.dotNetHelper.invokeMethodAsync("OnSeeked", time);
  }

  export async function SetSource(id: string, source: string): Promise<number> {
    const instance = _instances.get(id);

    if (!instance) {
      return 0;
    }

    const video = instance.element;

    const metadataLoaded = new Promise<number>(resolve => {
      const handler = () => {
        video.removeEventListener("loadedmetadata", handler);
        resolve(video.duration || 0);
      };

      video.addEventListener("loadedmetadata", handler, { once: true });
    });

    video.src = source;
    video.load();

    return await metadataLoaded;
  }

  export function SetVolume(id: string, volume: number): void {
    const instance = _instances.get(id);
    if (!instance) return;

    instance.element.volume = volume;
  }

  export function SetPlaybackRate(id: string, rate: number): void {
    const instance = _instances.get(id);
    if (!instance) return;

    instance.element.playbackRate = rate;
  }

  export function Download(source: string, filename: string): void {
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = source;
    a.download = filename;
    a.target = "_blank";
    a.click();
    document.body.removeChild(a);
  }

  export function Dispose(id: string): void {
    const instance = _instances.get(id);
    if (!instance) return;

    Stop(id);

    const el = instance.element;

    const clone = el.cloneNode(true) as HTMLVideoElement;
    el.replaceWith(clone);

    _instances.delete(id);
  }
}
