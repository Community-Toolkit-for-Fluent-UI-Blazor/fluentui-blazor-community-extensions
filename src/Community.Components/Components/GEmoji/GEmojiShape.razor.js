const instances = [];

export function initialize(elementId, element, elementReference) {
  const e = instances[elementId];

  if (!e) {
    instances[elementId] = {
      reference: elementReference,
      element: element
    };
  }
}

export function emojify(elementId, value) {
  const e = instances[elementId];

  if (!e) {
    return;
  }

  e.reference.invokeMethodAsync('MarkupContent', value)
    .then(markup => { element.innerHTML = markup; });
};
