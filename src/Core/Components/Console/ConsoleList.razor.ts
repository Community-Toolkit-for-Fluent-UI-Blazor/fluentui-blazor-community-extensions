export namespace FluentUI.Blazor.Community.Components.ConsoleList {

  interface ScrollState {
    element: HTMLElement;
    userIsAtBottom: boolean;
  }

  const scrollStates: WeakMap<HTMLElement, ScrollState> = new WeakMap();
  export function Initialize(element: HTMLElement, dotnetRef: any): void {
    if (!element) {
      return;
    }

    const state: ScrollState = {
      element,
      userIsAtBottom: true
    };

    scrollStates.set(element, state);

    element.addEventListener("scroll", () => {
      const atBottom = IsAtBottom(element);
      state.userIsAtBottom = atBottom;

      dotnetRef.invokeMethodAsync("OnScrollChanged", atBottom);
    });
  }

  export function Dispose(element: HTMLElement): void {
    if (!element) {
      return;
    }

    element.removeEventListener("scroll", () => { });
  }

  export function ScrollToBottom(element: HTMLElement): void {
    if (!element) {
      return;
    }

    requestAnimationFrame(() => {
      const rows = Array.from(element.querySelectorAll("tbody > tr:not([aria-hidden='true'])")) as HTMLElement[];

      if (rows.length === 0) {
        element.scrollTop = element.scrollHeight;
        return;
      }

      const lastRow = rows[rows.length - 1];
      lastRow.scrollIntoView({ block: "end", behavior: "auto" });
    });
  }

  export function IsAtBottom(element: HTMLElement, threshold: number = 5): boolean {
    if (!element) {
      return false;
    }

    const distance =
      element.scrollHeight - element.scrollTop - element.clientHeight;

    return distance < threshold;
  }

  export function AutoScrollIfNeeded(element: HTMLElement, autoscroll: boolean): void {
    if (!element) {
      return;
    }

    const state = scrollStates.get(element);

    if (!state) {
      return;
    }

    if (autoscroll && state.userIsAtBottom) {
      ScrollToBottom(element);
    }
  }
}
