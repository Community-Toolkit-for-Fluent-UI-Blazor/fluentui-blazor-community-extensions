export function store(items) {
  localStorage.setItem("scheduler-items", JSON.stringify(items));
}

export function retrieve() {
  const itemsJson = localStorage.getItem("scheduler-items");

  if (itemsJson) {
    return JSON.parse(itemsJson);
  }
}
