const _instances = {};
const NS = "http://www.w3.org/2000/svg";
let eraseMode = false;

let eraserOpts = {
  size: 12,
  shape: 0,
  opacity: 1,
  softEdges: false,
  cursor: "crosshair",
  partialErase: false
};


function num(value, def = 0) {
  const n = Number(value);

  return Number.isFinite(n) ? n : def;
}

export function init(ink, options) {
  _instances[ink.id] = {
    element: ink,
    options: penFromOptions(options?.pen || {}),
    isDrawing: false,
    currentPath: null,
    points: [],
    strokes: [],
    undone: [],
    penFilterId: null
  };

  setInkOptions(ink, options?.pen || {});
  attachPointerHandlers(ink);
}

export function setInkOptions(ink, penOptions) {
  const s = _instances[ink.id] || { strokes: [], undone: [] };
  s.opts = penFromOptions(penOptions || {});
  _instances[ink.id] = s;
  ensurePenShadowFilter(ink);
}

export function setEraseMode(ink, enabled, opts) {
  eraseMode = !!enabled;

  if (opts) {
    eraserOpts = { ...eraserOpts, ...normalizeEraser(opts) };
  }

  const container = ink.parentElement;

  if (container) {
    container.style.cursor = eraseMode ? (eraserOpts.cursor || "crosshair") : "default";
  }
}

export function clear(ink) {
  ink.innerHTML = "";

  const s = _instances[ink.id];

  if (s) {
    s.strokes = [];
    s.undone = [];
    s.points = [];
    s.currentPath = null;
    s.isDrawing = false;
  }
}

export function undo(ink) {
  const s = _instances[ink.id];

  if (!s || s.strokes.length === 0) {
    return;
  }

  const path = s.strokes.pop();
  s.undone.push(path);
  path.remove();
}

export function redo(ink) {
  const s = _instances[ink.id];

  if (!s || s.undone.length === 0) {
    return;
  }

  const path = s.undone.pop();
  ink.appendChild(path);
  s.strokes.push(path);
}

function attachPointerHandlers(ink) {
  const s = _instances[ink.id];

  ink.addEventListener("pointerdown", (e) => {
    ink.setPointerCapture?.(e.pointerId);

    if (eraseMode) {
      eraseAt(e.offsetX, e.offsetY, ink);
      return;
    }

    s.isDrawing = true;
    s.points = [];
    s.undone = []; // reset redo stack on new stroke

    const p = pressure(e, s.opts);
    const path = createPath(ink.id, s.opts, p);
    path.setAttribute("d", `M ${e.offsetX} ${e.offsetY}`);
    ink.appendChild(path);

    s.currentPath = path;
    s.strokes.push(path);
    s.points.push(pt(e, p));
  });

  ink.addEventListener("pointermove", (e) => {
    if (eraseMode) {
      if (e.buttons & 1) {
        eraseAt(e.offsetX, e.offsetY, ink);
      }

      return;
    }

    if (!s.isDrawing || !s.currentPath) {
      return;
    }

    const p = pressure(e, s.opts);
    s.points.push(pt(e, p));

    if (s.opts.smoothing && s.points.length >= 3) {
      const d = buildSmoothPath(s.points);
      s.currentPath.setAttribute("d", d);
    } else {
      const d = s.currentPath.getAttribute("d") + ` L ${e.offsetX} ${e.offsetY}`;
      s.currentPath.setAttribute("d", d);
    }

    s.currentPath.setAttribute("stroke-width", widthFromPressure(p, s.opts));
  });

  const end = (e) => {
    if (!s.isDrawing) {
      return;
    }

    s.isDrawing = false;
    ink.releasePointerCapture?.(e.pointerId);
    s.currentPath = null;
    s.points = [];
  };

  ink.addEventListener("pointerup", end);
  ink.addEventListener("pointercancel", end);
  ink.addEventListener("pointerleave", end);
}

function eraseAt(x, y, ink) {
  const s = _instances[ink.id];
  const size = Number(eraserOpts.size || 12);

  const left = x - size;
  const right = x + size;
  const top = y - size;
  const bottom = y + size;

  const survivors = [];

  for (const path of s.strokes) {
    const bb = path.getBBox();
    const hit = !(bb.x > right || bb.x + bb.width < left || bb.y > bottom || bb.y + bb.height < top);
    if (hit) {
      path.remove();
      s.undone = [];
    } else {
      survivors.push(path);
    }
  }
  s.strokes = survivors;
}

function createPath(ink, opts, p) {
  const path = document.createElementNS(NS, "path");
  path.setAttribute("fill", "none");
  path.setAttribute("stroke", opts.color);
  path.setAttribute("stroke-width", widthFromPressure(p, opts));
  path.setAttribute("stroke-linecap", opts.lineCap);
  path.setAttribute("stroke-linejoin", opts.lineJoin);
  path.setAttribute("opacity", num(opts.opacity, 1));
  if (opts.dashArray) path.setAttribute("stroke-dasharray", opts.dashArray);

  const s = _instances[ink.id];
  if (opts.shadow && s?.penFilterId) {
    path.setAttribute("filter", `url(#${s.penFilterId})`);
  }

  return path;
}

