const _resizerComponents = [];
const minimumSize = 20;

export function initialize(id, dotNetHelper, tileGridId) {
  const element = document.getElementById(id);

  if (!element) {
    return;
  }

  const child = element.getElementsByClassName('fluentcx-resizer-child-content-container')[0];
  const resizers = element.querySelectorAll(".fluentcx-resizer-handler");
  const tileGrid = document.getElementById(tileGridId);
  const dropZone = tileGrid ? element.parentElement : null; // If it is in tilegrid, get the parent (the drop zone)

  const { left, top, width, height } = element.getBoundingClientRect();

  const instance = {
    id: id,
    dotNetHelper: dotNetHelper,
    element: element,
    originalWidth: width,
    originalHeight: height,
    orientation: -1,
    originalMouseX: 0,
    originalMouseY: 0,
    child: child,
    resizers: resizers,
    tileGrid: tileGrid,
    dropZone: dropZone
  };

  _resizerComponents.push(instance);

  for (let i = 0; i < resizers.length; i++) {
    const current = resizers[i];
    current.addEventListener('mousedown', function (e) {
      beginResize(id, current, e);
    })
  }
}

export function setTileGrid(id, spanGridId) {
  for (var i = 0; i < _resizerComponents.length; i++) {
    if (_resizerComponents[i].id == id) {
      _resizerComponents[i].tileGrid = document.getElementById(spanGridId);
      _resizerComponents[i].dropZone = _resizerComponents[i].element.parentElement;
    }
  }
}

function getFromId(id) {
  for (let i = 0; i < _resizerComponents.length; ++i) {
    if (_resizerComponents[i].id == id) {
      return _resizerComponents[i];
    }
  }

  return null;
}

function beginResize(id, current, e) {
  e.preventDefault();
  const instance = getFromId(id);

  if (!instance) {
    return;
  }

  window.addEventListener('mousemove', resize);
  window.addEventListener('mouseup', stopResize);
  window.addEventListener('keydown', cancelResize);


  instance.originalMouseX = e.pageX;
  instance.originalMouseY = e.pageY;
  instance.element.style.backgroundColor = "var(--neutral-layer-1)";

  function cancelResize() {
    window.removeEventListener('keydown', cancelResize);
    window.removeEventListener('mousemove', resize);
    window.removeEventListener('mouseup', stopResize);

    instance.element.style.width = instance.originalWidth + "px";
    instance.element.style.height = instance.originalHeight + "px";
  }

  function resizeHorizontally(width, gridColumnWidth, maxWidth) {
    var newSpan = Math.ceil(width / gridColumnWidth);

    if (newSpan <= 1) {
      newSpan = 1;
      width = "-";
    }
    else if (width >= maxWidth) {
      width = maxWidth;
      newSpan = Math.ceil(width / gridColumnWidth);
    }

    instance.dropZone.style.gridColumnEnd = "span " + newSpan;
    instance.element.style.width = width == '-' ? "100%" : width + "px";
  }

  function resizeVertically(height, gridRowHeight, maxHeight) {
    var newSpan = Math.ceil(height / gridRowHeight);

    if (newSpan <= 1) {
      newSpan = 1;
      height = '-';
    }
    else if (height >= maxHeight) {
      width = maxHeight;
      newSpan = Math.ceil(height / gridRowHeight);
    }

    instance.dropZone.style.gridRowEnd = "span " + newSpan;
    instance.element.style.height = height == '-' ? "100%" : height + "px";
  }

  function resize(e) {
    if (instance.tileGrid != null &&
      instance.tileGrid !== undefined &&
      instance.dropZone != null &&
      instance.dropZone !== undefined) {
      let width = instance.originalWidth + (e.pageX - instance.originalMouseX);
      let height = instance.originalHeight + (e.pageY - instance.originalMouseY);
      const rect = instance.dropZone.getBoundingClientRect();
      const columnEnd = parseInt(instance.dropZone.style.gridColumnEnd.replace('span', '').trim());
      const rowEnd = parseInt(instance.dropZone.style.gridRowEnd.replace('span', '').trim());
      const gridColumnWidth = rect.width / columnEnd;
      const gridRowHeight = rect.height / rowEnd;
      const maxSize = instance.tileGrid.getBoundingClientRect();
      instance.dropZone.style.backgroundColor = "var(--neutral-layer-1)";

      if (current.classList.contains('fluentcx-resizer-handler-cursor-nwse')) {
        instance.orientation = 2;
        resizeHorizontally(width, gridColumnWidth, maxSize.width);
        resizeVertically(height, gridRowHeight, maxSize.height);
      }
      else if (current.classList.contains('fluentcx-resizer-handler-cursor-ns')) {
        instance.orientation = 1;
        resizeVertically(height, gridRowHeight, maxSize.height);        
      }
      else if (current.classList.contains('fluentcx-resizer-handler-cursor-ew')) {
        instance.orientation = 0;
        resizeHorizontally(width, gridColumnWidth, maxSize.width);
      }
    }
    else {
      const width = instance.originalWidth + (e.pageX - instance.originalMouseX);
      const height = instance.originalHeight + (e.pageY - instance.originalMouseY);

      if (current.classList.contains('fluentcx-resizer-handler-cursor-nwse')) {
        instance.orientation = 2;

        if (width > minimumSize) {
          instance.newWidth = width;
        }

        if (height > minimumSize) {
          instance.newHeight = height;
        }

        instance.element.style.height = height + "px";
        instance.element.style.width = width + "px";
      }
      else if (current.classList.contains('fluentcx-resizer-handler-cursor-ns')) {
        instance.orientation = 1;

        if (height > minimumSize) {
          instance.newHeight = height
        }

        if (instance.element) {
          instance.element.style.height = height + "px";
          instance.element.style.width = instance.originalWidth + "px";
        }
      }
      else if (current.classList.contains('fluentcx-resizer-handler-cursor-ew')) {
        instance.orientation = 0;

        if (width > minimumSize) {
          instance.newWidth = width
        }

        if (instance.element) {
          instance.element.style.height = instance.originalHeight + "px";
          instance.element.style.width = width + "px";
        }
      }
    }
  }

  function stopResize() {
    window.removeEventListener('keydown', cancelResize);
    window.removeEventListener('mousemove', resize);
    window.removeEventListener('mouseup', stopResize);

    const value = {
      id: instance.id,
      orientation: instance.orientation,
      originalSize: {
        width: instance.originalWidth,
        height: instance.originalHeight
      },
      newSize: {
        width: instance.newWidth,
        height: instance.newHeight
      }
    }

    instance.dotNetHelper.invokeMethodAsync('Resized', value);

    if (instance.tileGrid != null) {
      instance.dropZone.style.backgroundColor = 'transparent';
      instance.element.style.width = "100%";
      instance.element.style.height = "100%";
      const { left, top, width, height } = instance.element.getBoundingClientRect();
      instance.originalWidth = width;
      instance.originalHeight = height;
    }
    else {
      instance.element.style.backgroundColor = 'transparent';
      const { left, top, width, height } = instance.element.getBoundingClientRect();
      instance.originalWidth = width;
      instance.originalHeight = height;
    }
  }
}

export function destroy(id) {
  for (let i = _resizerComponents.length - 1; i > 0; i--) {
    if (_resizerComponents[i].id == id) {
      for (let j = 0; j < _resizerComponents[i].resizers.length; ++j) {
        _resizerComponents[i].resizers[j].removeEventListener("mousedown", function (e) {
          beginResize(id, _resizerComponents[i].resizers[j], e);
        });
      }

      _resizerComponents.splice(i, 1);
    }
  }
}
