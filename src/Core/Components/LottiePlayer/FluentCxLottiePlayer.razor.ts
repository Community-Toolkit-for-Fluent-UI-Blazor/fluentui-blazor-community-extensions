import { DotNet } from "../../d-ts/Microsoft.JSInterop";

declare global {
  interface LottieAnimation {
    setSpeed(speed: number): void;
    addEventListener(event: string, callback: () => void): void;
    play(): void;
    pause(): void;
    stop(): void;
    setDirection(direction: number): void;
    playSegments(segments: [number, number], forceFlag?: boolean): void;
    destroy(): void;
  }

  interface Window {
    lottie: {
      loadAnimation(options: {
        container: HTMLElement;
        renderer: 'svg' | 'canvas' | 'html';
        loop: boolean;
        autoplay: boolean;
        path: string;
      }): LottieAnimation;
    };
  }
}

export namespace FluentUI.Blazor.Community.LottiePlayer {
  const _instances: Record<string, LottieAnimation> = {};

  export function Load(
    id: string,
    dotNetRef: DotNet.DotNetObject,
    path: string,
    loop: boolean,
    autoplay: boolean,
    speed: number,
    renderer: 0 | 1 | 2
  ): void {
    if (!window.lottie) {
      console.error("Lottie library is not loaded.");
      return;
    }

    const container = document.getElementById(id);

    if (container) {
      // Clear previous animation if exists
      container.innerHTML = '';

      const animation = window.lottie.loadAnimation({
        container: container,
        renderer: renderer === 0 ? 'svg' : renderer === 1 ? 'canvas' : 'html',
        loop: loop || false,
        autoplay: autoplay || false,
        path: path
      });

      animation.setSpeed(speed || 1);
      animation.addEventListener('complete', () => dotNetRef.invokeMethodAsync('onComplete'));
      animation.addEventListener('loopComplete', () => dotNetRef.invokeMethodAsync('onLoop'));
      animation.addEventListener('enterFrame', () => dotNetRef.invokeMethodAsync('onFrame'));

      _instances[id] = animation;
    }
  }

  export function Play(id: string): void {
    const animation = _instances[id];

    if (animation) {
      animation.play();
    }
  }

  export function Pause(id: string): void {
    const animation = _instances[id];

    if (animation) {
      animation.pause();
    }
  }

  export function Stop(id: string): void {
    const animation = _instances[id];

    if (animation) {
      animation.stop();
    }
  }

  export function SetSpeed(id: string, speed: number): void {
    const animation = _instances[id];

    if (animation) {
      animation.setSpeed(speed);
    }
  }

  export function SetDirection(id: string, direction: number): void {
    const animation = _instances[id];

    if (animation) {
      animation.setDirection(direction);
    }
  }

  export function PlaySegments(id: string, start: number, end: number, forceFlag?: boolean): void {
    const animation = _instances[id];
    if (animation) {
      animation.playSegments([start, end], forceFlag);
    }
  }

  export function Dispose(id: string): void {
    const animation = _instances[id];
    if (animation) {
      animation.destroy();
      delete _instances[id];
    }
  }
}
