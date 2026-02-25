let _dotNet = null;
let _container = null;
let _selector = null;
let _resizeObserver = null;
let _active = null;
const dayView = 0;
const weekView = 1;
const monthView = 2;
const timelineView = 4;
const drag = 0;
const resize = 1;

function resolveElement(maybeElOrSelector) {
  // If argument is already a DOM element, return it
  if (maybeElOrSelector && maybeElOrSelector instanceof HTMLElement) return maybeElOrSelector;

  // If arg is a string, treat as selector or id
  if (typeof maybeElOrSelector === 'string') {
    // prefer querySelector first, then getElementById fallback
    let el = document.querySelector(maybeElOrSelector);
    if (el) return el;
    return document.getElementById(maybeElOrSelector) || null;
  }

  // If arg is an object (ElementReference proxy), it may have an "id" property set by Blazor
  // or it may already be a reference usable by Blazor (in which case the runtime passes the real element).
  if (maybeElOrSelector && typeof maybeElOrSelector === 'object') {
    // try common patterns
    if (maybeElOrSelector.id && typeof maybeElOrSelector.id === 'string') {
      const byId = document.getElementById(maybeElOrSelector.id);
      if (byId) return byId;
    }

    // try to unwrap (Blazor WebAssembly/Server typically pass the element itself, so previous checks handled it)
    // if no luck, return null
    return null;
  }

  return null;
}

export function initDrop(containerOrSelector, selector) {
  const container = resolveElement(containerOrSelector) || document.body;
  _container = container;
  _selector = selector || '.scheduler-content';

  container.addEventListener('dragover', (ev) => { ev.preventDefault(); });
}

export function disposeDrop() {
  _container.removeEventListener('dragover', (ev) => { });
}

export function observeResize(containerOrSelector, selector, dotNetRef) {
  const container = resolveElement(containerOrSelector) || document.body;
  _container = container;
  _selector = selector || '.scheduler-content';
  _dotNet = dotNetRef;

  const content = container.querySelector(_selector) || container;
  if (!content) return;

  if ('ResizeObserver' in window) {
    _resizeObserver = new ResizeObserver(entries => {
      dotNetRef.invokeMethodAsync('Scheduler_OnContentResize').catch(() => { });
    });
    _resizeObserver.observe(content);
  } else {
    window.addEventListener('resize', () => dotNetRef.invokeMethodAsync('Scheduler_OnContentResize').catch(() => { }));
  }
}

export function disposeObserve() {
  if (_resizeObserver) { _resizeObserver.disconnect(); _resizeObserver = null; }
  _dotNet = null;
  _container = null;
  _selector = null;
}

export function getBoundingClientRect(containerOrElement) {
  const container = resolveElement(containerOrElement) || document.body;

  if (!container) return null;

  return container.getBoundingClientRect();
}

export function getTimelineRect(containerOrElement) {
  const container = resolveElement(containerOrElement) || document.body;

  if (!container)
    return null;

  const rect = container.getBoundingClientRect();

  return rect;
}

export function getMonthGridRect(containerOrElement) {
  const container = resolveElement(containerOrElement) || document.body;

  if (!container)
    return null;

  const grid = container.querySelector('.scheduler-month-grid');

  if (!grid) {
    return null;
  }

  const rect = grid.getBoundingClientRect();

  return rect;
}

