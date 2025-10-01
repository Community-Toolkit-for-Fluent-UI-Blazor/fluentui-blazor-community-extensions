const spectrum = 0;
const waveform = 1;
const spatial = 2;
const vortex = 3;
const particlesField = 4;
const radialWaveform = 5;
const tunnel = 6;
const constellationField = 7;
const fractal = 8;

const _instances = {};

function bandEnergy(data, startHz, endHz) {
  const bins = data.length;
  const maxHz = 22050; // approx
  const i0 = Math.floor((startHz / maxHz) * bins);
  const i1 = Math.floor((endHz / maxHz) * bins);
  let sum = 0;
  for (let i = i0; i <= i1; i++) sum += data[i];
  return sum / Math.max(1, i1 - i0 + 1);
}

function initialize(id, audioElement, mode, cover, color) {
  const canvas = document.getElementById(id);

  const circles = Array.from({ length: 50 }, (x, i) => (
    {
      r: i * 40
    }));

  const stars = Array.from({ length: 300 }, () => ({
    x: (Math.random() - 0.5) * canvas.width,
    y: (Math.random() - 0.5) * canvas.height,
    z: Math.random() * 1 + 0.2
  }));

  const particles = Array.from({ length: 200 }, () => ({
    x: canvas.width / 2,
    y: canvas.height / 2,
    angle: Math.random() * Math.PI * 2,
    speed: Math.random() * 2,
    size: Math.random() * 3
  }));

  const constellations = Array.from({ length: 100 }, () => ({
    x: Math.random() * canvas.width,
    y: Math.random() * canvas.height
  }));

  const audio = document.getElementById(audioElement);
  const audioCtx = new AudioContext();
  const analyser = audioCtx.createAnalyser();
  const source = audioCtx.createMediaElementSource(audio);
  source.connect(analyser);
  analyser.connect(audioCtx.destination);

  audio.addEventListener("play", () => {
    if (audioCtx.state === "suspended") {
      audioCtx.resume();
    }
  });

  const bufferLength = analyser.frequencyBinCount;
  const dataArray = new Uint8Array(bufferLength);

  _instances[id] = {
    audio: audio,
    mode: mode,
    cover: cover,
    color: color,
    width: canvas.width,
    height: canvas.height,
    centerX: canvas.width / 2,
    centerY: canvas.height / 2,
    radius: 100,
    circles: circles,
    stars: stars,
    particles: particles,
    constellations: constellations,
    analyzer: analyser,
    bufferLength: bufferLength,
    dataArray: dataArray,
    audioCtx: audioCtx
  };
}

