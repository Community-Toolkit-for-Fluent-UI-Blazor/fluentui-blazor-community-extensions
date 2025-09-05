const state = new WeakMap();
const pen = 0;
const eraser = 1;

const solid = 0;
const dashed = 1;
const dotted = 2;

const lines = 1;
const dots = 2;

function getCtx(canvas) {
  const ctx = canvas.getContext('2d');
  ctx.lineCap = 'round';
  ctx.lineJoin = 'round';
  ctx.imageSmoothingEnabled = true;

  return ctx;
}

function hexToRgba(hex, opacity) {
  let color = hex.replace('#', '');

  if (color.length === 3) {
    color = color.split('').map(x => x + x).join('');
  }

  const r = parseInt(color.substring(0, 2), 16) || 0;
  const g = parseInt(color.substring(2, 4), 16) || 0;
  const b = parseInt(color.substring(4, 6), 16) || 0;
  const a = Math.min(Math.max(opacity ?? 1, 0), 1);

  return `rgba(${r},${g},${b},${a})`;
}

function clone(obj) {
  return JSON.parse(JSON.stringify(obj));
}

function setstrokeStyle(ctx, style, width) {
  ctx.setLineDash([]);

  if (style === dashed) {
    ctx.setLineDash([width * 2, width * 2]);
  }
  else if (style === dotted) {
    ctx.setLineDash([width, width * 2]);
  }
}

function pointerPos(canvas, e) {
  const rect = canvas.getBoundingClientRect();
  const x = (e.clientX - rect.left) * (canvas.width / rect.width);
  const y = (e.clientY - rect.top) * (canvas.height / rect.height);

  return { x, y };
}

function loadImage(b64, onload) {
  if (!b64) return null;
  const img = new Image();
  img.onload = () => onload && onload();
  img.src = `data:image;base64,${b64}`;
  return img;
}

function ensureImages(s, backgroundImage, watermarkImage, rerender) {
  if (backgroundImage !== undefined) {
    s.backgroundImage = backgroundImage;
    s.bgImg = loadImage(backgroundImage, () => rerender && rerender());
  }

  if (watermarkImage !== undefined) {
    s.watermarkImage = watermarkImage;
    s.wmImg = loadImage(watermarkImage, () => rerender && rerender());
  }
}

function drawBackground(canvas, ctx, s) {
  const opts = s.opts;

  ctx.save();
  ctx.setTransform(1, 0, 0, 1, 0, 0);
  ctx.clearRect(0, 0, canvas.width, canvas.height);
  ctx.fillStyle = opts.background || '#ffffff';
  ctx.fillRect(0, 0, canvas.width, canvas.height);
  ctx.restore();


  if (s.bgImg) {
    ctx.drawImage(s.bgImg, 0, 0, canvas.width, canvas.height);
  }


  if (s.wmImg && opts.watermarkOpacity > 0) {
    ctx.save();
    ctx.globalAlpha = Math.min(Math.max(opts.watermarkOpacity, 0), 1);
    ctx.drawImage(s.wmImg, 0, 0, canvas.width, canvas.height);
    ctx.restore();
  }


  if (opts.watermarkText && opts.watermarkText.length > 0 && opts.watermarkOpacity > 0) {
    ctx.save();
    ctx.globalAlpha = Math.min(Math.max(opts.watermarkOpacity, 0), 1);
    ctx.translate(canvas.width / 2, canvas.height / 2);
    ctx.rotate((-30 * Math.PI) / 180);
    ctx.fillStyle = 'rgba(0,0,0,1)';
    ctx.font = '700 32px Arial';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    ctx.fillText(opts.watermarkText, 0, 0);
    ctx.restore();
  }
}

function drawGrid(ctx, s) {
  const opts = s.opts;
  if (!opts.showGrid || !opts.gridSpacing || opts.gridSpacing < 4) return;

  const spacing = opts.gridSpacing;
  const color = opts.gridColor || '#e5e7eb';
  const opacity = Math.min(Math.max(opts.gridOpacity ?? 0.8, 0), 1);
  const rgba = hexToRgba(color, opacity);

  ctx.save();
  ctx.strokeStyle = rgba;
  ctx.fillStyle = rgba;
  ctx.lineWidth = 1;

  if (opts.gridType === dots) {
    for (let x = 0; x < opts.width; x += spacing) {
      for (let y = 0; y < opts.height; y += spacing) {
        ctx.beginPath();
        ctx.arc(x, y, 1.5, 0, 2 * Math.PI);
        ctx.fill();
      }
    }
  } else if(opts.gridType === lines) {
    // Lines
    for (let x = 0; x < opts.width; x += spacing) {
      ctx.beginPath();
      ctx.moveTo(x, 0);
      ctx.lineTo(x, opts.height);
      ctx.stroke();
    }
    for (let y = 0; y < opts.height; y += spacing) {
      ctx.beginPath();
      ctx.moveTo(0, y);
      ctx.lineTo(opts.width, y);
      ctx.stroke();
    }
  }

  ctx.restore();
}

