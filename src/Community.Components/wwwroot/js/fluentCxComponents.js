
class FluentCxComponentInstances {
  constructor() {
    this.instances = {};
  }

  addInstance(id, instance) {
    this.instances[id] = instance;
  }

  getInstance(id) {
    return this.instances[id];
  }

  removeInstance(id) {
    delete this.instances[id];
  }

  getLength() {
    return Object.keys(this.instances).length;
  }

  getAllInstances() {
    return Object.values(this.instances);
  }

  getAllInstancesBut(id) {
    return Object.values(this.instances).filter(instance => instance.id !== id);
  }
}

export const fluentCxComponentInstances = new FluentCxComponentInstances();
