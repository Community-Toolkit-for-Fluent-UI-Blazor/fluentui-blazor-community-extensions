const _instances = [];

function getInstance(id) {
  for (let i = 0; i < _instances.length; i++) {
    if (_instances[i].id === id) {
      return _instances[i];
    }
  }

  const element = document.getElementById(id);
  const value = {
    id: id,
    element: element,
    target: null,
    floatingButton: null
  };

  _instances.push(value);
  return value;
}

export function initialize(id, dotnetReference, floatingButtonId, target, linearOptions, radialOptions) {
  const instance = getInstance(id);
  instance.target = document.getElementById(target);
  instance.floatingButton = document.getElementById(floatingButtonId);
  instance.linearOptions = linearOptions;
  instance.radialOptions = radialOptions;
  instance.dotnetReference = dotnetReference;

  window.addEventListener('resize', () => {
    if (instance.isLinear) {
      setLinearPositionInternal(instance);
    }
    else {
      setRadialPositionInternal(instance);
    }
  });
}

function setLinearPositionInternal(instance) {
  if (instance) {
    setLinearPosition(instance.id);
  }
}

function setRadialPositionInternal(instance) {
  if (instance) {
    setRadialPosition(instance.id);
  }
}

export function updateLinearPosition(id, options) {
  const instance = getInstance(id);
  instance.linearOptions = options;
}

export function updateRadialPosition(id, options) {
  const instance = getInstance(id);
  instance.radialOptions = options;
}

export function setLinearPosition(id) {
  const instance = getInstance(id);

  if (instance) {
    const midOffsetHeight = instance.floatingButton.offsetHeight / 2;
    const midOffsetWidth = instance.floatingButton.offsetWidth / 2;
    let yOffset = 0;
    let xOffset = 0;

    if (instance.linearOptions.isTop) {
      yOffset = instance.floatingButton.offsetTop + (instance.linearOptions.isVertical ? instance.floatingButton.offsetHeight : 0);
    }
    else {
      let s = instance.linearOptions.isFixed ? window.document.documentElement.clientHeight : instance.target.clientHeight;
      yOffset = s - instance.floatingButton.offsetTop - (instance.linearOptions.isVertical ? 0 : instance.floatingButton.offsetHeight);
    }

    if (instance.linearOptions.isLeft) {
      xOffset = instance.floatingButton.offsetLeft + (instance.linearOptions.isVertical ? 0 : instance.floatingButton.offsetWidth);
    }
    else {
      let s = instance.linearOptions.isFixed ? window.document.documentElement.clientWidth : instance.target.clientWidth;
      xOffset = s - instance.floatingButton.offsetLeft - (instance.linearOptions.isVertical ? instance.floatingButton.offsetWidth : 0);
    }

    if (instance.linearOptions.isCenter) {
      const direction = instance.linearOptions.direction;

      if (direction === 0 || direction === 1 || direction === 2) {
        xOffset = instance.floatingButton.offsetLeft - midOffsetWidth;
      }
      else if (direction === 4) {
        xOffset = instance.floatingButton.offsetLeft + midOffsetWidth;
      }
      else {
        xOffset += midOffsetWidth;
      }
    }

    if (instance.linearOptions.isMiddle) {
      const direction = instance.linearOptions.direction;

      if (direction === 3 || direction === 4) {
        yOffset = instance.floatingButton.offsetTop - midOffsetHeight;
      }
      else if (direction === 0 || direction === 1) {
        yOffset = instance.floatingButton.offsetTop + midOffsetHeight;
      }
    }

    instance.element.style.setProperty('--sleekdial-vertical-offset', `${yOffset}px`);
    instance.element.style.setProperty('--sleekdial-horizontal-offset', `${xOffset}px`);
  }
}

export function setRadialPosition(id) {
  const instance = getInstance(id);

  if (instance) {
    var item = instance.element.querySelector(".sleekdialitem-radial");
    const width = item.offsetWidth;
    const height = item.offsetHeight;
    let yOffset = -1;
    let xOffset = -1;

    if (instance.radialOptions.isMiddle) {
      const height = instance.isFixed ? window.innerHeight : instance.target.clientHeight;
      yOffset = (height - instance.floatingButton.offsetTop - instance.element.offsetHeight / 2);
    }

    if (instance.radialOptions.isCenter) {
      const w = instance.isFixed ? window.innerWidth : instance.target.clientWidth;
      xOffset = (w - instance.floatingButton.offsetLeft - instance.element.offsetWidth / 2);
    }

    if (instance.radialOptions.isRight) {
      const w = instance.isFixed ? window.innerWidth : instance.target.clientWidth;
      xOffset = w;
    }

    if (instance.radialOptions.isBottom) {
      yOffset = instance.floatingButton.offsetTop;
    }

    const value = {
      x: xOffset,
      y: yOffset,
      width: width,
      height: height
    }

    instance.dotnetReference.invokeMethodAsync('RadialPositionUpdated', value);
  }
}

export function animateOpen(id, animation, duration, delay) {
  const instance = getInstance(id);
  if (instance) {
    instance.element.addEventListener('animationend', () => {
      instance.element.classList.remove('animation-' + animation + 'in');
      instance.element.removeEventListener('animationend', () => { });
      instance.dotnetReference.invokeMethodAsync('OnAnimationCompletedAsync', true);
    });

    instance.element.addEventListener('animationstart', () => {
      instance.element.classList.remove('sleekdial-popup-hidden');
    });

    instance.element.style.setProperty('--animation-duration', `${duration}ms`);
    instance.element.style.setProperty('--animation-delay', `${delay}ms`);
    instance.element.classList.add('animation-' + animation + 'in');
  }
}

export function animateClose(id, animation, duration, delay) {
  const instance = getInstance(id);
  if (instance) {
    instance.element.addEventListener('animationend', () => {
      instance.element.classList.add('sleekdial-popup-hidden');
      instance.element.classList.remove('animation-' + animation + 'out');
      instance.element.removeEventListener('animationend', () => { });
      instance.dotnetReference.invokeMethodAsync('OnAnimationCompletedAsync', false);
    });

    instance.element.style.setProperty('--animation-duration', `${duration}ms`);
    instance.element.style.setProperty('--animation-delay', `${delay}ms`);
    instance.element.classList.add('animation-' + animation + 'out');
  }
}


export function destroy(id) {
  for (let i = _instances.length - 1; i > 0; i--) {
    if (_instances[i].id === id) {
      window.removeEventListener('resize', () => setLinearPositionInternal(_instances[i]));
      _instances.splice(i, 1);
    }
  }
}
