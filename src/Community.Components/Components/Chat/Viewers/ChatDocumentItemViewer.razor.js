export function loadVideo(id, content, contentType) {
  const videoElement = document.getElementById("video-player-" + id);
  if (videoElement) {
    const videoBlob = new Blob([Uint8Array.from(atob(content), c => c.charCodeAt(0))], { type: contentType });
    const videoUrl = URL.createObjectURL(videoBlob);

    videoElement.src = videoUrl;

    URL.revokeObjectURL(videoUrl);
  }
}
