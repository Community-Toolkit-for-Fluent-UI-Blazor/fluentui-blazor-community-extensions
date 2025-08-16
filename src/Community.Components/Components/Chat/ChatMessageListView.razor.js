const _instances = [];

export function copyToClipboard(dotNetReference, text) {
  if (window.isSecureContext) {
    navigator.clipboard.writeText(text).then(function () {
      dotNetReference.invokeMethodAsync("OnCopied");
    })
      .catch(function (error) {
        dotNetReference.invokeMethodAsync("OnCopyError");
        console.log(error);
      })
  }
}

export function scrollToBottom() {

}

export async function initialize(id, dotNetReference, chunkSize) {
  const instance = {
    element: document.getElementById(id),
    audioChunks: [],
  };

  const audioStream = await navigator.mediaDevices.getUserMedia({ audio: true });
  instance.mediaRecorder = new MediaRecorder(audioStream);
  instance.mediaRecorder.ondataavailable = (e) => {
    instance.audioChunks.push(e.data);
  }

  instance.mediaRecorder.onstop = async () => {

    for (let i = 0; i < instance.audioChunks.length; i++) {
      await dotNetReference.invokeMethodAsync('StartAudioChunkStream');
      const blob = instance.audioChunks[i];

      const a = await blob.arrayBuffer();
      const view = new Uint8Array(a);
      const array = Array.from(view);

      for (let j = 0; j < array.length; j += chunkSize) {
        const chunk = array.slice(j, j + chunkSize);
        await dotNetReference.invokeMethodAsync('ReceiveAudioChunk', chunk);
      }

      await dotNetReference.invokeMethodAsync('AudioChunkCompleted');
    }

    instance.audioChunks = [];
  }

  _instances[id] = instance;
}

export function startAudioRecording(id) {
  const instance = _instances[id];

  if (instance) {
    instance.mediaRecorder.start();
  }
}

export function endAudioRecording(id) {
  const instance = _instances[id];

  if (instance) {
    instance.mediaRecorder.stop();
  }
}
