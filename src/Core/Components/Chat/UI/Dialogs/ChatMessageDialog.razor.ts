export namespace FluentUI.Blazor.Community.ChatMessageDialog {
  export function SetDialogContentHeight(id: string) {
    const body = document.getElementById(id);

    if (!body) {
      return;
    }

    const shadow = body.shadowRoot;

    if (!shadow) {
      return;
    }

    const content = shadow.querySelector('[part="content"]') as HTMLElement | null;

    if (!content) {
      return;
    }

    const slot = content.querySelector('slot') as HTMLSlotElement | null;

    if (!slot) {
      return;
    }

    const nodes = slot.assignedElements ? slot.assignedElements() : slot.assignedNodes();

    if (!nodes || nodes.length === 0) {
      return;
    }

    const wrapper = nodes[0] as HTMLElement;

    if (!wrapper) {
      return;
    }

    wrapper.style.display = 'flex';
    wrapper.style.height = '100%';
    wrapper.style.minHeight = '0';
  }
}
