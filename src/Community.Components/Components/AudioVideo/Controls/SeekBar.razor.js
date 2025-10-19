const _instances = {};

function getWidth(id) {
  if (!_instances[id] || !_instances[id].element) return 1;

  return _instances[id].element.getBoundingClientRect().width;
}

function initialize(id, dotnetHelper) {
  _instances[id] = {
    element: document.getElementById(id),
    dotnetHelper: dotnetHelper,
    ro: new ResizeObserver((entries) => {
      for (let entry of entries) {
        if (entry.target.id === id) {
          dotnetHelper.invokeMethodAsync("onResize", entry.contentRect.width);
        }
      }
    })
  };

  _instances[id].ro.observe(_instances[id].element);
}

function getOffsetX(id, clientX) {
  if (!_instances[id] || !_instances[id].element) return 0;

  const rect = _instances[id].element.getBoundingClientRect();
  return clientX - rect.left;
}

function dispose(id) {
  if (_instances[id]) {
    _instances[id].ro.disconnect();
    _instances[id].dotnetHelper.dispose();
    delete _instances[id];
  }
}

function getThumbnail(id, videoElement, canvasElement, thumbnailElement) {
  if (!_instances[id]) {
    return;
  }

  if (!videoElement || !canvasElement) {
    return;
  }

  const ctx = canvasElement.getContext("2d");
  ctx.drawImage(videoElement, 0, 0, canvasElement.width, canvasElement.height);
  const dataUrl = canvasElement.toDataURL("image/png");

  thumbnailElement.src = dataUrl;
}

export const fluentCxSeekBar = {
  getWidth: (id) => getWidth(id),
  initialize: (id, dotnetHelper) => initialize(id, dotnetHelper),
  getOffsetX: (id, clientX) => getOffsetX(id, clientX),
  getThumbnail: (id, videoElement, canvasElement, imageElement) => getThumbnail(id, videoElement, canvasElement, imageElement),
  dispose: (id) => dispose(id)
};