function drawPath(ctx, stroke) {
  const points = stroke.points || [];
  if (points.length < 2) return;

  ctx.beginPath();
  ctx.moveTo(points[0].x, points[0].y);

  if (stroke.smooth && points.length >= 3) {
    for (let i = 1; i < points.length - 1; i++) {
      const p1 = points[i];
      const p2 = points[i + 1];
      const mx = (p1.x + p2.x) / 2;
      const my = (p1.y + p2.y) / 2;
      ctx.quadraticCurveTo(p1.x, p1.y, mx, my);
    }

    const last = points[points.length - 1];
    ctx.lineTo(last.x, last.y);

  } else {
    for (let i = 1; i < points.length; i++) {
      ctx.lineTo(points[i].x, points[i].y);
    }
  }
  ctx.stroke();
}

function renderAll(canvas, strokes, s, inProgressStroke) {
  const ctx = getCtx(canvas);
  drawBackground(canvas, ctx, s);
  drawGrid(ctx, s);

  for (const st of strokes || []) {
    ctx.save();
    if (st.eraser) {
      ctx.globalCompositeOperation = 'destination-out';
      ctx.strokeStyle = 'rgba(0,0,0,1)';
      ctx.shadowBlur = 0;
      ctx.shadowColor = 'transparent';
    } else {
      ctx.globalCompositeOperation = 'source-over';
      ctx.strokeStyle = hexToRgba(st.colorHex || s.opts.penColor, st.opacity ?? s.opts.penOpacity);
      if (s.opts.useShadow) {
        ctx.shadowColor = hexToRgba(s.opts.shadowColor, s.opts.shadowOpacity);
        ctx.shadowBlur = 1.5;
      } else {
        ctx.shadowBlur = 0;
        ctx.shadowColor = 'transparent';
      }
    }

    const width = st.width || s.opts.strokeWidth || 3;
    ctx.lineWidth = width;
    setstrokeStyle(ctx, st.style || s.opts.strokeStyle, width);
    drawPath(ctx, st);
    ctx.restore();
  }

  if (inProgressStroke && inProgressStroke.points && inProgressStroke.points.length > 1) {
    const st = inProgressStroke;
    ctx.save();
    if (st.eraser) {
      ctx.globalCompositeOperation = 'destination-out';
      ctx.strokeStyle = 'rgba(0,0,0,1)';
      ctx.shadowBlur = 0;
      ctx.shadowColor = 'transparent';
    } else {
      ctx.globalCompositeOperation = 'source-over';
      ctx.strokeStyle = hexToRgba(st.colorHex || s.opts.penColor, st.opacity ?? s.opts.penOpacity);
      if (s.opts.useShadow) {
        ctx.shadowColor = 'rgba(0,0,0,0.24)';
        ctx.shadowBlur = 1.5;
      } else {
        ctx.shadowBlur = 0;
        ctx.shadowColor = 'transparent';
      }
    }
    const width = st.width || s.opts.strokeWidth || 3;
    ctx.lineWidth = width;
    setstrokeStyle(ctx, st.style || s.opts.strokeStyle, width);
    drawPath(ctx, st);
    ctx.restore();
  }
}

