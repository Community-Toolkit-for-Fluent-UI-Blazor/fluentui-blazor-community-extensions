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

export function initialize(id, dotnetReference, floatingButtonId, target, options) {
  const instance = getInstance(id);
  instance.target = document.getElementById(target);
  instance.floatingButton = document.getElementById(floatingButtonId);
  instance.isVertical = options.isVertical || false;
  instance.isFixed = options.isFixed || false;
  instance.isTop = options.isTop || false;
  instance.isMiddle = options.isMiddle || false;
  instance.isLeft = options.isLeft || false;
  instance.isCenter = options.isCenter || false;
  instance.direction = options.direction;
  instance.dotnetReference = dotnetReference;

  window.addEventListener('resize', () => setLinearPositionInternal(instance));
}

function setLinearPositionInternal(instance) {
  if (instance) {
    setLinearPosition(instance.id, instance.isVertical, instance.isFixed, instance.isTop, instance.isMiddle, instance.isLeft, instance.isCenter);
  }
}

export function updatePosition(id, options) {
  const instance = getInstance(id);
  instance.isVertical = options.isVertical;
  instance.isFixed = options.isFixed;
  instance.isTop = options.isTop;
  instance.isMiddle = options.isMiddle;
  instance.isLeft = options.isLeft;
  instance.isCenter = options.isCenter;
  instance.direction = options.direction;
}

export function setLinearPosition(id) {
  const instance = getInstance(id);

  if (instance) {
    const midOffsetHeight = instance.floatingButton.offsetHeight / 2;
    const midOffsetWidth = instance.floatingButton.offsetWidth / 2;
    let yOffset = 0;
    let xOffset = 0;

    if (instance.isTop) {
      yOffset = instance.floatingButton.offsetTop + (instance.isVertical ? instance.floatingButton.offsetHeight : 0);
    }
    else {
      let s = instance.isFixed ? window.document.documentElement.clientHeight : instance.target.clientHeight;
      yOffset = s - instance.floatingButton.offsetTop - (instance.isVertical ? 0 : instance.floatingButton.offsetHeight);
    }

    if (instance.isLeft) {
      xOffset = instance.floatingButton.offsetLeft + (instance.isVertical ? 0 : instance.floatingButton.offsetWidth);
    }
    else {
      let s = instance.isFixed ? window.document.documentElement.clientWidth : instance.target.clientWidth;
      xOffset = s - instance.floatingButton.offsetLeft - (instance.isVertical ? instance.floatingButton.offsetWidth : 0);
    }

    if (instance.isCenter) {
      const direction = instance.direction;

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

    if (instance.isMiddle) {
      const direction = instance.direction;

      if (direction === 3 || direction === 4) {
        yOffset = instance.floatingButton.offsetTop - midOffsetHeight;
      }
    }

    instance.element.style.setProperty('--sleekdial-vertical-offset', `${yOffset}px`);
    instance.element.style.setProperty('--sleekdial-horizontal-offset', `${xOffset}px`);
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

    instance.element.classList.remove('sleekdial-popup-hidden');
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
