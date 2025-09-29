export function blurAll(id) {
  const element = document.getElementById(id);

  element.querySelectorAll('.palette-item').forEach(el => {
    if (el) {
      el.blur();
    }
  });
};
