const _instances = {};

export function initialize(id, dotNetHelper) {
  _instances[id] = {
    elements: {},
    dotNetHelper: dotNetHelper,
    ro: new ResizeObserver(entries => {
      for (let entry of entries) {
        const width = entry.contentRect.width;
        const height = entry.contentRect.height;
        dotNetHelper.invokeMethodAsync('OnResize', entry.target.id, Math.round(width), Math.round(height));
      }
    })
  };
}

export function addObserver(providerId, id) {
  if (_instances[providerId]) {
    const element = document.getElementById(id);
    _instances[providerId].elements[id] = element;
    _instances[providerId].ro.observe(element);
  }
}

export function measureElement(providedId, id) {
  if (_instances[providedId] && _instances[providedId].elements[id]) {
    const element = _instances[providedId].elements[id];
    const rect = element.getBoundingClientRect();

    _instances[providedId].dotNetHelper.invokeMethodAsync('OnMeasured', id, Math.round(rect.width), Math.round(rect.height));
  }
}

export function removeObserver(providerId, id) {
  if (_instances[providerId]) {
    const resizeObserver = _instances[providerId].ro;
    const elements = _instances[providerId].elements;

    if (elements[id]) {
      resizeObserver.unobserve(elements[id]);
      delete elements[id];
    }
  }
}

export function clearObservers() {
  for (let [id, elements, resizeObserver] of _instances) {
    for (let element in elements) {
      resizeObserver.unobserve(element);
    }

    resizeObserver.disconnect();
    delete _instances[id];
  }

  _instances.forEach(instance => {
    instance.elements.forEach(element => {
      instance.ro.unobserve(element);
    });
    instance.ro.disconnect();
    instance.elements = {};
  });
  
  _instances.clear();
}
