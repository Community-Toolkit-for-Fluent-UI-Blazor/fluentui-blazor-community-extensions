const _instances = {};

function getInstance(id) {
  return _instances[id];
}

function initialize(id, reference) {
  _instances[id] = {
    reference: reference,
    images: []
  };
}

function updateScatterImages(id, images) {
  const instance = getInstance(id);

  if (instance) {
    instance.images = images.map(m => document.getElementById(m)).filter(el => el);
  }
}

function disposeInstance(id) {
  delete _instances[id];
}

export const fluentCxArtisticScatter = {
  initialize: (id, reference) => initialize(id, reference),
  updateScatterImages: (id, images) => updateScatterImages(id, images),
  dispose: (id) => disposeInstance(id)
};
