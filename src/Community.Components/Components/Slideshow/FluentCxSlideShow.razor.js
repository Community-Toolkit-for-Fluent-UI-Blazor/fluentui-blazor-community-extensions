const _instances = [];
const auto = 0;
const fill = 1;

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
    parent: element.parentElement,
    lastHeight: element.style.height
  }

  _instances.push(instance);
}

export function autoSize(id) {
  const instance = getInstance(id);

  if (instance) {
    const rect = instance.parent.getBoundingClientRect();
    instance.dotnetReference.invokeMethodAsync('getParentSize', Math.round(rect.width), Math.round(rect.height));
  }
}

export function destroy(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      _instances.splice(i, 1);
    }
  }
}

function getSize(maxWidth, maxHeight, imgWidth, imgHeight) {
  if (imgWidth > maxWidth || imgHeight > maxHeight) {
    const ratioX = maxWidth / imgWidth;
    const ratioY = maxHeight / imgHeight;
    var ratio = Math.min(ratioX, ratioY);
    const width = (imgWidth * ratio);
    const height = (imgHeight * ratio);

    return {
      width: width,
      height: height
    }
  }

  return { width: 0, height: 0 };
}

function getFillSize(maxWidth, maxHeight, imgWidth, imgHeight) {

  const width = maxWidth;
  const ratio = maxWidth / imgWidth;
  const height = Math.ceil(imgHeight * ratio);

  const value = {
    width: width,
    height: height
  }

  return value;
}

export function setImagesSize(containerId, imageRatio, imageIdCollection) {
  if (imageIdCollection.length === 0) {
    return;
  }

  const instance = getInstance(containerId);

  if (instance) {
    const rc = instance.element.getBoundingClientRect();

    if (imageRatio === auto) {
      for (let i = 0; i < imageIdCollection.length; ++i) {
        const img = document.getElementById(imageIdCollection[i]);

        if (img) {
          const size = getSize(rc.width, rc.height, img.naturalWidth, img.naturalHeight);

          if (size.width > 0 &&
            size.height > 0) {
            img.width = size.width;
            img.height = size.height;
          }
        }
      }
    }
    else if (imageRatio === fill) {
      let img = document.getElementById(imageIdCollection[0]);
      let size = 0;

      if (img) {
        size = getFillSize(rc.width, rc.height, img.naturalWidth, img.naturalHeight);
        img.width = size.width;
        img.height = size.height;

        instance.element.height = size.height;
      }

      for (let i = 1; i < imageIdCollection.length; ++i) {
        img = document.getElementById(imageIdCollection[i]);

        if (img) {
          img.width = size.width;
          img.height = size.height;
        }
      }
    }
  }
}
