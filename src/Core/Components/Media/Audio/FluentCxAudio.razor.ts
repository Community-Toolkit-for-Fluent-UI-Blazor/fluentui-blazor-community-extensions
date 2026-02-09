import { DotNet } from "../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.Audio {
  interface AudioInstance {
    id: string;
    element: HTMLAudioElement;
    dotNetHelper?: DotNet.DotNetObject;
  }

  const _audioInstances: Record<string, AudioInstance> = {};

  interface ResizeInstance {
    id: string;
    element: HTMLElement;
    resizeObserver: ResizeObserver;
    dotNetHelper: DotNet.DotNetObject;
  }

  const _resizeInstances: Record<string, ResizeInstance> = {};
  export function Initialize(audioElement: HTMLAudioElement, dotNetRef: DotNet.DotNetObject): void {
    if (!audioElement) return;

    const id = audioElement.id || crypto.randomUUID();

    _audioInstances[id] = {
      id,
      element: audioElement,
      dotNetHelper: dotNetRef
    };

    audioElement.addEventListener("loadedmetadata", () => {
      dotNetRef.invokeMethodAsync("setDuration", audioElement.duration);
    });

    audioElement.addEventListener("ended", () => {
      dotNetRef.invokeMethodAsync("onTrackEnded");
    });

    audioElement.addEventListener("seeked", () => {
      dotNetRef.invokeMethodAsync("setSeek", audioElement.currentTime);
    });

    audioElement.addEventListener("timeupdate", () => {
      dotNetRef.invokeMethodAsync("updateElapsedTime", audioElement.currentTime);
    });
  }
  export function Measure(id: string): { width: number; height: number } {
    const element = document.getElementById(id);
    if (!element) return { width: 0, height: 0 };

    const rect = element.getBoundingClientRect();
    return { width: rect.width, height: rect.height };
  }
  export function ObserveResize(id: string, dotNetRef: DotNet.DotNetObject): void {
    const element = document.getElementById(id);
    if (!element) return;

    StopObservingResize(id);

    const resizeObserver = new ResizeObserver(entries => {
      for (const entry of entries) {
        const rect = entry.contentRect;
        dotNetRef.invokeMethodAsync("onResize", Math.round(rect.width), Math.round(rect.height));
      }
    });

    resizeObserver.observe(element);

    _resizeInstances[id] = {
      id,
      element,
      resizeObserver,
      dotNetHelper: dotNetRef
    };
  }

  export function StopObservingResize(id: string): void {
    const instance = _resizeInstances[id];
    if (!instance) return;

    instance.resizeObserver.disconnect();
    delete _resizeInstances[id];
  }

  export function Exists(id: string): boolean {
    return document.getElementById(id) !== null;
  }

  export function Play(audio: HTMLAudioElement | null): void {
    audio?.play();
  }

  export function Pause(audio: HTMLAudioElement | null): void {
    audio?.pause();
  }

  export function TogglePlayPause(audio: HTMLAudioElement | null): void {
    if (!audio) return;
    audio.paused ? audio.play() : audio.pause();
  }

  export function Stop(audio: HTMLAudioElement | null): void {
    if (!audio) return;
    audio.pause();
    audio.currentTime = 0;
  }

  export function SetVolume(audio: HTMLAudioElement | null, volume: number): void {
    if (audio) audio.volume = volume;
  }

  export function Seek(audio: HTMLAudioElement | null, time: number): void {
    if (audio) audio.currentTime = time;
  }

  export async function SetAudioSource(audio: HTMLAudioElement | null, source: string): Promise<number> {
    if (!audio) {
      return 0;
    }

    const metadataLoaded = new Promise<number>(resolve => {
      const handler = () => {
        audio.removeEventListener("loadedmetadata", handler);
        resolve(audio.duration || 0);
      };

      audio.addEventListener("loadedmetadata", handler, { once: true });
    });

    audio.src = source;
    audio.load();

    return await metadataLoaded;
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
  export function Dispose(audio: HTMLAudioElement | null): void {
    if (!audio) return;

    const id = audio.id;
    const instance = _audioInstances[id];
    if (!instance) return;

    Stop(audio);

    audio.replaceWith(audio.cloneNode(true));

    delete _audioInstances[id];
  }

  export async function SeekAndResume(audio: HTMLAudioElement | null, time: number): Promise<boolean> {
    if (!audio) return false;

    try { audio.pause(); } catch { }

    if (!isFinite(audio.duration) || isNaN(audio.duration)) {
      await new Promise(resolve => {
        const onMeta = () => {
          audio.removeEventListener("loadedmetadata", onMeta);
          resolve(null);
        };
        audio.addEventListener("loadedmetadata", onMeta);
        audio.load?.();
      });
    }

    const target = Math.min(Math.max(time, 0), audio.duration || time);

    if (typeof audio.fastSeek === "function") {
      try { audio.fastSeek(target); } catch { audio.currentTime = target; }
    } else {
      audio.currentTime = target;
    }

    await new Promise(resolve => {
      let done = false;
      const timeout = setTimeout(() => {
        if (!done) { done = true; cleanup(); resolve(null); }
      }, 800);

      const onSeeked = () => {
        if (!done) { done = true; cleanup(); resolve(null); }
      };

      const cleanup = () => {
        clearTimeout(timeout);
        audio.removeEventListener("seeked", onSeeked);
      };

      audio.addEventListener("seeked", onSeeked, { once: true });
    });

    const tryPlay = async () => {
      try { await audio.play(); } catch { }

      await new Promise(resolve => {
        let done = false;
        const timeout = setTimeout(() => {
          if (!done) { done = true; cleanup(); resolve(null); }
        }, 1500);

        const onPlaying = () => {
          if (!done) { done = true; cleanup(); resolve(null); }
        };

        const onCanPlay = () => {
          if (!done) { done = true; cleanup(); resolve(null); }
        };

        const cleanup = () => {
          clearTimeout(timeout);
          audio.removeEventListener("playing", onPlaying);
          audio.removeEventListener("canplay", onCanPlay);
        };

        audio.addEventListener("playing", onPlaying, { once: true });
        audio.addEventListener("canplay", onCanPlay, { once: true });
      });
    };

    const before = audio.currentTime;
    await tryPlay();
    const after = audio.currentTime;

    if (Math.abs(after - before) < 0.01) {
      audio.pause();
      audio.currentTime = target + 0.001;
      await tryPlay();
    }

    return true;
  }
}
