export function saveLayout(id, items) {
  if (localStorage == null) {
    return;
  }

  localStorage.setItem(id, JSON.stringify(items));
}

export function loadLayout(id) {
  if (localStorage == null) {
    return [];
  }

  var content = localStorage.getItem(id);

  if (content == null) {
    return [];
  }

  return JSON.parse(content);
}
