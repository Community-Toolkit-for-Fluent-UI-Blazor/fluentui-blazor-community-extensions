const _resizeGroup = new Map();
const _intersectionGroup = new Map();
const _mutationGroup = new Map();
let _windowResizeHandler = null;

function debounce(callback, delay) {
  let timer;

  return (...args) => {
    clearTimeout(timer);
    timer = setTimeout(() => callback(...args), delay);
  };
}

function ensureResizeGroup(groupId, dotNetRef, options = {})
{
  if (!_resizeGroup.has(groupId)) {
    const elements = new Map();
    let pendingEntries = [];
    let scheduled = false;
    const delay = options.debounce || 0;

    const flush = () => {
      dotNetRef.invokeMethodAsync('OnResizeBatch', {
        entries: pendingEntries,
        groupId: groupId
      });

      pendingEntries = [];
      scheduled = false;
    };

    const scheduleFlush = delay > 0 ? debounce(flush, delay) : () => {
      if (!scheduled) {
        scheduled = true;
        requestAnimationFrame(flush);
      }
    }

    const observer = new ResizeObserver((entries) => {
      for (let entry of entries) {
        const id = entry.target.id;

        if (id) {
          const rect = entry.target.getBoundingClientRect();

          pendingEntries.push({
            id: id,
            width: entry.contentRect.width,
            height: entry.contentRect.height,
            rect: {
              top: rect.top,
              bottom: rect.bottom,
              left: rect.left,
              right: rect.right
            }
          });
        }
      }

      scheduleFlush();
    });

    _resizeGroup.set(groupId, { observer, elements });
  }
}

function ensureMutationGroup(groupId, dotNetRef, options = {}) {
  if (!_mutationGroup.has(groupId)) {
    const elements = new Map();
    let pendingEntries = [];
    let scheduled = false;
    const delay = options.debounce || 0;

    const flush = () => {
      dotNetRef.invokeMethodAsync('OnMutationBatch', {
        entries: pendingEntries,
        groupId: groupId
      });
      pendingEntries = [];
      scheduled = false;
    };

    const scheduleFlush = delay > 0 ? debounce(flush, delay) : () => {
      if (!scheduled) {
        scheduled = true;
        requestAnimationFrame(flush);
      }
    };

    const observer = new MutationObserver((mutations) => {
      for (let mutation of mutations) {
        const id = mutation.target.id;
        if (id) {
          pendingEntries.push({
            id: id,
            type: mutation.type,
            attributeName: mutation.attributeName || null,
            oldValue: mutation.oldValue || null,
            addedNodes: mutation.addedNodes ? [...mutation.addedNodes].map(n => n.nodeName) : [],
            removedNodes: mutation.removedNodes ? [...mutation.removedNodes].map(n => n.nodeName) : []
          });
        }
      }
      scheduleFlush();
    });

    _mutationGroup.set(groupId, { observer, elements });
  }
}

function registerMutation(groupId, id, element, dotNetRef, options = {}) {
  if (!element) return;
  ensureMutationGroup(groupId, dotNetRef, options);
  const group = _mutationGroup.get(groupId);
  group.elements.set(id, element);
  group.observer.observe(element, {
    attributes: options.attributes ?? true,
    childList: options.childList ?? true,
    subtree: options.subtree ?? true,
    characterData: options.characterData ?? false,
    attributeOldValue: options.attributeOldValue ?? false,
    characterDataOldValue: options.characterDataOldValue ?? false
  });
}

function unregisterMutation(groupId, id) {
  const group = _mutationGroup.get(groupId);
  if (group && group.elements.has(id)) {
    const element = group.elements.get(id);
    group.observer.disconnect();
    group.elements.delete(id);
  }
}

function registerResize(groupId, id, element, dotNetRef, options = {}) {
  if (!element) {
    return;
  }

  ensureResizeGroup(groupId, dotNetRef, options);
  const group = _resizeGroup.get(groupId);
  group.elements.set(id, element);
  group.observer.observe(element);
}

