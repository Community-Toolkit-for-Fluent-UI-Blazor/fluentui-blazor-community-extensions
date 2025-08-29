const truncateMiddle = (text) => {
  const maxLength = instance.maxLengthBeforeTruncate;

  if (text.length <= maxLength) {
    return text;
  }

  const part = Math.floor((maxLength - 3) / 2);
  return text.slice(0, part) + '...' + text.slice(-part);
};

export function initialize(id, dotNetReference, maxLengthBeforeTruncate) {
  const element = document.getElementById(id);
  const hiddenPathBar = document.getElementById(`hidden-menu-container-${id}`);

  if (element && hiddenPathBar) {
    const resizeObserver = new ResizeObserver(() => refreshPathBar(id));
    const observable = new IntersectionObserver((entries) => {
      const lastEntry = entries.find(e => e.target === instance.hiddenPathBar.lastElementChild);

      if (!lastEntry || lastEntry.isIntersecting) {
        instance.hiddenPathBar.style.display = 'none';

        const value = {
          overflowItems: instance.overflowItems.map(i => i.id),
          visibleItems: instance.visibleItems.map(i => i.id),
          lastVisibleItemText: ''
        };

        instance.dotNetReference.invokeMethodAsync('updateOverflowAndVisible', value);

        return;
      }

      while (!lastEntry.isIntersecting && visibleItems.length > 1) {
        const itemToMove = visibleItems.pop();
        instance.overflowItems.push(itemToMove);
        itemToMove.style.display = 'none';
        observable.unobserve(itemToMove);
      };

      let truncated = '';

      if (visibleItems.length === 1 && !lastEntry.isIntersecting) {
        truncated = truncateMiddle(visibleItems[0].textContent);
      }

      instance.hiddenPathBar.style.display = 'none';

      const value = {
        overflowItems: instance.overflowItems.map(i => i.id),
        visibleItems: instance.visibleItems.map(i => i.id),
        lastVisibleItemText: truncated
      };

      instance.dotNetReference.invokeMethodAsync('updateOverflowAndVisible', value);

    }, { root: hiddenPathBar, threshold: 1.0 });

    const instance = {
      maxLengthBeforeTruncate: maxLengthBeforeTruncate || 30,
      hiddenPathBar: hiddenPathBar,
      dotNetReference: dotNetReference,
      resizeObserver: resizeObserver,
      observable: observable,
      children: [],
      overflowItems: [],
      visibleItems: []
    };

    resizeObserver.observe(hiddenPathBar);

    window.fluentCxComponentInstances.addInstance(id, instance);

    refreshPathBar(id);
  };
}

export function refreshPathBar(id) {
  const instance = window.fluentCxComponentInstances.getInstance(id);

  if (instance) {
    instance.hiddenPathBar.style.display = 'block';

    if (instance.children.length > 0) {
      instance.children.forEach(child => instance.observable.unobserve(child));
    }

    instance.children = Array.from(instance.hiddenPathBar.children);

    if (instance.children.length > 0) {
      instance.visibleItems = [...instance.children];
      instance.overflowItems = [];
      instance.children.forEach(child => instance.observable.observe(child));
    }
  }
}

export function dispose(id) {
  const instance = window.fluentCxComponentInstances.getInstance(id);

  if (instance) {
    instance.children.forEach(child => instance.observable.unobserve(child));
    instance.observable.disconnect();
    instance.resizeObserver.disconnect();
    window.fluentCxComponentInstances.removeInstance(id);
  }
}
