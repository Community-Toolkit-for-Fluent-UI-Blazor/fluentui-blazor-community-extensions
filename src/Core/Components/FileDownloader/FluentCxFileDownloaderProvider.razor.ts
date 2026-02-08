 export namespace FluentUI.Blazor.Community.Components.FileDownloaderProvider {

  interface FileBuffer {
    chunks: Uint8Array[];
    cancelled: boolean;
  }

  const files: Map<string, FileBuffer> = new Map();

  export function InitializeStream(filename: string): void {
    if (!files.has(filename)) {
      files.set(filename, { chunks: [], cancelled: false });
    }
  }

  export function Stream(filename: string, chunk: Uint8Array): void {
    const file = files.get(filename);

    if (!file || file.cancelled) {
      return;
    }

    file.chunks.push(chunk);
   }

   export function CancelStream(filename: string): void {
     const file = files.get(filename);
     if (file) {
       file.cancelled = true;
       file.chunks = [];
     }
   }

  export function DownloadStream(filename: string, target: string, mimeType: string): void {
    const file = files.get(filename);
    if (!file || file.cancelled || file.chunks.length === 0) {
      return;
    }

    const totalLength = file.chunks.reduce((acc, c) => acc + c.length, 0);

    const merged = new Uint8Array(totalLength);
    let offset = 0;

    for (const chunk of file.chunks) {
      merged.set(chunk, offset);
      offset += chunk.length;
    }

    openFile(filename, merged, true, target, mimeType);
    destroyStream(filename);
  }

  export function OpenFile(filename: string, content: Uint8Array, target: string, mimeType: string): void {
    openFile(filename, content, true, target, mimeType);
  }

  function destroyStream(filename: string): void {
    files.delete(filename);
  }

  function openFile(filename: string, content: Uint8Array, download: boolean, target: string, mimeType: string): void {
    const safeBuffer = new Uint8Array(content).buffer;
    const blob = new Blob([safeBuffer], { type: mimeType });
    const file = new File([blob], filename, { type: mimeType });
    const url = URL.createObjectURL(file);

    const a = document.createElement("a");
    a.href = url;
    a.target = target;

    if (download) {
      a.download = filename;
    }

    document.body.appendChild(a);
    a.click();

    URL.revokeObjectURL(url);
    a.remove();
  }
}
