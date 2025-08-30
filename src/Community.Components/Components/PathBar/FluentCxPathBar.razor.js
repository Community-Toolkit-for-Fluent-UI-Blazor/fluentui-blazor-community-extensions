export function initialize(id, dotNetReference, mutationConfiguration) {
  const element = document.getElementById(id);
  const itemContainer = document.getElementById(`fluentcx-path-bar-container-${id}`);

  if (element && itemContainer) {
    const observer = new ResizeObserver((entries) => {
      dotNetReference.invokeMethodAsync('OnResize', entries[0].contentRect.width);
    });

    observer.observe(itemContainer);

    const mutationObserver = new MutationObserver((mutationsList) => {
      dotNetReference.invokeMethodAsync('OnMutated');
    });

    mutationObserver.observe(itemContainer, mutationConfiguration || {
      attributes: true,
      childList: true,
      subtree: false,
      attributeFilter: ['class', 'style']
    });

    const instance = {
      id: id,
      element: element,
      itemContainer: itemContainer,
      dotNetReference: dotNetReference,
      mutationObserver: mutationObserver,
      resizeObserver: observer,
    };

    window.fluentCxComponentInstances.addInstance(id, instance);
  }
}

export function getWidth(id) {
  var documentElement = document.getElementById(id);

  if (documentElement) {
    return documentElement.getBoundingClientRect().width;
  };

  return 0.0;
}

export function dispose(id) {
  const instance = window.fluentCxComponentInstances.getInstance(id);

  if (instance) {
    if (instance.resizeObserver) {
      instance.resizeObserver.disconnect();
      instance.resizeObserver = null;
    }

    if (instance.mutationObserver) {
      instance.mutationObserver.disconnect();
      instance.mutationObserver = null;
    }

    window.fluentCxComponentInstances.removeInstance(id);
  }
}
