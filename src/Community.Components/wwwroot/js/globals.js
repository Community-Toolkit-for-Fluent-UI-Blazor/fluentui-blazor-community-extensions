import { fluentCxComponentInstances } from './fluentCxComponents.js';

window.fluentCxComponentInstances = {
  getInstance: (id) => fluentCxComponentInstances.getInstance(id),
  addInstance: (id, instance) => fluentCxComponentInstances.addInstance(id, instance),
  removeInstance: (id) => fluentCxComponentInstances.removeInstance(id),
  getLength: () => fluentCxComponentInstances.getLength(),
  getAllInstances: () => fluentCxComponentInstances.getAllInstances(),
  getAllInstanceBut: (id) => fluentCxComponentInstances.getAllInstanceBut(id)
};
