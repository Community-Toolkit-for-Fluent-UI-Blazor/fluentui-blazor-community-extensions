const _instances = new Map();

function initialize(id, element, dotnetRef) {
  _instances.set(id, { element, dotnetRef });

  element.addEventListener('loadedmetadata', () => {
    dotnetRef.invokeMethodAsync("setDuration", element.duration);
  });

  element.addEventListener('ended', () => {
    dotnetRef.invokeMethodAsync("onTrackEnded");
  });

  element.addEventListener('seeked', () => {
    dotnetRef.invokeMethodAsync("setSeek", element.currentTime);
  });

  element.addEventListener('timeupdate', () => {
    dotnetRef.invokeMethodAsync("updateElapsedTime", element.currentTime);
  });
}

function pictureInPicture(id) {
  const instance = _instances.get(id);

  if (instance) {
    const video = instance.element;

    if (document.pictureInPictureElement) {
      document.exitPictureInPicture();
    }
    else {
      video.requestPictureInPicture();
    }
  }
}

function fullscreen(id) {
  const container = document.getElementById(id);

  if (container) {
    if (!document.fullscreenElement) {
      if (container.requestFullscreen) {
        container.requestFullscreen();
      } else if (container.webkitRequestFullscreen) {
        container.webkitRequestFullscreen();
      } else if (container.msRequestFullscreen) {
        container.msRequestFullscreen();
      }
    } else {
      document.exitFullscreen();
    }
  }
}

function play(id) {
  if (_instances.has(id)) {
    const instance = _instances.get(id);

    instance.element.play();
  }
}

function pause(id) {
  if (_instances.has(id)) {
    const instance = _instances.get(id);
    instance.element.pause();
  }
}

function seek(id, time) {
  if (_instances.has(id)) {
    const instance = _instances.get(id);
    instance.element.currentTime = time;
    instance.dotnetRef.invokeMethodAsync('OnSeeked', time);
  }
}

function setSource(id, source) {
  if (_instances.has(id)) {
    const instance = _instances.get(id);
    instance.element.src = source;
    instance.element.load();
  }
}

function stop(id) {
  if (_instances.has(id)) {
    const instance = _instances.get(id);
    instance.element.pause();
    instance.element.currentTime = 0;
  }
}

function togglePlayPause(id) {
  const instance = _instances.get(id);
  if (instance) {
    const video = instance.element;
    if (video.paused) {
      video.play();
    } else {
      video.pause();
    }
  }
}

function setVolume(id, volume) {
  const instance = _instances.get(id);
  if (instance) {
    const video = instance.element;
    video.volume = volume;
  }
}

function download(source, filename) {
  const a = document.createElement('a');
  document.body.appendChild(a);
  a.href = source;
  a.download = filename;
  a.target = "_blank";
  a.click();
  document.body.removeChild(a);
}

function setPlaybackRate(id, rate) {
  const instance = _instances.get(id);
  if (instance) {
    const video = instance.element;
    video.playbackRate = rate;
  }
}

function dispose(id) {
  if (_instances.has(id)) {
    stop(id);

    const instance = _instances.get(id);

    instance.element.removeEventListener('loadedmetadata', () => { });
    instance.element.removeEventListener('timeupdate', () => { });
    instance.element.removeEventListener('ended', () => { });
    instance.element.removeEventListener('seeked', () => { });

    _instances.delete(id);
  }
}

export const fluentCxVideo = {
  initialize: (id, element, dotNetHelper) => initialize(id, element, dotNetHelper),
  play: (id) => play(id),
  pause: (id) => pause(id),
  seek: (id, time) => seek(id, time),
  setSource: (id, source) => setSource(id, source),
  stop: (id) => stop(id),
  download: (source, filename) => download(source, filename),
  dispose: (id) => dispose(id),
  setVolume: (id, volume) => setVolume(id, volume),
  togglePlayPause: (id) => togglePlayPause(id),
  pictureInPicture: (id) => pictureInPicture(id),
  fullscreen: (id) => fullscreen(id),
  setPlaybackRate: (id, rate) => setPlaybackRate(id, rate)
}
