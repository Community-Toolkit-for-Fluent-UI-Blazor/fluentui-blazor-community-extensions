export namespace FluentUI.Blazor.Community.AudioVisualizer {
  export enum VisualizerMode {
    Spectrum = 0,
    Waveform = 1,
    Spatial = 2,
    Vortex = 3,
    ParticlesField = 4,
    RadialWaveform = 5,
    Tunnel = 6,
    ConstellationField = 7,
    Fractal = 8
  }

  interface Star {
    x: number;
    y: number;
    z: number;
  }

  interface Particle {
    x: number;
    y: number;
    angle: number;
    speed: number;
    size: number;
  }

  interface Circle {
    r: number;
  }

  interface ConstellationPoint {
    x: number;
    y: number;
  }

  interface AudioVisualizerInstance {
    id: string;
    audio: HTMLAudioElement;
    mode: VisualizerMode;
    color: string | null;

    // Dimensions CSS (logiques)
    cssWidth: number;
    cssHeight: number;

    // Dimensions réelles (pixels du buffer)
    width: number;
    height: number;

    centerX: number;
    centerY: number;
    radius: number;

    circles: Circle[];
    stars: Star[];
    particles: Particle[];
    constellations: ConstellationPoint[];

    analyzer: AnalyserNode;
    bufferLength: number;
    dataArray: Uint8Array<ArrayBuffer>;
    audioCtx: AudioContext;

    hueBase: number;
    time: number;
  }

  const _instances: Record<string, AudioVisualizerInstance> = {};

  function bandEnergy(data: Uint8Array, startHz: number, endHz: number): number {
    const bins = data.length;
    const maxHz = 22050;
    const i0 = Math.floor((startHz / maxHz) * bins);
    const i1 = Math.floor((endHz / maxHz) * bins);
    let sum = 0;
    for (let i = i0; i <= i1; i++) sum += data[i];
    return sum / Math.max(1, i1 - i0 + 1);
  }

  export function Initialize(id: string, audioElementId: string, mode: VisualizerMode, color: string | null): void {
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas) return;

    const audio = document.getElementById(audioElementId) as HTMLAudioElement;
    if (!audio) return;

    const audioCtx = new AudioContext();
    const analyser = audioCtx.createAnalyser();
    const source = audioCtx.createMediaElementSource(audio);
    source.connect(analyser);
    analyser.connect(audioCtx.destination);
    analyser.fftSize = 2048;

    audio.addEventListener("play", () => {
      if (audioCtx.state === "suspended") audioCtx.resume();
    });

    const bufferLength = analyser.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    // Dimensions CSS (avant ResizeCanvas)
    const cssWidth = canvas.clientWidth;
    const cssHeight = canvas.clientHeight;

    // Dimensions réelles (seront mises à jour dans ResizeCanvas)
    const width = canvas.width;
    const height = canvas.height;

    const circles = Array.from({ length: 50 }, (_, i) => ({ r: i * 40 }));
    const stars = Array.from({ length: 300 }, () => ({
      x: (Math.random() - 0.5) * cssWidth,
      y: (Math.random() - 0.5) * cssHeight,
      z: Math.random() * 1 + 0.2
    }));
    const particles = Array.from({ length: 200 }, () => ({
      x: cssWidth / 2,
      y: cssHeight / 2,
      angle: Math.random() * Math.PI * 2,
      speed: Math.random() * 2,
      size: Math.random() * 3
    }));
    const constellations = Array.from({ length: 100 }, () => ({
      x: Math.random() * cssWidth,
      y: Math.random() * cssHeight
    }));

    _instances[id] = {
      id,
      audio,
      mode,
      color,

      cssWidth,
      cssHeight,

      width,
      height,

      centerX: cssWidth / 2,
      centerY: cssHeight / 2,
      radius: 100,

      circles,
      stars,
      particles,
      constellations,

      analyzer: analyser,
      bufferLength,
      dataArray,
      audioCtx,

      hueBase: 200,
      time: 0
    };
  }
  export function SetMode(id: string, mode: VisualizerMode): void {
    const instance = _instances[id];
    if (!instance) return;

    instance.mode = mode;

    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas) return;

    const ctx = canvas.getContext("2d")!;
    const analyser = instance.analyzer;
    const dataArray = instance.dataArray;

    let last = performance.now();

    function drawBranch(x: number, y: number, len: number, angle: number, depth: number, freq: number) {
      if (depth === 0) return;
      const x2 = x + len * Math.cos(angle);
      const y2 = y + len * Math.sin(angle);

      ctx.beginPath();
      ctx.moveTo(x, y);
      ctx.lineTo(x2, y2);
      ctx.strokeStyle = `hsl(${freq},100%,50%)`;
      ctx.stroke();

      drawBranch(x2, y2, len * 0.7, angle - 0.5, depth - 1, freq);
      drawBranch(x2, y2, len * 0.7, angle + 0.5, depth - 1, freq);
    }

    function render() {
      requestAnimationFrame(render);

      const now = performance.now();
      const dt = Math.min(0.05, (now - last) / 1000);
      last = now;
      instance.time += dt;

      analyser.smoothingTimeConstant = 0.85;
      analyser.getByteFrequencyData(dataArray);

      const width = instance.cssWidth;
      const height = instance.cssHeight;
      const centerX = instance.centerX;
      const centerY = instance.centerY;
      const radius = instance.radius;

      ctx.clearRect(0, 0, width, height);

      switch (instance.mode) {

        case VisualizerMode.Spectrum: {
          const barWidth = 4;
          const barSpacing = 2;
          for (let i = 0; i < instance.bufferLength; i++) {
            const value = dataArray[i];
            const x = centerX + (i - instance.bufferLength / 2) * (barWidth + barSpacing);
            const y = height - value;
            const scale = 1 + value / 256;
            ctx.fillStyle = `rgb(${value},${value / 2},${255 - value})`;
            ctx.fillRect(x, y, barWidth, value * scale);
          }
          break;
        }

        case VisualizerMode.Waveform: {
          const timeArray = new Uint8Array(analyser.fftSize);
          analyser.getByteTimeDomainData(timeArray);

          ctx.lineWidth = 2;
          ctx.strokeStyle = instance.color ?? "#78e8ff";
          ctx.beginPath();

          const slice = width / timeArray.length;
          for (let i = 0; i < timeArray.length; i++) {
            const v = timeArray[i] / 128.0;
            const y = v * height / 2;
            const x = i * slice;
            if (i === 0) ctx.moveTo(x, y);
            else ctx.lineTo(x, y);
          }
          ctx.stroke();
          break;
        }

        case VisualizerMode.Spatial: {
          const bass = bandEnergy(dataArray, 20, 200) / 255;
          const mid = bandEnergy(dataArray, 200, 2000) / 255;
          const high = bandEnergy(dataArray, 2000, 12000) / 255;

          ctx.globalCompositeOperation = 'source-over';
          ctx.fillStyle = 'rgba(5, 8, 12, 0.35)';
          ctx.fillRect(0, 0, width, height);

          const maxR = Math.hypot(centerX, centerY);
          const breathing = 1 + Math.sin(instance.time * 2.4) * (0.05 + bass * 0.12);
          const rotation = instance.time * (0.15 + high * 0.4);
          instance.hueBase = (instance.hueBase + (20 + mid * 120) * dt) % 360;

          ctx.save();
          ctx.translate(centerX, centerY);
          ctx.rotate(rotation);
          ctx.scale(breathing, 1);

          const ringCount = 18;
          for (let i = 0; i < ringCount; i++) {
            const t = i / ringCount;
            const r = 30 + t * maxR * 0.95;
            const w = 6 + t * 22 * (0.4 + mid);
            const hue = (instance.hueBase + i * 10) % 360;
            const alpha = 0.06 + (1 - t) * (0.10 + high * 0.10);

            const g = ctx.createRadialGradient(0, 0, r - w, 0, 0, r + w);
            g.addColorStop(0, `hsla(${hue}, 90%, 65%, 0)`);
            g.addColorStop(0.5, `hsla(${hue}, 90%, 65%, ${alpha})`);
            g.addColorStop(1, `hsla(${hue}, 90%, 65%, 0)`);

            ctx.strokeStyle = g;
            ctx.lineWidth = w;
            ctx.beginPath();
            ctx.arc(0, 0, r, 0, Math.PI * 2);
            ctx.stroke();
          }
          ctx.restore();

          ctx.save();
          ctx.translate(centerX, centerY);
          ctx.globalCompositeOperation = 'lighter';
          ctx.shadowBlur = 8;

          const starSpeed = 0.9 + bass * 2.0;
          for (const s of instance.stars) {
            s.z -= 0.008 * starSpeed;
            if (s.z < 0.12) {
              s.x = (Math.random() - 0.5) * width;
              s.y = (Math.random() - 0.5) * height;
              s.z = 1;
            }

            const a = mid * 0.03;
            const cosA = Math.cos(a), sinA = Math.sin(a);
            const rx = s.x * cosA - s.y * sinA;
            const ry = s.x * sinA + s.y * cosA;
            s.x = rx; s.y = ry;

            const px = s.x / s.z;
            const py = s.y / s.z;
            const size = Math.max(0.5, (1 - s.z) * (1.5 + high * 2.5));
            const hue = (instance.hueBase + s.z * 120) % 360;

            ctx.fillStyle = `hsl(${hue}, 90%, ${60 + bass * 30}%)`;
            ctx.shadowColor = ctx.fillStyle;

            ctx.beginPath();
            ctx.arc(px, py, size, 0, Math.PI * 2);
            ctx.fill();
          }
          ctx.restore();
          break;
        }

        case VisualizerMode.Vortex: {
          analyser.getByteFrequencyData(dataArray);

          const bass = bandEnergy(dataArray, 20, 200) / 255;
          const mid = bandEnergy(dataArray, 200, 2000) / 255;
          const high = bandEnergy(dataArray, 2000, 12000) / 255;
          const speed = 1 + bass * 6;
          const rotationSpeed = 0.002 + high * 0.01;

          ctx.fillStyle = 'rgba(0, 0, 0, 0.2)';
          ctx.fillRect(0, 0, width, height);

          ctx.save();
          ctx.translate(centerX, centerY);

          for (const s of instance.stars) {
            s.z -= 0.01 * speed;

            if (s.z < 0.2) {
              s.x = (Math.random() - 0.5) * width;
              s.y = (Math.random() - 0.5) * height;
              s.z = Math.random() * 0.8 + 0.2;
            }

            const angle = Math.atan2(s.y, s.x);
            const radius = Math.sqrt(s.x * s.x + s.y * s.y);
            const newAngle = angle + rotationSpeed;

            s.x = Math.cos(newAngle) * radius;
            s.y = Math.sin(newAngle) * radius;

            const px = s.x / s.z;
            const py = s.y / s.z;
            const rawSize = (1 - s.z) * (1 + mid * 3);
            const size = Math.max(0.5, rawSize);
            const hue = 200 + Math.floor(60 * high);

            ctx.fillStyle = `hsla(${hue}, 90%, 70%, 0.8)`;
            ctx.beginPath();
            ctx.arc(px, py, size, 0, Math.PI * 2);
            ctx.fill();
          }

          ctx.restore();
          break;
        }

        case VisualizerMode.ParticlesField: {
          analyser.getByteFrequencyData(dataArray);
          const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

          for (const p of instance.particles) {
            p.x += Math.cos(p.angle) * p.speed * (avg / 100);
            p.y += Math.sin(p.angle) * p.speed * (avg / 100);

            if (p.x < 0 || p.x > width || p.y < 0 || p.y > height) {
              p.x = centerX;
              p.y = centerY;
            }

            ctx.fillStyle = `hsl(${avg},100%,50%)`;
            ctx.beginPath();
            ctx.arc(p.x, p.y, p.size, 0, Math.PI * 2);
            ctx.fill();
          }
          break;
        }

        case VisualizerMode.RadialWaveform: {
          analyser.getByteTimeDomainData(dataArray);

          ctx.beginPath();
          for (let i = 0; i < dataArray.length; i++) {
            const angle = (i / dataArray.length) * Math.PI * 2;
            const value = dataArray[i] / 128.0;
            const r = radius + (value - 1) * 50;
            const x = centerX + r * Math.cos(angle);
            const y = centerY + r * Math.sin(angle);
            if (i === 0) ctx.moveTo(x, y);
            else ctx.lineTo(x, y);
          }
          ctx.closePath();
          ctx.strokeStyle = instance.color ?? "#78e8ff";
          ctx.stroke();
          break;
        }

        case VisualizerMode.Tunnel: {
          analyser.getByteFrequencyData(dataArray);
          const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

          for (const c of instance.circles) {
            c.r -= 2 + avg / 200;
            if (c.r < 0) c.r = width;

            ctx.beginPath();
            ctx.arc(centerX, centerY, c.r, 0, Math.PI * 2);
            ctx.strokeStyle = `hsl(${c.r},100%,50%)`;
            ctx.stroke();
          }
          break;
        }

        case VisualizerMode.ConstellationField: {
          analyser.getByteFrequencyData(dataArray);
          const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

          ctx.fillStyle = instance.color ?? "#78e8ff";
          for (const s of instance.constellations) {
            ctx.beginPath();
            ctx.arc(s.x, s.y, 2, 0, Math.PI * 2);
            ctx.fill();
          }

          if (avg > 100) {
            for (let i = 0; i < instance.stars.length; i++) {
              for (let j = i + 1; j < instance.stars.length; j++) {
                if (Math.hypot(instance.stars[i].x - instance.stars[j].x, instance.stars[i].y - instance.stars[j].y) < 100) {
                  ctx.strokeStyle = "rgba(255,255,255,0.2)";
                  ctx.beginPath();
                  ctx.moveTo(instance.stars[i].x, instance.stars[i].y);
                  ctx.lineTo(instance.stars[j].x, instance.stars[j].y);
                  ctx.stroke();
                }
              }
            }
          }
          break;
        }

        case VisualizerMode.Fractal: {
          analyser.getByteFrequencyData(dataArray);
          const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;
          drawBranch(width, height, avg / 2, -Math.PI / 2, 6, avg);
          break;
        }
      }
    }

    render();
  }

  export async function Dispose(id: string): Promise<void> {
    const instance = _instances[id];
    if (!instance) return;

    await instance.audioCtx.close();
    delete _instances[id];
  }

  export function ResizeCanvas(id: string, width: number, height: number): void {
    const canvas = document.getElementById(id) as HTMLCanvasElement;
    if (!canvas) return;

    const dpr = window.devicePixelRatio || 1;

    canvas.style.width = width + "px";
    canvas.style.height = height + "px";
    canvas.width = Math.floor(width * dpr);
    canvas.height = Math.floor(height * dpr);

    const ctx = canvas.getContext("2d");
    if (!ctx) return;

    ctx.setTransform(1, 0, 0, 1, 0, 0);
    ctx.scale(dpr, dpr);

    const instance = _instances[id];
    if (!instance) return;

    instance.cssWidth = width;
    instance.cssHeight = height;

    instance.width = canvas.width;
    instance.height = canvas.height;

    instance.centerX = width / 2;
    instance.centerY = height / 2;
  }
}
