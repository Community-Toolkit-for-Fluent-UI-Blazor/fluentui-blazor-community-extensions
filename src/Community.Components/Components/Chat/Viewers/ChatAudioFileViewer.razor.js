const _instances = [];

export function initialize(id, dotnetReference) {
  const audioElement = document.getElementById("audio-" + id);

  if (audioElement) {
    const obj = {
      audioElement: audioElement,
      dotNetRef: dotnetReference
    };

    _instances[id] = obj;

    audioElement.addEventListener('timeupdate', () => {
      if (audioElement.duration == audioElement.currentTime) {
        audioElement.pause();
        obj.dotNetRef.invokeMethodAsync('onPlayCompleted');
      }
    });

    
  }
}

export function play(id) {
  const instance = _instances[id];

  if (instance) {
    instance.audioElement.play();
  }
}

export function pause(id) {
  const instance = _instances[id];

  if (instance) {
    instance.audioElement.pause();
  }
}
