const _instances = [];
const auto = 0;
const fill = 1;
const horizontal = 0;
const moveNext = 0;
const movePrevious = 1;

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
  const itemsContainer = document.getElementById(`slide-show-items-container-${id}`);

  if (element && itemsContainer) {
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
      fixedHeight: height !== undefined && height !== null,
      startX: 0,
      startY: 0,
      itemsContainer: itemsContainer,
      items: []
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

function onTouchStart(instance, e) {
  instance.startX = e.touches[0].clientX;
  instance.startY = e.touches[0].clientY;


  console.log(`Touch start: (${instance.startX}, ${instance.startY})`);
}

function onTouchEnd(instance, e, touchThreshold) {
  const endY = e.changedTouches[0].clientY;
  const endX = e.changedTouches[0].clientX;
  const deltaX = endX - instance.startX;
  const deltaY = endY - instance.startY;

  if (Math.abs(deltaX) > Math.abs(deltaY)) {
    if (Math.abs(deltaX) > touchThreshold) {
      if (deltaX > 0) {
        instance.dotnetReference.invokeMethodAsync('onTouchSwipe', moveNext);
      }
      else {
        instance.dotnetReference.invokeMethodAsync('onTouchSwipe', movePrevious);
      }
    }
  }
  else {
    if (Math.abs(deltaY) > touchThreshold) {
      if (deltaY > 0) {
        instance.dotnetReference.invokeMethodAsync('onTouchSwipe', moveNext);
      }
      else {
        instance.dotnetReference.invokeMethodAsync('onTouchSwipe', movePrevious);
      }
    }
  }

  console.log(`Touch end: (${endX}, ${endY}) - Delta: (${deltaX}, ${deltaY})`);
}

function onTouchMove(e) {
  e.preventDefault();

  console.log(`Touch move: (${e.touches[0].clientX}, ${e.touches[0].clientY})`);
}

export function disableOrEnableTouch(containerId, isTouchEnabled, touchThreshold) {
  const instance = getInstance(containerId);

  if (instance) {
    if (isTouchEnabled) {
      instance.element.addEventListener('touchstart', e => {
        onTouchStart(instance, e);
      });

      instance.element.addEventListener('touchend', e => {
        onTouchEnd(instance, e, touchThreshold);
      });

      instance.element.addEventListener('touchmove', e => {
        onTouchMove(e);
      }, { passive: false });
    }
    else {
      instance.element.removeEventListener('touchstart', e => {
        onTouchStart(instance, e);
      });

      instance.element.removeEventListener('touchend', e => {
        onTouchEnd(instance, e, touchThreshold);
      });

      instance.element.removeEventListener('touchmove', e => {
        onTouchMove(e);
      });
    }
  }
}

export function destroy(id) {
  for (let i = _instances.length - 1; i >= 0; i--) {
    if (_instances[i].id == id) {
      _instances[i].resizeObserver.unobserve(_instances[i].parent);
      _instances[i].resizeObserver.disconnect();

      instance.element.removeEventListener('touchstart', e => {
        onTouchStart(instance, e);
      });

      instance.element.removeEventListener('touchend', e => {
        onTouchEnd(instance, e, touchThreshold);
      });

      instance.element.removeEventListener('touchmove', e => {
        onTouchMove(e);
      });

      instance.itemsContainer.removeEventListener('transitionend', () => {
        onTransitionEnd(instance);
      });

      _instances.splice(i, 1);
    }
  }
}

// These two functions are used for infinite loop mode only.
// We need to do this to reduce the complexity of the Blazor code and avoid manipulating the DOM from Blazor.
export function infiniteLoopMoveNext(id, orientation) {
  const instance = getInstance(id);

  if (instance) {
    const firstElement = instance.itemsContainer.firstElementChild;
    instance.itemsContainer.style.transition = 'transform var(--slideshow-duration) ease-in-out';
    instance.itemsContainer.style.transform = orientation === horizontal ? 'translateX(calc(-100% / var(--slideshow-item-count, 1)))' : 'translateY(calc(-100% / var(--slideshow-item-count, 1)))';

    instance.itemsContainer.addEventListener('transitionend', () => {
      instance.itemsContainer.style.transition = 'none';
      instance.itemsContainer.append(firstElement);
      instance.itemsContainer.style.transform = orientation === horizontal ? 'translateX(0%)' : 'translateY(0%)';
    }, { once: true });
  }
}

export function infiniteLoopMovePrevious(id, orientation) {
  const instance = getInstance(id);

  if (instance) {
    const lastElement = instance.itemsContainer.lastElementChild;
    instance.itemsContainer.style.transition = 'none';
    instance.itemsContainer.prepend(lastElement);
    instance.itemsContainer.style.transform = orientation === horizontal ? 'translateX(calc(-100% / var(--slideshow-item-count, 1)))' : 'translateY(calc(-100% / var(--slideshow-item-count, 1)))';

    requestAnimationFrame(() => {
      instance.itemsContainer.style.transition = 'transform var(--slideshow-duration) ease-in-out';
      instance.itemsContainer.style.transform = orientation === horizontal ? 'translateX(0%)' : 'translateY(0%)';
    });
  }
}

export function storeItems(id, idCollection) {
  const instance = getInstance(id);

  if (instance) {
    for(let i = 0; i < idCollection.length; i++) {
      const item = document.getElementById(idCollection[i]);

      if (item) {
        instance.items.push(item);
      }
    }
  }
}

export function restoreItems(id) {
  const instance = getInstance(id);

  if (instance) {
    for(let i = instance.itemsContainer.children.length - 1; i >= 0; i--) {
      instance.itemsContainer.removeChild(instance.itemsContainer.children[i]);
    }

    for (let i = 0; i < instance.items.length; i++) {
      instance.itemsContainer.appendChild(instance.items[i]);
    }
  }
}

export function clearTransition(id) {
  const instance = getInstance(id);

  if (instance) {
    instance.itemsContainer.style.transition = '';
    instance.itemsContainer.style.transform = '';
  }
}
