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
    originalSize: { width: 0, height: 0 }
  }

  _instances.push(instance);
}

export function autoSize(id) {
  const instance = getInstance(id);

  if (instance) {
    const rect = instance.parent.getBoundingClientRect();
    const width = Math.round(rect.width);
    const height = Math.round(rect.height);

    instance.originalSize.width = width;
    instance.originalSize.height = height;

    instance.dotnetReference.invokeMethodAsync('getParentSize', width, height);

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
      for (let i = 0; i < imageIdCollection.length; i++) {
        const img = document.getElementById(imageIdCollection[i]);

        if (img.naturalWidth === 0 || img.naturalWidth === 0) {
          img.loading = 'eager';
          img.src = img.src;

          img.onload = () => {
            const size = getSize(rc.width, instance.originalSize.height, img.naturalWidth, img.naturalHeight);
            img.width = size.width;
            img.height = size.height;
          };
        }
        else {
          const size = getSize(rc.width, instance.originalSize.height, img.naturalWidth, img.naturalHeight);
          img.width = size.width;
          img.height = size.height;
        }
      }

      instance.dotnetReference.invokeMethodAsync('ResizeCompleted', instance.originalSize.height);
    }
    else if (imageRatio === fill) {
      let img = document.getElementById(imageIdCollection[0]);

      if (img) {
        // If the image wasn't loaded.
        if (img.naturalHeight === 0 || img.naturalWidth === 0) {
          // Force loading
          img.loading = 'eager';
          img.src = img.src;

          img.onload = () => {
            const size = getFillSize(rc.width, rc.height, img.naturalWidth, img.naturalHeight);
            img.width = size.width;
            img.height = size.height;

            for (let i = 1; i < imageIdCollection.length; ++i) {
              img = document.getElementById(imageIdCollection[i]);

              if (img) {
                img.width = size.width;
                img.height = size.height;
              }
            }

            instance.dotnetReference.invokeMethodAsync('ResizeCompleted', size.height);
          };
        }
        else {
          const size = getFillSize(rc.width, rc.height, img.naturalWidth, img.naturalHeight);
          img.width = size.width;
          img.height = size.height;

          for (let i = 1; i < imageIdCollection.length; ++i) {
            img = document.getElementById(imageIdCollection[i]);

            if (img) {
              img.width = size.width;
              img.height = size.height;
            }
          }

          instance.dotnetReference.invokeMethodAsync('ResizeCompleted', size.height);
        }
      } 
    }
  }
}
