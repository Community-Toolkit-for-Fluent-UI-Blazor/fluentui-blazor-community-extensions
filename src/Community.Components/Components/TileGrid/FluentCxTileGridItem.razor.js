const _instances = [];

export function initialize(id, dotNetReference) {
  var element = document.getElementById(id);

  if (!element) {
    return;
  }

  var parent = element.parentElement;
  const preview = element.getElementsByClassName('fluent-tile-grid-item-preview')[0];
  const main = element.getElementsByClassName('fluent-tile-grid-item-original')[0];

  _instances[id] = {
    minimumSize: 20,
    originalWidth: 0,
    originalHeight: 0,
    originalX: 0,
    originalY: 0,
    originalMouseX: 0,
    originalMouseY: 0,
    newWidth: 0,
    newHeight: 0,
    resizers: [],
    element: element,
    dotNetHelper: dotNetReference,
    parent: parent,
    orientation: 0,
    preview: preview,
    main: main
  };

  const resizers = element.querySelectorAll(".fluent-tile-grid-item-resize-handle");

  _instances[id].resizers = resizers;

  for (let i = 0; i < resizers.length; i++) {
    const current = resizers[i];
    current.addEventListener('mousedown', function (e) {
      beginResize(id, current, e);
    })
  }
}

function beginResize(id, current, e) {
  e.preventDefault();
  const instance = _instances[id];

  if (!instance) {
    return;
  }

  const { left, top, width, height } = instance.main.getBoundingClientRect();

  instance.originalWidth = width;
  instance.originalHeight = height;
  instance.originalX = left;
  instance.originalY = top;

  instance.originalMouseX = e.pageX;
  instance.originalMouseY = e.pageY;
  window.addEventListener('mousemove', resize);
  window.addEventListener('mouseup', stopResize);

  if (instance.preview) {
    instance.preview.style.width = instance.originalWidth + "px";
    instance.preview.style.height = instance.originalHeight + "px";
    instance.preview.style.left = instance.originalX + "px";
    instance.preview.style.top = instance.originalY + "px";
    instance.preview.style.display = 'block';
  }

  function resize(e) {
    const width = instance.originalWidth + (e.pageX - instance.originalMouseX);
    const height = instance.originalHeight + (e.pageY - instance.originalMouseY);

    if (current.classList.contains('fluent-tile-grid-cursor-nwse-resize')) {
      instance.orientation = 2;

      if (width > instance.minimumSize) {
        instance.newWidth = width
      }

      if (height > instance.minimumSize) {
        instance.newHeight = height
      }

      if (instance.preview) {
        instance.preview.style.width = width + "px";
        instance.preview.style.height = height + "px";
      }
    }
    else if (current.classList.contains('fluent-tile-grid-cursor-ns-resize')) {
      instance.orientation = 1;

      if (height > instance.minimumSize) {
        instance.newHeight = height
      }

      if (instance.preview) {
        instance.preview.style.height = height + "px";
        instance.preview.style.width = instance.originalWidth + "px";
      }
    }
    else if (current.classList.contains('fluent-tile-grid-cursor-ew-resize')) {
      const width = instance.originalWidth + (e.pageX - instance.originalMouseX)
      instance.orientation = 0;

      if (width > instance.minimumSize) {
        instance.newWidth = width
      }

      if (instance.preview) {
        instance.preview.style.width = width + "px";
        instance.preview.style.height = instance.originalHeight + "px";
      }
    }
  }

  function stopResize() {
    window.removeEventListener('mousemove', resize);
    window.removeEventListener('mouseup', stopResize);

    if (instance.preview) {
      instance.preview.style.display = 'none';
    }

    var mainBoundingRect = instance.parent.getBoundingClientRect();

    const value = {
      orientation: instance.orientation,

      original: {
        width: instance.originalWidth,
        height: instance.originalHeight,
        x: instance.originalX,
        y: instance.originalY,
      },

      newSize: {
        width: instance.newWidth,
        height: instance.newHeight,
      },

      parent: {
        width: mainBoundingRect.width,
        height: mainBoundingRect.height,
      }
    };

    instance.dotNetHelper.invokeMethodAsync('UpdateColumnAndRowSpan', value);
  }
}

export function columnAndRowSpanUpdated(id, columnSpan, rowSpan) {
  const instance = _instances[id];

  if (instance) {
    instance.element.style.gridColumnEnd = "span " + columnSpan;
    instance.element.style.gridRowEnd = "span " + rowSpan;

    var boundingRect = instance.main.getBoundingClientRect();

    instance.originalWidth = boundingRect.width;
    instance.originalHeight = boundingRect.height;

    if (instance.preview) {
      instance.preview.style.width = instance.originalWidth + "px";
      instance.preview.style.height = instance.originalHeight + "px";
    }

  }
}

export function destroy(id) {
  const instance = _instances[id];

  if (instance) {
    for (let i = 0; i < instance.resizers.length; ++i) {
      instance.resizers[i].removeEventListener("mousedown", function (e) {
        beginResize(id, instance.resizers[i], e);
      });
    }
  }
}
