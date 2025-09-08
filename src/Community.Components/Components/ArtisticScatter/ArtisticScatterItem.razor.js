export function initialize(reference, dotNetRef) {
  reference.addEventListener('animationend', () => {
    dotNetRef.invokeMethodAsync('OnAnimationCompleted');
  });
}

export function dispose(reference) {
  reference.removeEventListener('animationend', () => { });
}
