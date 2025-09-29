const NS = "http://www.w3.org/2000/svg";

const none = 0;
const lines = 1;
const dots = 2;

function num(value, def = 0) {
  const n = Number(value);

  return Number.isFinite(n) ? n : def;
}

function int(value, def = 0)
{
  const n = parseInt(value, 10);

  return Number.isFinite(n) ? n : def;
}


export function updateGrid(grid, opt) {
  grid.innerHTML = "";
  const { width, height } = grid.getBoundingClientRect();

  const displayMode = opt?.displayMode || none;

  if (opt?.backgroundColor && opt.backgroundColor !== "transparent") {
    const bg = document.createElementNS(NS, "rect");
    const margin = Number(opt?.margin || 0);
    bg.setAttribute("x", margin);
    bg.setAttribute("y", margin);
    bg.setAttribute("width", Math.max(0, width - 2 * margin));
    bg.setAttribute("height", Math.max(0, height - 2 * margin));
    bg.setAttribute("fill", opt.backgroundColor);
    grid.appendChild(bg);
  }

  if (displayMode === none) {
    if (opt?.showAxes) {
      drawAxes(grid, width, height);
    }

    return;
  }

  const g = document.createElementNS(NS, "g");
  const color = opt?.color || "#ccc";
  const opacity = num(opt?.opacity, 0.5);
  const strokeWidth = num(opt?.strokeWidth, 1);
  const dashArray = opt?.dashArray || null;
  const cell = Math.max(2, num(opt?.cellSize, 20));
  const margin = Number(opt?.margin || 0);
  const innerW = Math.max(0, width - 2 * margin);
  const innerH = Math.max(0, height - 2 * margin);
  g.setAttribute("transform", `translate(${margin},${margin})`);

  if (displayMode === lines) {
    g.setAttribute("stroke", color);
    g.setAttribute("stroke-opacity", opacity);
    g.setAttribute("fill", "none");

    for (let x = 0, i = 0; x <= innerW + 0.01; x += cell, i++) {
      const line = document.createElementNS(NS, "line");
      line.setAttribute("x1", x); line.setAttribute("y1", 0);
      line.setAttribute("x2", x); line.setAttribute("y2", innerH);

      const boldEvery = int(opt?.boldEvery, 0);
      const sw = (boldEvery && i % boldEvery === 0) ? strokeWidth * 1.8 : strokeWidth;
      line.setAttribute("stroke-width", sw);

      if (dashArray) {
        line.setAttribute("stroke-dasharray", dashArray);
      }

      g.appendChild(line);
    }

    for (let y = 0, j = 0; y <= innerH + 0.01; y += cell, j++) {
      const line = document.createElementNS(NS, "line");
      line.setAttribute("x1", 0); line.setAttribute("y1", y);
      line.setAttribute("x2", innerW); line.setAttribute("y2", y);

      const boldEvery = int(opt?.boldEvery, 0);
      const sw = (boldEvery && j % boldEvery === 0) ? strokeWidth * 1.8 : strokeWidth;
      line.setAttribute("stroke-width", sw);

      if (dashArray) {
        line.setAttribute("stroke-dasharray", dashArray);
      }

      g.appendChild(line);
    }
  } else if (displayMode === dots) {
    const radius = num(opt?.pointRadius, 1.5);

    for (let x = 0; x <= innerW + 0.01; x += cell) {
      for (let y = 0; y <= innerH + 0.01; y += cell) {
        const dot = document.createElementNS(NS, "circle");
        dot.setAttribute("cx", x);
        dot.setAttribute("cy", y);
        dot.setAttribute("r", radius);
        dot.setAttribute("fill", color);
        dot.setAttribute("fill-opacity", opacity);
        g.appendChild(dot);
      }
    }
  }

  grid.appendChild(g);

  if (opt?.showAxes) {
    drawAxes(grid, width, height);
  }
}

function drawAxes(svg, width, height) {
  const ax = document.createElementNS(NS, "line");
  ax.setAttribute("x1", 0); ax.setAttribute("y1", height / 2);
  ax.setAttribute("x2", width); ax.setAttribute("y2", height / 2);
  ax.setAttribute("stroke", "#888"); ax.setAttribute("stroke-width", 2);
  svg.appendChild(ax);

  const ay = document.createElementNS(NS, "line");
  ay.setAttribute("x1", width / 2); ay.setAttribute("y1", 0);
  ay.setAttribute("x2", width / 2); ay.setAttribute("y2", height);
  ay.setAttribute("stroke", "#888"); ay.setAttribute("stroke-width", 2);
  svg.appendChild(ay);
}