function updateOptions(canvas, opts) {
  let s = state.get(canvas);
  if (!s) {
    s = {
      opts: {},
      strokes: [],
      drawing: false,
      points: [],
      pressures: [],
      tool: 0,
      dotnet: null,
      bgImg: null,
      wmImg: null,
      backgroundImage: null,
      watermarkImage: null
    };
    state.set(canvas, s);
  }
  s.opts = {
    ...s.opts,
    strokeWidth: opts.strokeWidth ?? 3,
    penColor: opts.penColor || '#000000',
    penOpacity: opts.penOpacity ?? 1,
    strokeStyle: opts.strokeStyle || 0,
    smooth: !!opts.smooth,
    useShadow: !!opts.useShadow,
    usePointerPressure: !!opts.usePointerPressure,
    width: opts.width,
    height: opts.height,
    background: opts.background || '#ffffff',
    showSeparator: !!opts.showSeparator,
    separatorY: opts.separatorY ?? 0.6,
    showGrid: !!opts.showGrid,
    gridType: opts.gridType || lines,
    gridSpacing: opts.gridSpacing ?? 24,
    gridColor: opts.gridColor || '#e5e7eb',
    gridOpacity: opts.gridOpacity ?? 0.8,
    watermarkText: opts.watermarkText || '',
    watermarkOpacity: opts.watermarkOpacity ?? 0.15,
    shadowColor: opts.shadowColor || '#000000',
    shadowOpacity: opts.shadowOpacity ?? 0.24
  };
  return s;
}

function attachInput(canvas, dotnetRef) {
  const s = state.get(canvas);
  if (!s) return;

  const onDown = (e) => {
    if (!e.isPrimary) return;
    e.preventDefault();
    canvas.setPointerCapture(e.pointerId);
    s.drawing = true;
    s.points = [];
    s.pressures = [];
    const p = pointerPos(canvas, e);
    s.points.push(p);
    const pres = s.opts.usePointerPressure ? (e.pressure || 1) : 1;
    s.pressures.push(pres);

    const strokePreview = buildInProgressStroke(s);
    renderAll(canvas, s.strokes, s, strokePreview);
  };

  const onMove = (e) => {
    if (!s.drawing || !e.isPrimary) {
      return;
    }

    e.preventDefault();
    const p = pointerPos(canvas, e);
    s.points.push(p);
    const pres = s.opts.usePointerPressure ? (e.pressure || 1) : 1;
    s.pressures.push(pres);

    const strokePreview = buildInProgressStroke(s);
    renderAll(canvas, s.strokes, s, strokePreview);
  };

  const onUp = (e) => {
    if (!s.drawing || !e.isPrimary) return;
    e.preventDefault();
    s.drawing = false;
    try {
      canvas.releasePointerCapture(e.pointerId);
    } catch { }

    const stroke = buildCompletedStroke(s);

    renderAll(canvas, s.strokes, s, null);

    if (s.dotnet && stroke.points && stroke.points.length > 1) {
      s.dotnet.invokeMethodAsync('OnStrokeCompleted', stroke);
    }
  };

  const onCancel = (e) => {
    if (!s.drawing) return;
    s.drawing = false;
    s.points = [];
    s.pressures = [];
    renderAll(canvas, s.strokes, s, null);
  };

  detachInput(canvas);

  canvas.addEventListener('pointerdown', onDown, { passive: false });
  canvas.addEventListener('pointermove', onMove, { passive: false });
  canvas.addEventListener('pointerup', onUp, { passive: false });
  canvas.addEventListener('pointercancel', onCancel, { passive: false });
  canvas.addEventListener('pointerleave', onCancel, { passive: false });

  s.detach = () => {
    canvas.removeEventListener('pointerdown', onDown);
    canvas.removeEventListener('pointermove', onMove);
    canvas.removeEventListener('pointerup', onUp);
    canvas.removeEventListener('pointercancel', onCancel);
    canvas.removeEventListener('pointerleave', onCancel);
  };
}

function detachInput(canvas) {
  const s = state.get(canvas);

  if (s && s.detach) {
    s.detach();
    delete s.detach;
  }
}

function buildInProgressStroke(s) {
  const baseWidth = s.opts.strokeWidth || 3;
  const style = s.opts.strokeStyle || solid;
  const opacity = s.opts.penOpacity ?? 1;
  const color = s.opts.penColor || '#000000';

  let width = baseWidth;

  if (s.opts.usePointerPressure && s.pressures.length > 0) {
    const pres = s.pressures[s.pressures.length - 1] || 1;
    width = Math.max(0.5, baseWidth * pres);
  }

  return {
    width,
    color,
    opacity,
    smooth: !!s.opts.smooth,
    eraser: s.tool === eraser,
    style,
    points: s.points.map(p => ({ x: p.x, y: p.y }))
  };
}

