const _instances = [];

function getInstance(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      return _instances[i];
    }
  }

  return null;
}

function getSize(maxWidth, maxHeight, imgWidth, imgHeight) {
  let width = 0;
  let height = 0;

  if (imgWidth > maxWidth || imgHeight > maxHeight) {
    if (maxWidth > maxHeight) {
      let ratio = maxHeight / imgHeight;

      width = imgWidth * ratio;
      height = imgHeight * ratio;
    }
    else {
      let ratio = maxWidth / imgWidth;

      width = imgWidth * ratio;
      height = imgHeight * ratio;
    }
    
  }

  const value = {
    width: width,
    height: height
  }

  return value;
}

export function initialize(id) {
  const element = document.getElementById(id);
  const instance = {
    id: id,
    element: element
  }

  _instances.push(instance);
}

export function setImageSize(id) {
  const instance = getInstance(id);

  if (instance) {
    const rc = instance.element.getBoundingClientRect();
    const imgs = instance.element.getElementsByTagName("img");

    for (let i = 0; i < imgs.length; i++) {
      const img = imgs[i];

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
}

export function destroy(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      _instances.splice(i, 1);
    }
  }
}