function penFromOptions(p) {
  const lineCap = mapLineCap(p?.lineCap);
  const lineJoin = mapLineJoin(p?.lineJoin);

  return {
    color: p?.color || "#111",
    baseWidth: num(p?.baseWidth, 2),
    dashArray: p?.dashArray || null,
    pressure: (p?.pressureEnabled !== false),
    smoothing: (p?.smoothing !== false),
    opacity: num(p?.opacity, 1),
    lineCap,
    lineJoin,
    shadow: !!(p?.shadow?.enabled),
    shadowColor: p?.shadow?.color || "rgba(0,0,0,0.3)",
    shadowBlur: num(p?.shadow?.blur, 2),
    shadowOffsetX: num(p?.shadow?.offsetX, 1),
    shadowOffsetY: num(p?.shadow?.offsetY, 1)
  };
}

function ensurePenShadowFilter(ink) {
  const s = _instances[ink.id];
  if (!s) return;

  let defs = ink.querySelector("defs");

  if (!defs) {
    defs = document.createElementNS(NS, "defs");
    ink.insertBefore(defs, ink.firstChild || null);
  } else {
    if (s.penFilterId) {
      const old = defs.querySelector(`#${s.penFilterId}`);
      old?.remove();
    }
  }

  if (!s.opts.shadow) {
    s.penFilterId = null;
    return;
  }

  const id = `penShadow-${Math.random().toString(36).slice(2)}`;
  s.penFilterId = id;

  const filter = document.createElementNS(NS, "filter");
  filter.setAttribute("id", id);
  filter.setAttribute("filterUnits", "userSpaceOnUse");

  const feFlood = document.createElementNS(NS, "feFlood");
  feFlood.setAttribute("flood-color", s.opts.shadowColor);
  feFlood.setAttribute("flood-opacity", "1");
  feFlood.setAttribute("result", "flood");

  const feComposite = document.createElementNS(NS, "feComposite");
  feComposite.setAttribute("in", "flood");
  feComposite.setAttribute("in2", "SourceAlpha");
  feComposite.setAttribute("operator", "in");
  feComposite.setAttribute("result", "shadowColor");

  const feOffset = document.createElementNS(NS, "feOffset");
  feOffset.setAttribute("dx", s.opts.shadowOffsetX);
  feOffset.setAttribute("dy", s.opts.shadowOffsetY);
  feOffset.setAttribute("result", "offsetShadow");

  const feGaussian = document.createElementNS(NS, "feGaussianBlur");
  feGaussian.setAttribute("stdDeviation", s.opts.shadowBlur);
  feGaussian.setAttribute("in", "offsetShadow");
  feGaussian.setAttribute("result", "blurredShadow");

  const feMerge = document.createElementNS(NS, "feMerge");
  const m1 = document.createElementNS(NS, "feMergeNode");
  m1.setAttribute("in", "blurredShadow");
  const m2 = document.createElementNS(NS, "feMergeNode");
  m2.setAttribute("in", "SourceGraphic");
  feMerge.appendChild(m1);
  feMerge.appendChild(m2);

  filter.appendChild(feFlood);
  filter.appendChild(feComposite);
  filter.appendChild(feOffset);
  filter.appendChild(feGaussian);
  filter.appendChild(feMerge);

  defs.appendChild(filter);
}

function buildSmoothPath(points) {
  if (points.length < 2) {
    const a = points[0];
    return `M ${a.x} ${a.y}`;
  }

  let d = `M ${points[0].x} ${points[0].y}`;

  for (let i = 0; i < points.length - 1; i++) {
    const p0 = points[i - 1] || points[i];
    const p1 = points[i];
    const p2 = points[i + 1];
    const p3 = points[i + 2] || p2;
    const c1x = p1.x + (p2.x - p0.x) / 6;
    const c1y = p1.y + (p2.y - p0.y) / 6;
    const c2x = p2.x - (p3.x - p1.x) / 6;
    const c2y = p2.y - (p3.y - p1.y) / 6;
    d += ` C ${c1x} ${c1y}, ${c2x} ${c2y}, ${p2.x} ${p2.y}`;
  }

  return d;
}

function clamp(v, a, b) {
  return Math.max(a, Math.min(b, v));
}

function pressure(e, opts) {
  let raw = (opts.pressure && typeof e.pressure === "number") ? e.pressure : 0.5;

  if (!raw || raw === 0) {
    raw = 0.5;
  }

  return clamp(raw, 0.1, 1.0);
}

function widthFromPressure(p, opts) {
  const base = Number(opts.baseWidth || 2);

  return base * (0.4 + 0.6 * p);
}

function pt(e, p) {
  return {
    x: e.offsetX,
    y: e.offsetY,
    p
  };
}

function mapLineCap(val) {
  return ["butt", "round", "square"][val] || "round";
}

function mapLineJoin(val) {
  return ["miter", "round", "bevel"][val] || "round";
}

function normalizeEraser(opts) {
  return {
    size: num(opts.size, eraserOpts.size),
    shape: typeof opts.shape === "number" ? opts.shape : eraserOpts.shape,
    opacity: num(opts.opacity, eraserOpts.opacity),
    softEdges: !!opts.softEdges,
    cursor: opts.cursor || eraserOpts.cursor,
    partialErase: !!opts.partialErase
  };
}