function setMode(id, mode) {
  const instance = _instances[id];
  const canvas = document.getElementById(id);

  if (instance && canvas) {
    const analyser = instance.analyzer;
    const ctx = canvas.getContext("2d");
    const dataArray = instance.dataArray;
    const width = instance.width;
    const height = instance.height;
    const centerX = instance.centerX;
    const centerY = instance.centerY;
    const radius = instance.radius;
    const bufferLength = instance.bufferLength;
    const stars = instance.stars;
    const particles = instance.particles;
    const circles = instance.circles;
    const constellations = instance.constellations;
    const color = instance.color;

    function drawBranch(x, y, len, angle, depth, freq) {
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

    function draw() {
      requestAnimationFrame(draw);
      analyser.getByteFrequencyData(dataArray);
      analyser.smoothingTimeConstant = 0.85;
      ctx.clearRect(0, 0, width, height);
      analyser.getByteFrequencyData(dataArray);

      if (mode === spectrum) {
        const barWidth = 4;
        const barSpacing = 2;
        for (let i = 0; i < bufferLength; i++) {
          const value = dataArray[i];
          const x = centerX + (i - bufferLength / 2) * (barWidth + barSpacing);
          const y = height - value;
          const scale = 1 + value / 256;
          ctx.fillStyle = `rgb(${value},${value / 2},${255 - value})`;
          ctx.fillRect(x, y, barWidth, value * scale);
        }
      }
      else if (mode === waveform) {
        const timeArray = new Uint8Array(analyser.fftSize);
        analyser.getByteTimeDomainData(timeArray);

        ctx.lineWidth = 2;
        ctx.strokeStyle = color ?? "#78e8ff";
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
      }
      else if (mode === spatial) {
        analyser.getByteFrequencyData(dataArray);

        const bass = bandEnergy(dataArray, 20, 200) / 255;
        const mid = bandEnergy(dataArray, 200, 2000) / 255;
        const high = bandEnergy(dataArray, 2000, 12000) / 255;
        let speed = 0.01 + bass * 0.1;
        if (bass > 0.6) {
          speed *= 3;
        }

        ctx.fillStyle = "rgba(0,0,0,0.1)";
        ctx.fillRect(0, 0, width, height);
        ctx.save();
        ctx.translate(centerX, centerY);

        for (const s of stars) {
          s.z -= speed;

          if (s.z < 0.1) {
            s.x = (Math.random() - 0.5) * width;
            s.y = (Math.random() - 0.5) * height;
            s.z = 1;
          }

          const angle = mid * 0.05;
          const cosA = Math.cos(angle);
          const sinA = Math.sin(angle);
          const rx = s.x * cosA - s.y * sinA;
          const ry = s.x * sinA + s.y * cosA;
          s.x = rx;
          s.y = ry;

          const px = s.x / s.z;
          const py = s.y / s.z;
          const rawSize = (1 - s.z) * (2 + mid * 4);
          const size = Math.max(0.5, rawSize);
          const hue = 180 + Math.floor(150 * high);
          const lightness = 50 + bass * 30;

          ctx.fillStyle = `hsl(${hue}, 100%, ${lightness}%)`;
          ctx.shadowBlur = 20;
          ctx.shadowColor = ctx.fillStyle;

          ctx.beginPath();
          ctx.arc(px, py, size, 0, Math.PI * 2);
          ctx.fill();
        }

        ctx.restore();

        // Nebulas
        ctx.globalAlpha = 0.1 + high * 0.3;
        const nebulaHue = 280 + high * 80;
        const gradient = ctx.createRadialGradient(centerX, centerY, 0, centerX, centerY, width / 2);
        gradient.addColorStop(0, `hsla(${nebulaHue}, 100%, 60%, 0.6)`);
        gradient.addColorStop(1, "transparent");
        ctx.fillStyle = gradient;
        ctx.fillRect(0, 0, width, height);
        ctx.globalAlpha = 1;
      }
      else if (mode === vortex) {
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

        for (const s of stars) {
          s.z -= 0.01 * speed;

          if (s.z < 0.2) {
            s.x = (Math.random() - 0.5) * width;
            s.y = (Math.random() - 0.5) * height;
            s.z = Math.random() * 0.8 + 0.2;
          }

          // Rotation en spirale
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
      }
      else if (mode === particlesField) {
        analyser.getByteFrequencyData(dataArray);
        const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

        particles.forEach(p => {
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
        });
      }
      else if (mode === radialWaveform) {
        analyser.getByteTimeDomainData(dataArray);

        ctx.beginPath();
        for (let i = 0; i < dataArray.length; i++) {
          const angle = (i / dataArray.length) * Math.PI * 2;
          const value = dataArray[i] / 128.0;
          const r = radius + (value - 1) * 50;
          const x = centerX + r * Math.cos(angle);
          const y = centerY + r * Math.sin(angle);
          if (i === 0) ctx.moveTo(x, y); else ctx.lineTo(x, y);
        }
        ctx.closePath();
        ctx.strokeStyle = color ?? "#78e8ff";
        ctx.stroke();
      }
      else if (mode === tunnel) {
        analyser.getByteFrequencyData(dataArray);
        const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

        circles.forEach(c => {
          c.r -= 2 + avg / 200;
          if (c.r < 0) c.r = width;
          ctx.beginPath();
          ctx.arc(centerX, centerY, c.r, 0, Math.PI * 2);
          ctx.strokeStyle = `hsl(${c.r},100%,50%)`;
          ctx.stroke();
        });
      }
      else if (mode === constellationField) {
        analyser.getByteFrequencyData(dataArray);
        const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;

        ctx.fillStyle = color ?? "#78e8ff";
        constellations.forEach(s => {
          ctx.beginPath();
          ctx.arc(s.x, s.y, 2, 0, Math.PI * 2);
          ctx.fill();
        });

        if (avg > 100) {
          for (let i = 0; i < stars.length; i++) {
            for (let j = i + 1; j < stars.length; j++) {
              if (Math.hypot(stars[i].x - stars[j].x, stars[i].y - stars[j].y) < 100) {
                ctx.strokeStyle = "rgba(255,255,255,0.2)";
                ctx.beginPath();
                ctx.moveTo(stars[i].x, stars[i].y);
                ctx.lineTo(stars[j].x, stars[j].y);
                ctx.stroke();
              }
            }
          }
        }
      }
      else if (mode === fractal) {
        analyser.getByteFrequencyData(dataArray);
        const avg = dataArray.reduce((a, b) => a + b) / dataArray.length;
        drawBranch(width, height, avg / 2, -Math.PI / 2, 6, avg);
      }
    }

    draw();
  }
}

async function dispose(id) {
  if (_instances[id]) {
    await _instances[id].audioCtx.close();
    delete _instances[id];
  }
}

export const fluentCxAudioVisualizer = {
  initialize: (id, audioElement, mode, cover, dotNetRef) => initialize(id, audioElement, mode, cover, dotNetRef),
  dispose: (id) => dispose(id),
  setMode: (id, mode) => setMode(id, mode)
}
