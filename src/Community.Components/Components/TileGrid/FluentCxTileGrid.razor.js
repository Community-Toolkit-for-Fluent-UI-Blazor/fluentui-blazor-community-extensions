const _tileGridInstances = [];

export function initialize(id) {
  const element = document.getElementById(id);

  if (!element) {
    return;
  }

  _tileGridInstances[id] = element;
}

export function swap(id, sourceId, targetId) {
  const instance = _tileGridInstances[id];

  if (!instance) {
    return;
  }

  var source = document.getElementById(sourceId);

  if (!source) {
    return;
  }

  var target = document.getElementById(targetId);

  if (!target) {
    return;
  }

  instance.insertBefore(target, source);
  instance.insertBefore(source, target.nextSibling);
}
