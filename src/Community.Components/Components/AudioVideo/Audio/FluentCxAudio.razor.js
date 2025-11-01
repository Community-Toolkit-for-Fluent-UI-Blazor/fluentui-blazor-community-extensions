function initialize(audio, dotnetRef) {
  if (audio) {
    audio.addEventListener('loadedmetadata', () => {
      dotnetRef.invokeMethodAsync("setDuration", audio.duration);
    });

    audio.addEventListener('ended', () => {
      dotnetRef.invokeMethodAsync("onTrackEnded");
    });

    audio.addEventListener('seeked', () => {
      dotnetRef.invokeMethodAsync("setSeek", audio.currentTime);
    });

    audio.addEventListener('timeupdate', () => {
      dotnetRef.invokeMethodAsync("updateElapsedTime", audio.currentTime);
    });
  }
}

function measure(id) {
  const element = document.getElementById(id);

  if (element) {
    const rect = element.getBoundingClientRect();
    return { width: rect.width, height: rect.height };
  }

  return { width: 0, height: 0 };
}

function observeResize(id, dotnetRef) {
  const element = document.getElementById(id);

  if (element) {
    const resizeObserver = new ResizeObserver(entries => {
      for (let entry of entries) {
        const rect = entry.contentRect;
        dotnetRef.invokeMethodAsync("onResize", rect.width, rect.height);
      }
    });

    resizeObserver.observe(element);
  }
}

function download(source, filename) {
  const a = document.createElement("a");
  document.body.appendChild(a);
  a.href = source;
  a.download = filename;
  a.target = "_blank";
  a.click();

  document.body.removeChild(a);
}

function play(audio) {
  if (audio) {
    audio.play();
  }
}

function pause(audio) {
  if (audio) {
    audio.pause();
  }
}

function setAudioSource(audio, source) {
  if (audio) {
    audio.src = source;
    audio.load();
  }
}

function togglePlayPause(audio) {
  if (audio) {
    if (audio.paused) {
      audio.play();
    } else {
      audio.pause();
    }
  }
}

function stop(audio) {
  if (audio) {
    audio.pause();
    audio.currentTime = 0;
  }
}

function setVolume(audio, volume) {
  if (audio) {
    audio.volume = volume;
  }
}

function seek(audio, time) {
  if (audio) {
    audio.currentTime = time;
  }
}

function dispose(audio) {
  if (!audio) {
    return;
  }

  stop(audio);

  audio.removeEventListener('loadedmetadata', () => {
    dotnetRef.invokeMethodAsync("setDuration", audio.duration);
  });

  audio.removeEventListener('ended', () => {
    dotnetRef.invokeMethodAsync("onTrackEnded");
  });

  audio.removeEventListener('seeked', () => {
    dotnetRef.invokeMethodAsync("setSeek", audio.currentTime);
  });

  audio.removeEventListener('timeupdate', () => {
    dotnetRef.invokeMethodAsync("updateElapsedTime", audio.currentTime);
  });
}

export const fluentCxAudio = {
  initialize: (element, dotNetRef) => initialize(element, dotNetRef),
  measure: (id) => measure(id),
  play: (id) => play(id),
  pause: (id) => pause(id),
  togglePlayPause: (id) => togglePlayPause(id),
  stop: (id) => stop(id),
  setVolume: (id, volume) => setVolume(id, volume),
  seek: (id, time) => seek(id, time),
  setAudioSource: (id, source) => setAudioSource(id, source),
  download: (source, filename) => download(source, filename),
  dispose: (id) => dispose(id),
  observeResize: (id, dotNetRef) => observeResize(id, dotNetRef),
  seekAndResume: async function (audio, time) {
    if (!audio) return false;

    try { audio.pause(); } catch { }

    if (!isFinite(audio.duration) || isNaN(audio.duration)) {
      await new Promise(resolve => {
        const onMeta = () => { audio.removeEventListener('loadedmetadata', onMeta); resolve(); };
        audio.addEventListener('loadedmetadata', onMeta);
        audio.load?.();
      });
    }

    const target = Math.min(Math.max(time, 0), audio.duration || time);
    if (typeof audio.fastSeek === 'function') {
      try { audio.fastSeek(target); } catch { audio.currentTime = target; }
    } else {
      audio.currentTime = target;
    }

    await new Promise((resolve) => {
      let done = false;
      const t = setTimeout(() => { if (!done) { done = true; cleanup(); resolve(); } }, 800);
      const onSeeked = () => { if (!done) { done = true; cleanup(); resolve(); } };
      const cleanup = () => { clearTimeout(t); audio.removeEventListener('seeked', onSeeked); };
      audio.addEventListener('seeked', onSeeked, { once: true });
    });

    const tryPlay = async () => {
      try { await audio.play(); } catch { }
      await new Promise((resolve) => {
        let done = false;
        const t = setTimeout(() => { if (!done) { done = true; cleanup(); resolve(); } }, 1500);
        const onPlaying = () => { if (!done) { done = true; cleanup(); resolve(); } };
        const onCanPlay = () => { if (!done) { done = true; cleanup(); resolve(); } };
        const cleanup = () => {
          clearTimeout(t);
          audio.removeEventListener('playing', onPlaying);
          audio.removeEventListener('canplay', onCanPlay);
        };
        audio.addEventListener('playing', onPlaying, { once: true });
        audio.addEventListener('canplay', onCanPlay, { once: true });
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
