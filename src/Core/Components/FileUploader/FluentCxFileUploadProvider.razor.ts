export namespace FluentUI.Blazor.Community.Components.FileDownloaderProvider {

  interface FileBuffer {
    chunks: Uint8Array<ArrayBuffer>[];
    cancelled: boolean;
  }

  const files: Map<string, FileBuffer> = new Map();

  export function InitializeStream(id: string): void {
    if (!files.has(id)) {
      files.set(id, { chunks: [], cancelled: false });
    }
  }

  export function Stream(id: string, chunk: Uint8Array<ArrayBuffer>): void {
    const file = files.get(id);

    if (!file || file.cancelled) {
      return;
    }

    file.chunks.push(chunk);
  }

  export function CancelStream(id: string): void {
    const file = files.get(id);
    if (file) {
      file.cancelled = true;
      file.chunks = [];
    }
  }

  export function FinalizeStream(id: string, contentType: string): string {
    const file = files.get(id);

    if (!file || file.cancelled) {
      return '';
    }

    const blob = new Blob(file.chunks, { type: contentType });

    return URL.createObjectURL(blob);
  }

  export function RevokeUrl(url: string): void {
    URL.revokeObjectURL(url);
  }

  function destroyStream(id: string): void {
    files.delete(id);
  }
}
