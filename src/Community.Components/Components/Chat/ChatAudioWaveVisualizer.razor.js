export function initialize(id) {
  const element = document.getElementById(id);

  if (element) {
    const children = element.querySelectorAll(".bar");

    for (let i = 0; i < children.length; i++) {
      const bar = children[i];

      if (bar) {
        const angle = (360 / children.length) * i;
        bar.style.transform = `rotate(${angle}deg) translateY(-40px)`;
        bar.style.setProperty('--bar-height', (10 + Math.random() * 15) + 'px');
      }
    }

    setInterval(() => {
      const bars = document.querySelectorAll('.bar');
      bars.forEach(bar => {
        const newHeight = 10 + Math.random() * 15;
        bar.style.setProperty('--bar-height', `${newHeight}px`);
      });
    }, 300);
  }
}
