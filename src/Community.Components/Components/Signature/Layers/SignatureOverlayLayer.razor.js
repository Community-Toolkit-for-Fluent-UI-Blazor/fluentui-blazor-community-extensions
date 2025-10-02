const NS = "http://www.w3.org/2000/svg";

function mapAlign(val) {
  ["start", "middle", "end"][val] || "middle";
}

function clamp(val, min, max) {
  return Math.max(min, Math.min(max, val));
}

function clampPercent(v)
{
  const n = num(v, 50);
  return clamp(n, 0, 100);
}

function num(v, d = 0) {
  const n = Number(v);
  return Number.isFinite(n) ? n : d;
}

export function updateOverlay(overlay, opt) {
  overlay.innerHTML = "";
  const { width, height } = overlay.getBoundingClientRect();

  const g = document.createElementNS(NS, "g");

  if (opt?.imageUrl) {
    const img = document.createElementNS(NS, "image");
    img.setAttributeNS("http://www.w3.org/1999/xlink", "href", opt.imageUrl);
    img.setAttribute("x", 0);
    img.setAttribute("y", 0);
    img.setAttribute("width", width);
    img.setAttribute("height", height);
    img.setAttribute("opacity", num(opt.opacity, 0.1));
    g.appendChild(img);
  }

  if (opt?.text) {
    const align = mapAlign(opt?.textAlign)
    const anchor = align;
    const color = opt.color || "#000";
    const opacity = num(opt.opacity, 0.1);
    const fontFamily = opt.fontFamily || "sans-serif";
    const fontWeight = opt.fontWeight || "bold";
    const fontSize = num(opt.fontSize, 48);
    const letterSpacing = num(opt.letterSpacing, 0.0);
    const px = (clampPercent(opt?.position?.item1 ?? opt?.position?.x ?? 50) / 100) * width;
    const py = (clampPercent(opt?.position?.item2 ?? opt?.position?.y ?? 50) / 100) * height;
    const rotation = num(opt.rotation, 0);

    if (opt.repeat) {
      // Tile the watermark text in a rotated grid
      const stepX = fontSize * 8;
      const stepY = fontSize * 6;
      const startX = -width;
      const endX = width * 2;
      const startY = -height;
      const endY = height * 2;
      const tile = document.createElementNS(NS, "g");
      tile.setAttribute(
        "transform",
        `rotate(${rotation}, ${width / 2}, ${height / 2})`,
      );
      for (let y = startY; y < endY; y += stepY) {
        for (let x = startX; x < endX; x += stepX) {
          const t = document.createElementNS(NS, "text");
          t.textContent = opt.text;
          t.setAttribute("x", x);
          t.setAttribute("y", y);
          t.setAttribute("fill", color);
          t.setAttribute("opacity", opacity);
          t.setAttribute("font-family", fontFamily);
          t.setAttribute("font-weight", fontWeight);
          t.setAttribute("font-size", `${fontSize}px`);
          t.setAttribute("text-anchor", anchor);
          t.setAttribute("dominant-baseline", "middle");
          if (letterSpacing)
            t.setAttribute("letter-spacing", `${letterSpacing}px`);
          tile.appendChild(t);
        }
      }
      g.appendChild(tile);
    } else {
      const t = document.createElementNS(NS, "text");
      t.textContent = opt.text;
      t.setAttribute("x", px);
      t.setAttribute("y", py);
      t.setAttribute("fill", color);
      t.setAttribute("opacity", opacity);
      t.setAttribute("font-family", fontFamily);
      t.setAttribute("font-weight", fontWeight);
      t.setAttribute("font-size", `${fontSize}px`);
      t.setAttribute("text-anchor", anchor);
      t.setAttribute("dominant-baseline", "middle");
      if (letterSpacing) t.setAttribute("letter-spacing", `${letterSpacing}px`);
      t.setAttribute("transform", `rotate(${rotation}, ${px}, ${py})`);
      g.appendChild(t);
    }
  }

  overlay.appendChild(g);
}
