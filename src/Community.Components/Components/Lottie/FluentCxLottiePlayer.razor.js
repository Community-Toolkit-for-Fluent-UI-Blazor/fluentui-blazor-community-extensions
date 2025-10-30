const _instances = {};

function load(id, dotNetRef, path, loop, autoplay, speed, renderer) {
  if (!window.lottie) {
    console.error("Lottie library is not loaded.");
    return;
  }

  const container = document.getElementById(id);

  if (container) {
    // Clear previous animation if exists
    container.innerHTML = '';

    const animation = lottie.loadAnimation({
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

function play(id) {
  const animation = _instances[id];

  if (animation) {
    animation.play();
  }
}

function pause(id) {
  const animation = _instances[id];

  if (animation) {
    animation.pause();
  }
}

function stop(id) {
  const animation = _instances[id];

  if (animation) {
    animation.stop();
  }
}

function setSpeed(id, speed) {
  const animation = _instances[id];

  if (animation) {
    animation.setSpeed(speed);
  }
}

function setDirection(id, direction) {
  const animation = _instances[id];

  if (animation) {
    animation.setDirection(direction);
  }
}

function playSegments(id, start, end, forceFlag) {
  const animation = _instances[id];
  if (animation) {
    animation.playSegments([start, end], forceFlag);
  }
}

function dispose(id) {
  const animation = _instances[id];
  if (animation) {
    animation.destroy();
    delete _instances[id];
  }
}

export const fluentcxLottiePlayer = {
  load: (id, dotnetRef, path, loop, autoplay, speed, renderer) => load(id, dotnetRef, path, loop, autoplay, speed, renderer),
  play: (id) => play(id),
  pause: (id) => pause(id),
  stop: (id) => stop(id),
  setSpeed: (id, speed) => setSpeed(id, speed),
  setDirection: (id, direction) => setDirection(id, direction),
  playSegments: (id, start, end, forceFlag) => playSegments(id, start, end, forceFlag),
  dispose: (id) => dispose(id)
};
