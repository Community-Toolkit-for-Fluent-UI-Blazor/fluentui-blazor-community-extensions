import { DotNet } from "../../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.ChatAudioFileViewer {
  interface PlayerInstance {
    id: string;
    audioElement: HTMLAudioElement;
    dotNet: DotNet.DotNetObject;
  }

  const _instances: PlayerInstance[] = [];

  function getInstance(id: string): PlayerInstance | null {
    return _instances.find(x => x.id === id) ?? null;
  }

  export function Initialize(id: string, dotNet: DotNet.DotNetObject): void {
    const audioElement = document.getElementById("audio-" + id) as HTMLAudioElement | null;
    if (!audioElement) return;

    const existing = getInstance(id);
    if (!existing) {
      const instance: PlayerInstance = { id, audioElement, dotNet };
      _instances.push(instance);

      audioElement.addEventListener("timeupdate", () => {
        if (audioElement.duration > 0 && audioElement.currentTime >= audioElement.duration) {
          audioElement.pause();
          instance.dotNet.invokeMethodAsync("onPlayCompleted");
        }
      });
    }
  }

  export function Play(id: string): void {
    const inst = getInstance(id);
    if (!inst) return;

    void inst.audioElement.play();
  }

  export function Pause(id: string): void {
    const inst = getInstance(id);
    if (!inst) return;

    inst.audioElement.pause();
  }

  export function Dispose(id: string): void {
    const index = _instances.findIndex(x => x.id === id);
    if (index < 0) return;

    const inst = _instances[index];
    inst.audioElement.pause();
    _instances.splice(index, 1);
  }
}