export async function measureLayout(containerOrElement, selector, view) {
  return new Promise(resolve => {
    const container = resolveElement(containerOrElement) || document.body;
    const selectorStr = selector || '.scheduler-content';
    const content = container.querySelector(selectorStr) || container;
    const contRect = container.getBoundingClientRect();
    const contScrollLeft = container.scrollLeft || 0;
    const contScrollTop = container.scrollTop || 0;
    let contentRect = content.getBoundingClientRect();

    const localLeft = contentRect.left - contRect.left + contScrollLeft;
    const localTop = contentRect.top - contRect.top + contScrollTop;

    // valeurs communes
    let labelWidth = 0;
    let headerHeight = 0;
    let cellWidth = 0;
    let cellHeight = 0;
    let gap = 0;
    let labelHeight = 0;
    let usableHeight = 0;
    let paddingTop = 0, paddingBottom = 0, paddingLeft = 0, paddingRight = 0;

    if (view === dayView) {
      labelWidth = 135;
    }
    else if (view === monthView) {
      const grid = container.querySelector('.scheduler-month-grid');
      const firstCell = grid?.querySelector('.scheduler-day-cell');
      const label = firstCell?.querySelector('.scheduler-day-label');
      if (grid && firstCell && label) {
        const gridStyle = window.getComputedStyle(grid);
        gap = parseFloat(gridStyle.gap || gridStyle.rowGap || '0');

        const cellRect = firstCell.getBoundingClientRect();
        const labelRect = label.getBoundingClientRect();
        const labelStyle = window.getComputedStyle(label);
        const cellStyle = window.getComputedStyle(firstCell);

        const marginTop = parseFloat(labelStyle.marginTop || '0');
        const marginBottom = parseFloat(labelStyle.marginBottom || '0');
        paddingTop = parseFloat(cellStyle.paddingTop || '0');
        paddingBottom = parseFloat(cellStyle.paddingBottom || '0');
        paddingLeft = parseFloat(cellStyle.paddingLeft || '0');
        paddingRight = parseFloat(cellStyle.paddingRight || '0');

        labelHeight = labelRect.height + marginTop + marginBottom + paddingTop;
        cellHeight = cellRect.height;
        usableHeight = cellHeight - labelHeight - paddingBottom;
        cellWidth = cellRect.width;
      }
    }
    else if (view === weekView) {
      const grid = container.querySelector('.scheduler-week-grid');
      const firstCell = grid?.querySelector('.scheduler-week-day');
      const labelCell = grid?.querySelector('.scheduler-week-label');
      const header = container.querySelector('.scheduler-week-header');
      if (grid && firstCell && labelCell && header) {
        const headerRect = header.getBoundingClientRect();
        const cellRect = firstCell.getBoundingClientRect();
        const labelRect = labelCell.getBoundingClientRect();
        const cellStyle = window.getComputedStyle(firstCell);

        paddingTop = parseFloat(cellStyle.paddingTop || '0');
        paddingBottom = parseFloat(cellStyle.paddingBottom || '0');
        paddingLeft = parseFloat(cellStyle.paddingLeft || '0');
        paddingRight = parseFloat(cellStyle.paddingRight || '0');

        labelWidth = labelRect.width * 2;
        cellWidth = cellRect.width;
        cellHeight = cellRect.height;
        headerHeight = headerRect.height;
      }
    }
    else if (view === timelineView) {
      const header = container.querySelector('.scheduler-timeline-full-header');
      if (header) {
        const headerRect = header.getBoundingClientRect();
        headerHeight = headerRect.height;
      }

      labelWidth = 0;

      const firstCell = container.querySelector('.scheduler-timeline-hour');
      if (firstCell) {
        const cellRect = firstCell.getBoundingClientRect();
        cellWidth = cellRect.width;
        cellHeight = cellRect.height;
      }
    }

    requestAnimationFrame(() => {
      const body = container.querySelector('.scheduler-timeline');
      if (body) {
        const gridWidthAttr = parseFloat(body.style.width || '0');
        const gridScrollWidth = body.scrollWidth;
        const realGridWidth = Math.max(gridWidthAttr, gridScrollWidth);
        contentRect.width = realGridWidth;
      }

      const overlayLeft = view === timelineView ? 0 : localLeft + labelWidth;
      const overlayTop = localTop + headerHeight;
      const overlayWidth = Math.max(0, contentRect.width - labelWidth);
      const overlayHeight = contentRect.height - headerHeight;

      resolve({
        overlay: {
          x: overlayLeft,
          y: overlayTop,
          width: overlayWidth,
          height: overlayHeight
        },
        cellSize: {
          width: cellWidth,
          height: cellHeight
        },
        labelSize: {
          width: labelWidth,
          height: labelHeight
        },
        contentSize: {
          x: contRect.left,
          y: contRect.top,
          width: contRect.width,
          height: contRect.height
        },
        padding: {
          top: paddingTop,
          bottom: paddingBottom,
          left: paddingLeft,
          right: paddingRight
        },
        gap: gap,
        headerHeight: headerHeight,
        usableHeight: usableHeight,
        local: {
          x: localLeft,
          y: localTop
        }
      });
    });
  });
}

export function startPointerTracking(dotNetRef, itemId, mode) {
  _active = { dotNetRef, itemId, mode };

  function moveHandler(e) {
    if (!_active) {
      return;
    }

    e.preventDefault();

    _active.dotNetRef.invokeMethodAsync(
      'OnPointerMove',
      _active.itemId,
      _active.mode,
      e.clientX,
      e.clientY,
      e.pointerType
    ).catch(() => { });
  }

  function upHandler(e) {
    if (!_active) {
      return;
    }

    _active.dotNetRef.invokeMethodAsync(
      'OnPointerUp',
      _active.itemId,
      _active.mode,
      e.pointerType
    ).catch(() => { });

    document.removeEventListener('pointermove', moveHandler);
    document.removeEventListener('pointerup', upHandler);
    _active = null;
  }

  document.addEventListener('pointermove', moveHandler);
  document.addEventListener('pointerup', upHandler);
}

export function stopPointerTracking() {
  _active = null;
}

export function getScrollLeft(containerOrElement) {
  const container = resolveElement(containerOrElement) || document.body;

  if (!container) return 0;

  return container.scrollLeft || 0;
}

