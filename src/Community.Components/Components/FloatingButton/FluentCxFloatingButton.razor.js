export function hasValidTarget(target) {
  if (!target) {
    return false;
  }

  var element = document.getElementById(target);

  return element != null && element !== undefined;
}
