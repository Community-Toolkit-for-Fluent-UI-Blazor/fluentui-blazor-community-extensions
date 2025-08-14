const _instances = [];

export function initialize(id) {
  var element = document.getElementById(id);

  if (!element) {
    return;
  }

  const messageContainer = element.getElementsByClassName("chat-reply-messagebar")[0];
  const messageTrimSpan = element.getElementsByClassName("chat-reply-messagebar-trim")[0];

  _instances[id] = {
    id: id,
    container: messageContainer,
    trim: messageTrimSpan,
    element: element
  };
}

function measureTextSize(text) {
  const span = document.createElement("span");
  span.style.visibility = 'hidden';
  span.style.position = 'absolute';
  span.style.whiteSpace = 'nowrap';
  span.textContent = text;

  document.body.appendChild(span);
  const width = span.offsetWidth;

  document.body.removeChild(span);

  return width;
}

export function setText(id, text) {
  const instance = _instances[id];

  if (instance) {
    const textWidth = measureTextSize(text);
    const width = instance.container.getBoundingClientRect().width;
    const usedWidth = textWidth > width ? width : textWidth;

    instance.container.width = usedWidth + "px";
    instance.trim.style.width = usedWidth + "px";
    instance.trim.innerHTML = "<i>" + text + "</i>";
    instance.element.classList.add("chat-reply-measured");
  }
}

export function destroy(id) {
  var index = _instances.findIndex(x => x.id == id);
  _instances.splice(index, 1);
}
