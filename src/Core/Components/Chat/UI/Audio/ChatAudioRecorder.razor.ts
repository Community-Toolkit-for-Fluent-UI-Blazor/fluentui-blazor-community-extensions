import { DotNet } from "../../../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.ChatAudioRecorder {
  interface RecorderInstance {
    id: string;
    dotNet: DotNet.DotNetObject;
    chunkSize: number;
    stream: MediaStream | null;
    mediaRecorder: MediaRecorder | null;
    audioChunks: Blob[];
    element: HTMLElement | null;
    bars: HTMLElement[];
    audioContext: AudioContext | null;
    analyser: AnalyserNode | null;
    data: Uint8Array<ArrayBuffer> | null;
    frameId: number | null;
    barHeights: number[];
    startTime: number;
    timerHandle: number | null;
  }

  const _instances: RecorderInstance[] = [];

  function getInstance(id: string): RecorderInstance | null {
    return _instances.find((x) => x.id === id) ?? null;
  }

  export function Initialize(
    id: string,
    dotNet: DotNet.DotNetObject,
    chunkSize: number,
  ): void {
    const element = document.getElementById(id) as HTMLElement | null;
    if (!element) return;

    const bars = Array.from(
      element.querySelectorAll(".audio-bar"),
    ) as HTMLElement[];
    if (bars.length === 0) return;

    const count = bars.length;
    const step = 360 / count;

    for (let i = 0; i < count; i++) {
      const angle = step * i;
      bars[i].style.transform = `rotate(${angle}deg) translateY(-40px)`;
      bars[i].style.setProperty("--bar-height", "4px");
      bars[i].style.opacity = "0.2";
    }

    _instances.push({
      id,
      dotNet,
      chunkSize,
      stream: null,
      mediaRecorder: null,
      audioChunks: [],
      element,
      bars,
      audioContext: null,
      analyser: null,
      data: null,
      frameId: null,
      barHeights: new Array(count).fill(4),
      startTime: 0,
      timerHandle: null,
    });
  }

  export async function Start(id: string): Promise<void> {
    const inst = getInstance(id);

    if (!inst) return;

    inst.stream = await navigator.mediaDevices.getUserMedia({ audio: true });
    inst.audioContext = new AudioContext();

    const source = inst.audioContext.createMediaStreamSource(inst.stream);

    inst.analyser = inst.audioContext.createAnalyser();
    inst.analyser.fftSize = 128;
    inst.data = new Uint8Array(inst.analyser.frequencyBinCount);

    source.connect(inst.analyser);

    inst.mediaRecorder = new MediaRecorder(inst.stream);
    inst.audioChunks = [];

    inst.mediaRecorder.ondataavailable = (e) => inst.audioChunks.push(e.data);

    inst.mediaRecorder.onstop = async () => {
      const allBlobs = inst.audioChunks;
      inst.audioChunks = [];

      const buffers: Uint8Array[] = [];

      for (const blob of allBlobs) {
        const buffer = new Uint8Array(await blob.arrayBuffer());
        buffers.push(buffer);
      }

      const totalLength = buffers.reduce((sum, b) => sum + b.length, 0);
      const merged = new Uint8Array(totalLength);
      let offset = 0;
      for (const b of buffers) {
        merged.set(b, offset);
        offset += b.length;
      }

      for (let i = 0; i < merged.length; i += inst.chunkSize) {
        const chunk = merged.slice(i, i + inst.chunkSize);
        await inst.dotNet.invokeMethodAsync(
          "ReceiveAudioChunk",
          chunk
        );
      }

      await inst.dotNet.invokeMethodAsync("RecordingCompleted");
    };

    inst.mediaRecorder.start();

    inst.startTime = performance.now();

    if (inst.timerHandle !== null) {
      clearInterval(inst.timerHandle);
      inst.timerHandle = null;
    }

      inst.timerHandle = window.setInterval(() => {
        const elapsed = performance.now() - inst.startTime;
        const totalSeconds = Math.floor(elapsed / 1000);
        const minutes = Math.floor(totalSeconds / 60);
        const seconds = totalSeconds % 60;
        const text = `${minutes.toString().padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;

        inst.dotNet.invokeMethodAsync("UpdateRecordingTime", text);
      }, 1000);

    function animate() {
      if (!inst || !inst.analyser || !inst.data) return;

      inst.analyser.getByteFrequencyData(inst.data);

      for (let i = 0; i < inst.bars.length; i++) {
        const value = inst.data[i % inst.data.length];

        const normalized = value / 255;
        const eased = Math.pow(normalized, 1.5);
        const targetHeight = 4 + eased * 32;
        const current = inst.barHeights[i];
        const smoothed = current + (targetHeight - current) * 0.25;

        inst.barHeights[i] = smoothed;
        inst.bars[i].style.setProperty("--bar-height", `${smoothed}px`);

        const opacity = 0.2 + eased * 0.8;
        inst.bars[i].style.opacity = opacity.toString();
      }

      inst.frameId = requestAnimationFrame(animate);
    }

    animate();
  }

  export function Stop(id: string): void {
    const inst = getInstance(id);
    if (!inst) return;

    if (inst.frameId !== null) {
      cancelAnimationFrame(inst.frameId);
      inst.frameId = null;
    }

    if (inst.audioContext) {
      inst.audioContext.close();
      inst.audioContext = null;
    }

    if (inst.timerHandle !== null) {
      clearInterval(inst.timerHandle);
      inst.timerHandle = null;
    }

    if (inst.mediaRecorder && inst.mediaRecorder.state !== "inactive") {
      inst.mediaRecorder.stop();
    }

    if (inst.stream) {
      inst.stream.getTracks().forEach((t) => t.stop());
      inst.stream = null;
    }
  }

  export function Dispose(id: string): void {
    const inst = getInstance(id);
    if (!inst) return;

    Stop(id);

    const index = _instances.findIndex((x) => x.id === id);
    if (index >= 0) {
      _instances.splice(index, 1);
    }
  }
}
