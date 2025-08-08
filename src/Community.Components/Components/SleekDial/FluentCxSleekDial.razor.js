const _instances = [];

function getInstance(id) {
  for (let i = 0; i < _instances.length; i++) {
    if (_instances[i].id === id) {
      return _instances[i];
    }
  }

  const element = document.getElementById(id);
  const value = {
    id: id,
    element: element,
  };

  _instances.push(value);
  return value;
}

function closeAllButThis(id) {
  for (let i = 0; i < _instances.length; i++) {
    if (_instances[i].id !== id) {
      if (_instances[i].isOpen) {
        _instances[i].dotNetReference.invokeMethodAsync('ShowOrHidePopupAsync', false);
        _instances[i].isOpen = false;
      }
    }
  }
}

export function initialize(id, dotNetReference) {
  const instance = getInstance(id);
  instance.dotNetReference = dotNetReference;
  instance.isOpen = false;

  const button = document.getElementById("Button" + id);

  if (button) {
    button.addEventListener('click', (event) => {
      event.stopPropagation();
      closeAllButThis(id);
      dotNetReference.invokeMethodAsync('OnClickAsync');
      instance.isOpen = !instance.isOpen;
    });
  }

  document.body.addEventListener('click', (event) => {
    dotNetReference.invokeMethodAsync('ShowOrHidePopupAsync', false);
  });
}

export function destroy(id) {
  for (let i = 0; i < _instances.length; i++) {
    if (_instances[i].id === id) {
      document.body.removeEventListener('click', instance.dotNetReference);
      _instances.splice(i, 1);
      break;
    }
  }
}
