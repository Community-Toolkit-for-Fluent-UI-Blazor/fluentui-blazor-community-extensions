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

function setAutoSizeImages(instance, imageIdCollection) {
  for (let i = 0; i < imageIdCollection.length; i++) {
    const img = document.getElementById(imageIdCollection[i]);

    if (img.naturalWidth === 0 || img.naturalHeight === 0) {
      img.onload = () => {
        const size = getSize(instance.width, instance.height, img.naturalWidth, img.naturalHeight);
        img.width = size.width;
        img.height = size.height;
      };
    }
    else {
      const size = getSize(instance.width, instance.height, img.naturalWidth, img.naturalHeight);
      img.width = size.width;
      img.height = size.height;
    }
  }

  instance.dotnetReference.invokeMethodAsync('setAutoSizeCompleted');
}

function setFillSizeImage(instance, img, imageIdCollection) {
  const size = getFillSize(instance.width, instance.height, img.naturalWidth, img.naturalHeight);
  const w = Math.min(size.width, instance.width);
  const h = Math.min(size.height, instance.height);

  img.width = w;
  img.height = h;

  for (let i = 1; i < imageIdCollection.length; ++i) {
    const childImg = document.getElementById(imageIdCollection[i]);

    if (childImg) {
      childImg.width = w;
      childImg.height = h;
    }
  }

  instance.dotnetReference.invokeMethodAsync('setFillSizeCompleted', w, h);
}

function setFillSizeImages(instance, imageIdCollection) {
  const img = document.getElementById(imageIdCollection[0]);

  if (img) {
    // If the image wasn't loaded.
    if (img.naturalHeight === 0 || img.naturalWidth === 0) {
      // Force loading
      img.loading = 'eager';
      img.src = img.src;

      img.onload = () => {
        setFillSizeImage(instance, img, imageIdCollection);
      };
    }
    else {
      setFillSizeImage(instance, img, imageIdCollection);
    }
  }
}

function setSizeFromParent(instance, width, height) {
  if (instance) {
    const rect = instance.parent.getBoundingClientRect();
    const w = width === undefined || width === null ? Math.round(rect.width) : width;
    const h = height === undefined || height === null ? Math.round(rect.height) : height;

    instance.width = w;
    instance.height = h;
    instance.dotnetReference.invokeMethodAsync('setSizeFromParent', w, h);
  }
}

export function initialize(id, dotnetReference, width, height) {
  const element = document.getElementById(id);
  const instance = {
    id: id,
    element: element,
    dotnetReference: dotnetReference,
    parent: element.parentElement,
    resizeObserver: null,
    imageRatio: auto,
    imageIdCollection: [],
    width: width,
    height: height,
    fixedWidth: width !== undefined && width !== null,
    fixedHeight: height !== undefined && height !== null
  }

  _instances.push(instance);

  const resizeObserver = new ResizeObserver((entries) => {
    for (let entry of entries) {
      instance.dotnetReference.invokeMethodAsync('resizeObserverEvent',
        {
          width: Math.ceil(entry.contentRect.width),
          height: Math.ceil(entry.contentRect.height),
          fixedWidth: instance.fixedWidth,
          fixedHeight: instance.fixedHeight
        })
        .then(data => {
          instance.width = data[0];
          instance.height = data[1];
          setImagesSize(instance.id, instance.imageRatio, instance.imageIdCollection);
        });
    }
  });

  resizeObserver.observe(instance.parent);
  instance.resizeObserver = resizeObserver;

  if (width === undefined || width === null || height === undefined || height === null) {
    setSizeFromParent(instance, width, height);
  }
}

export function setImagesSize(containerId, imageRatio, imageIdCollection) {
  if (imageIdCollection.length === 0) {
    return;
  }

  const instance = getInstance(containerId);

  if (instance) {
    instance.imageIdCollection = imageIdCollection;
    instance.imageRatio = imageRatio;

    if (imageRatio === auto) {
      setAutoSizeImages(instance, imageIdCollection);
    }
    else if (imageRatio === fill) {
      setFillSizeImages(instance, imageIdCollection);
    }
  }
}

export function destroy(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      _instances[i].resizeObserver.unobserve(_instances[i].parent);
      _instances[i].resizeObserver.disconnect();
      _instances.splice(i, 1);
    }
  }
}
