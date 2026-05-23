export namespace FluentUI.Blazor.Community.Components.Clipboard {
  function isSupported(): boolean {
    return isSecureContext && !!navigator.clipboard;
  }

  export function writeText(text: string): Promise<boolean> {
    if (!isSupported()) {
      return Promise.resolve(false);
    }

    return navigator.clipboard.writeText(text).then(() => true).catch(() => false);
  }

  export function readText(): Promise<string | null> {
    if (!isSupported()) {
      return Promise.resolve(null);
    }

    return navigator.clipboard.readText().then(text => text).catch(() => null);
  }

  export function writeHtml(html: string): Promise<boolean> {
    if (!isSupported() || !navigator.clipboard.write) {
      return Promise.resolve(false);
    }

    const blob = new Blob([html], { type: "text/html" });
    const item = new ClipboardItem({ "text/html": blob });

    return navigator.clipboard.write([item]).then(() => true).catch(() => false);
  }

  export function writeImage(blob: Blob): Promise<boolean> {
    if (!isSupported() || !navigator.clipboard.write) {
      return Promise.resolve(false);
    }

    const item = new ClipboardItem({ [blob.type]: blob });

    return navigator.clipboard.write([item]).then(() => true).catch(() => false);
  }
}
