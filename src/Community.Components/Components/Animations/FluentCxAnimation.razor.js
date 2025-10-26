const _instances = {};

function createState(dotNetRef, options) {
  return {
    dotNetRef,
    rafId: 0,
    startTime: 0,
    pauseTime: 0,
    elapsedBeforePause: 0,
    elapsed: 0,
    progress: 0,
    duration: options.duration ?? 0,
    speed: options.speed ?? 1.0,
    loopMode: 'none',
    remainingLoops: 0,
    isRunning: false,
    isPaused: false,
    isStopped: true
  };
}

export function startAnimation(id, dotNetObject, options = {}) {
  if (!_instances[id]) {
    _instances[id] = createState(dotNetObject, options);
  }
  const state = _instances[id];

  setLoop(id, options.loop ?? false);
  state.speed = options.speed ?? 1.0;
  state.duration = options.duration ?? 0;
  state.elapsedBeforePause = 0;
  state.elapsed = 0;
  state.progress = 0;
  state.isPaused = false;
  state.isStopped = false;
  state.isRunning = true;
  state.startTime = performance.now();

  loop(id);
}

export function pauseAnimation(id) {
  const s = _instances[id];
  if (!s || s.isPaused || s.isStopped) return;
  s.isPaused = true;
  s.pauseTime = performance.now();
}

export function resumeAnimation(id) {
  const s = _instances[id];
  if (!s || !s.isPaused || s.isStopped) return;
  s.isPaused = false;
  s.startTime = performance.now();
}

export function stopAnimation(id, { reset = true } = {}) {
  const s = _instances[id];
  if (!s) return;
  s.isStopped = true;
  s.isRunning = false;
  s.isPaused = false;
  if (s.rafId) cancelAnimationFrame(s.rafId);
  if (reset) {
    s.startTime = 0;
    s.pauseTime = 0;
    s.elapsedBeforePause = 0;
    s.elapsed = 0;
    s.progress = 0;
  }
}

export function setDuration(id, duration) {
  const s = _instances[id];

  if (!s) {
    return;
  }

  s.duration = Math.max(0, duration | 0);
}

export function setLoop(id, value) {
  const s = _instances[id];
  if (!s) return;
  if (value === true) {
    s.loopMode = 'infinite';
    s.remainingLoops = Infinity;
  } else if (value === false) {
    s.loopMode = 'none';
    s.remainingLoops = 0;
  } else if (typeof value === 'number') {
    s.loopMode = value > 0 ? 'count' : 'none';
    s.remainingLoops = value;
  }
}

export function setSpeed(id, multiplier) {
  const s = _instances[id];
  if (!s) return;
  s.speed = Math.max(0.000001, Number(multiplier) || 1.0);
}

export function seekMs(id, ms) {
  const s = _instances[id];
  if (!s || s.duration <= 0) return;
  const clamped = Math.min(s.duration, Math.max(0, Number(ms) || 0));
  s.elapsed = clamped;
  s.progress = s.duration > 0 ? (s.elapsed / s.duration) : 0;
  s.elapsedBeforePause = s.elapsed;
  if (s.isRunning && !s.isPaused) {
    s.startTime = performance.now();
  }
}

function loop(id) {
  const s = _instances[id];
  if (!s || !s.isRunning) return;

  const now = performance.now();

  if (!s.isPaused && !s.isStopped) {
    const delta = Math.max(0, (now - s.startTime)) * s.speed;
    s.elapsed = s.elapsedBeforePause + delta;

    if (s.duration > 0) {
      if (s.elapsed >= s.duration) {
        const times = Math.floor(s.elapsed / s.duration);
        if (s.loopMode === 'none') {
          s.elapsed = s.duration;
          s.progress = 1;
          safeInvokeFrame(s);
          stopAnimation(id, { reset: false });
          s.dotNetRef.invokeMethodAsync('OnAnimationCompleted');
          return;
        } else {
          s.dotNetRef.invokeMethodAsync('OnLoopCompleted');
          advanceLoops(s, times, id);
          if (s.isStopped) return;
          s.elapsed = s.elapsed % s.duration;
          s.elapsedBeforePause = s.elapsed;
          s.startTime = now;
        }
      }

      s.progress = s.duration > 0 ? (s.elapsed / s.duration) : 0;
    }
    safeInvokeFrame(s);
  }

  s.rafId = requestAnimationFrame(() => loop(id));
}

function advanceLoops(s, times, id) {
  if (s.loopMode === 'infinite') return;
  if (s.loopMode === 'count') {
    s.remainingLoops -= times;
    if (s.remainingLoops <= 0) {
      s.elapsed = s.duration;
      s.progress = 1;
      safeInvokeFrame(s);
      stopAnimation(id, { reset: false });
    }
  }
}

function safeInvokeFrame(s) {
  if (s.dotNetRef && typeof s.dotNetRef.invokeMethodAsync === 'function') {
    try {
      s.dotNetRef.invokeMethodAsync('OnAnimationFrame').then(batch => {
        applyAnimationBatch(batch);
      });
    } catch { }
  }
}

function applyAnimationBatch(batch) {
  batch.forEach(item => {
    const el = document.getElementById(item.id);

    if (!el) {
      return;
    }

    let transform = "";

    if (item.x !== undefined || item.y !== undefined) {
      transform += `translate(${item.x ?? 0}px, ${item.y ?? 0}px) `;
    }

    if (item.sx !== undefined || item.sy !== undefined) {
      transform += `scale(${item.sx ?? 1}, ${item.sy ?? 1}) `;
    }

    if (item.r !== undefined) {
      transform += `rotate(${item.r}deg)`;
    }

    if (transform) {
      el.style.transform = transform;
    }

    if (item.o !== undefined) {
      el.style.opacity = item.o;
    }

    if (item.c !== undefined) {
      el.style.color = item.c;
    }

    if (item.bc !== undefined) {
      el.style.backgroundColor = item.bc;
    }
  });
}

export function dispose(id) {
  stopAnimation(id);
  delete _instances[id];
}