function buildCompletedStroke(s) {
  const baseWidth = s.opts.strokeWidth || 3;
  let width = baseWidth;

  if (s.opts.usePointerPressure && s.pressures.length > 0) {
    const avg = s.pressures.reduce((a, b) => a + b, 0) / s.pressures.length;
    width = Math.max(0.5, baseWidth * (avg || 1));
  }

  return {
    width,
    color: s.opts.penColor || '#000000',
    opacity: s.opts.penOpacity ?? 1,
    smooth: !!s.opts.smooth,
    eraser: s.tool === eraser,
    strokeStyle: s.opts.strokeStyle || solid,
    points: s.points.map(p => ({ x: p.x, y: p.y }))
  };
}

function drawPreviewLine(canvas, options) {
  const ctx = canvas.getContext("2d");
  ctx.clearRect(0, 0, canvas.width, canvas.height);

  ctx.globalAlpha = options.opacity;
  ctx.lineWidth = options.thickness;
  ctx.strokeStyle = options.color;
  ctx.lineJoin = "round";
  ctx.lineCap = options.smooth ? "round" : "butt";

  if (options.shadow) {
    ctx.shadowColor = options.shadowColor;
    ctx.shadowBlur = 4;
    ctx.shadowOffsetX = 2;
    ctx.shadowOffsetY = 2;
    ctx.globalAlpha = options.shadowOpacity;
  } else {
    ctx.shadowColor = "transparent";
    ctx.shadowBlur = 0;
    ctx.shadowOffsetX = 0;
    ctx.shadowOffsetY = 0;
  }

  if (options.style === dashed) {
    ctx.setLineDash([10, 5]);
  } else if (options.style === dotted) {
    ctx.setLineDash([2, 4]);
  } else {
    ctx.setLineDash([]);
  }

  ctx.beginPath();
  ctx.moveTo(20, canvas.height / 2);
  ctx.lineTo(canvas.width - 20, canvas.height / 2);
  ctx.stroke();
  ctx.globalAlpha = 1.0;
};

function setAutoIntrinsicSize(canvas) {
  const el = canvas?.parentElement;

  if (el) {
    const rect = el.getBoundingClientRect();
    const ratio = window.devicePixelRatio || 1;

    canvas.width = rect.width * ratio;
    canvas.height = rect.height * ratio;

    canvas.getContext("2d").scale(ratio, ratio);

    const s = state.get(canvas);
    s.dotnet?.invokeMethodAsync('OnIntrinsicSizeSet', Math.round(canvas.width), Math.round(canvas.height));
  }
}

export const fluentCxSignature = {
  initialize: function (canvas, dotnetRef, opts, backgroundImage, watermarkImage) {
    const s = updateOptions(canvas, opts);
    s.dotnet = dotnetRef;

    const resizeObserver = new ResizeObserver((entries) => {
    });

    resizeObserver.observe(canvas.parentElement);

    ensureImages(s, backgroundImage, watermarkImage, () => {
      renderAll(canvas, s.strokes, s, null);
    });

    s.strokes = s.strokes || [];
    attachInput(canvas, dotnetRef);
    renderAll(canvas, s.strokes, s, null);
  },

  setOptions: function (canvas, opts, backgroundImage, watermarkImage) {
    const s = updateOptions(canvas, opts);

    ensureImages(s, backgroundImage, watermarkImage, () => {
      renderAll(canvas, s.strokes, s, null);
    });

    renderAll(canvas, s.strokes, s, null);
  },

  setTool: function (canvas, tool) {
    const s = state.get(canvas);

    if (!s) {
      return;
    }

    s.tool = tool === eraser ? eraser : pen;
  },

  render: function (canvas, strokes, opts, backgroundImage, watermarkImage) {
    const s = updateOptions(canvas, opts);
    s.strokes = Array.isArray(strokes) ? clone(strokes) : [];

    ensureImages(s, backgroundImage, watermarkImage, () => {
      renderAll(canvas, s.strokes, s, null);
    });

    renderAll(canvas, s.strokes, s, null);
  },

  clear: function (canvas, opts, backgroundImage, watermarkImage) {
    const s = updateOptions(canvas, opts);
    s.strokes = [];

    ensureImages(s, backgroundImage, watermarkImage, () => {
      renderAll(canvas, s.strokes, s, null);
    });

    renderAll(canvas, s.strokes, s, null);
  },

  detachInput: function (canvas) {
    detachInput(canvas);
  },

  drawPreviewLine: function (canvas, options) {
    drawPreviewLine(canvas, options);
  },

  setAutoIntrinsicSize: function (canvas) {
    setAutoIntrinsicSize(canvas);
  }
};
