export namespace FluentUI.Blazor.Community.FileManager {
  export function InsideDialog(id: string) {
    const slideshow = document.getElementById(id);


    if (!slideshow) return;

    const body = slideshow.closest("fluent-dialog-body") as HTMLElement | null;


    if (!body) return;

    const shadow = body.shadowRoot;

    if (!shadow) return;

    const content = shadow.querySelector('[part="content"]') as HTMLElement | null;

    if (!content) return;

    const slot = content.querySelector('slot') as HTMLSlotElement | null;

    if (!slot) return;

    const nodes = slot.assignedElements ? slot.assignedElements() : slot.assignedNodes();

    if (!nodes || nodes.length === 0) return;

    const wrapper = nodes[0] as HTMLElement;

    if (!wrapper) return;

    const titleSlot = shadow.querySelector('slot[name="title"]') as HTMLSlotElement | null;
    const actionsSlot = shadow.querySelector('slot[name="action"]') as HTMLSlotElement | null;
    const titleEl = titleSlot?.assignedElements()?.[0] as HTMLElement | undefined;
    const actionsEl = actionsSlot?.assignedElements()?.[0] as HTMLElement | undefined;
    const bodyHeight = body.getBoundingClientRect().height;
    const titleHeight = titleEl?.getBoundingClientRect().height ?? 0;
    const actionsHeight = actionsEl?.getBoundingClientRect().height ?? 0;
    const availableHeight = bodyHeight - titleHeight - actionsHeight - 20;

    wrapper.style.display = "flex";
    wrapper.style.flexDirection = "column";
    wrapper.style.height = `${availableHeight}px`;
    wrapper.style.minHeight = "0"; 
  }
}