function unregisterResize(groupId, id) {
  const group = _resizeGroup.get(groupId);
  if (group && group.elements.has(id)) {
    const element = group.elements.get(id);
    group.observer.unobserve(element);
    group.elements.delete(id);
  }
}

function ensureIntersectionGroup(groupId, dotNetRef, options = {}) {
  if (!_intersectionGroup.has(groupId)) {
    const elements = new Map();
    let pendingEntries = [];
    let scheduled = false;
    const delay = options.debounce || 0;

    const flush = () => {
      dotNetRef.invokeMethodAsync('OnIntersectBatch', {
        entries: pendingEntries,
        groupId: groupId
      });

      pendingEntries = [];
      scheduled = false;
    };

    const scheduleFlush = delay > 0 ? debounce(flush, delay) : () => {
      if (!scheduled) {
        scheduled = true;
        requestAnimationFrame(flush);
      }
    }

    const observer = new IntersectionObserver((entries) => {
      for (let entry of entries) {
        const id = entry.target.id;

        if (id) {
          pendingEntries.push({
            id: id,
            isIntersecting: entry.isIntersecting,
            intersectionRatio: entry.intersectionRatio,
            boundingClientRect: {
              top: entry.boundingClientRect.top,
              left: entry.boundingClientRect.left,
              bottom: entry.boundingClientRect.bottom,
              right: entry.boundingClientRect.right,
              width: entry.boundingClientRect.width,
              height: entry.boundingClientRect.height
            },
            rootBounds: entry.rootBounds
              ? {
                top: entry.rootBounds.top,
                left: entry.rootBounds.left,
                bottom: entry.rootBounds.bottom,
                right: entry.rootBounds.right,
                width: entry.rootBounds.width,
                height: entry.rootBounds.height
              }
              : null,
            intersectionRect: {
              top: entry.intersectionRect.top,
              left: entry.intersectionRect.left,
              bottom: entry.intersectionRect.bottom,
              right: entry.intersectionRect.right,
              width: entry.intersectionRect.width,
              height: entry.intersectionRect.height
            }

          });
        }
      }

      scheduleFlush();
    }, { threshold: options.threshold ?? 0.4 });

    _intersectionGroup.set(groupId, { observer, elements });
  }
}


function registerIntersect(groupId, id, element, dotNetRef, options = {}) {
  if (!element) {
    return;
  }

  ensureIntersectionGroup(groupId, dotNetRef, options);
  const group = _intersectionGroup.get(groupId);
  group.elements.set(id, element);
  group.observer.observe(element);
}

function unregisterIntersect(groupId, id) {
  const group = _intersectionGroup.get(groupId);
  if (group && group.elements.has(id)) {
    const element = group.elements.get(id);
    group.observer.unobserve(element);
    group.elements.delete(id);
  }
}

function registerWindowResize(dotNetRef, options = {}) {
  if (_windowResizeHandler) {
    return;
  }

  const delay = options.debounce || 100;
  _windowResizeHandler = debounce(() => {
    dotNetRef.invokeMethodAsync("OnWindowResize", {
      width: window.innerWidth,
      height: window.innerHeight
    });
  }, delay);

  window.addEventListener("resize", _windowResizeHandler);
}

function unregisterWindowResize() {
  if (_windowResizeHandler) {
    window.removeEventListener("resize", _windowResizeHandler);
    _windowResizeHandler = null;
  }
}

export const fluentCxObserverProvider = {
    registerResize: (groupId, id, element, dotNetRef, options) => registerResize(groupId, id, element, dotNetRef, options),
    unregisterResize: (groupId, id) => unregisterResize(groupId, id),
    registerIntersect: (groupId, id, element, dotNetRef, options) => registerIntersect(groupId, id, element, dotNetRef, options),
    unregisterIntersect: (groupId, id) => unregisterIntersect(groupId, id),
    registerMutation: (groupId, id, element, dotNetRef, options) => registerMutation(groupId, id, element, dotNetRef, options),
    unregisterMutation: (groupId, id) => unregisterMutation(groupId, id),
    registerWindowResize: (dotNetRef, options) => registerWindowResize(dotNetRef, options),
    unregisterWindowResize: () => unregisterWindowResize()
};
