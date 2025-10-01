const NS = "http://www.w3.org/2000/svg";

function getElement(id) {
  return document.getElementById(id);
}

export function exportSvg(gridId, inkId, overlayId) {
  const grid = getElement(gridId);
  const ink = getElement(inkId);
  const overlay = getElement(overlayId);

  const { width, height } = ink.getBoundingClientRect();
  const root = document.createElementNS(NS, "svg");
  root.setAttribute("xmlns", NS);
  root.setAttribute("width", width);
  root.setAttribute("height", height);
  root.setAttribute("viewBox", `0 0 ${width} ${height}`);

  const appendLayer = (layerSvg) => {
    const layerGroup = document.createElementNS(NS, "g");
    for (const node of Array.from(layerSvg.childNodes)) {
      layerGroup.appendChild(node.cloneNode(true));
    }
    root.appendChild(layerGroup);
  };

  appendLayer(grid);
  appendLayer(ink);
  appendLayer(overlay);

  return new XMLSerializer().serializeToString(root);
}



