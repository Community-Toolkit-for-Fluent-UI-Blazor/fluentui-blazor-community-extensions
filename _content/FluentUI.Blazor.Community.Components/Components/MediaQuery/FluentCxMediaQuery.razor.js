const _instances = [];

export function initialize(id, dotnetHelper, query) {
  const mediaQuery = window.matchMedia(query);
  const callback = e => dotnetHelper.invokeMethodAsync('OnMediaQueryChangedAsync', e.matches);
  mediaQuery.addEventListener('change', callback);
  _instances[id] = {
    callback: callback,
    mediaQuery: mediaQuery,
  }

  return mediaQuery.matches;
}


export function dispose(id) {
  const instance = _instances[id];

  if (instance) {
    instance.mediaQuery.removeEventListener('change', instance.callback);
    _instances.splice(id, 1);
  }
}
