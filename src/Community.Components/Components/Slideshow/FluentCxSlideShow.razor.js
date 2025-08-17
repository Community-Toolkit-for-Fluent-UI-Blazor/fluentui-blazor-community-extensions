const _instances = [];

function getInstance(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      return _instances[i];
    }
  }

  return null;
}

export function initialize(id, dotnetReference) {
  const element = document.getElementById(id);
  const instance = {
    id: id,
    element: element,
    dotnetReference: dotnetReference,
    parent: element.parentElement
  }

  _instances.push(instance);
}

export function autoSize(id) {
  const instance = getInstance(id);

  if (instance) {
    const rect = instance.parent.getBoundingClientRect();
    instance.dotnetReference.invokeMethodAsync('getParentSize', rect.width, rect.height);
  }
}

export function destroy(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      _instances.splice(i, 1);
    }
  }
}
