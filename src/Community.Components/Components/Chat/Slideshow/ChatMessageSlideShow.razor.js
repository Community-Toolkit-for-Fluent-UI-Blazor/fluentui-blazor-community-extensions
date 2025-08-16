export function updateSize(id, dotNetReference) {
  const instance = document.getElementById(id);
  const dialogBody = document.getElementsByClassName('fluent-dialog-body')[0];

  if (instance && dialogBody) {
    const rect = dialogBody.getBoundingClientRect();
    dotNetReference.invokeMethodAsync('getParentSize', rect.width, rect.height);
  }
}
