export namespace FluentUI.Blazor.Community.Resizer {

  interface ResizerInstance {
    id: string,
    dotNetHelper: any,
    element: HTMLElement,
    originalWidth: number,
    originalHeight: number,
    orientation: number,
    originalMouseX: number,
    originalMouseY: number,
    resizers: any,
    tileGrid: HTMLElement | null,
    dropzone: HTMLElement | null,
    columnSpan: number,
    rowSpan: number,
    minimumWidth: number,
    minimumHeight: number
  }

  const _resizerComponents = [] as ResizerInstance[];
  export function Initialize(id: string, dotNetHelper: any): void {
    const element = document.getElementById(id) as HTMLElement;

    if (!element) {
      return;
    }

    const child = element.getElementsByClassName('fluentcx-resizer-child-content-container')[0].children[0] as HTMLElement;
    const resizers = element.querySelectorAll(".fluentcx-resizer-handler");
    //const tileGrid = document.getElementById(tileGridId);
    const dropzone = null;//tileGrid ? element.parentElement : null; // If it is in tilegrid, get the parent (the drop zone)
    const { left, top, width, height } = element.getBoundingClientRect();

    const instance: ResizerInstance = {
      id: id,
      dotNetHelper: dotNetHelper,
      element: element,
      originalWidth: width,
      originalHeight: height,
      orientation: -1,
      originalMouseX: 0,
      originalMouseY: 0,
      resizers: resizers,
      tileGrid: null,//tileGrid,
      dropzone: dropzone,
      columnSpan: 0,
      rowSpan: 0,
      minimumWidth: GetMinimumWidth(child),
      minimumHeight: GetMinimumHeight(child)
    };

    _resizerComponents.push(instance);

    for (let i = 0; i < resizers.length; i++) {
      const current = resizers[i] as HTMLElement;
      current.addEventListener('mousedown', function (e: MouseEvent) {
        BeginResize(id, current, e);
      })
    }
  }

  function GetMinimumWidth(child: HTMLElement): number {
    if (!child) {
      return 20;
    }

    let tagName = child.tagName.toLowerCase();

    if (tagName === 'fluent-card') {
      return 50;
    }

    if (tagName === 'fluent-button') {
      const value = parseInt(child.style.minWidth);
      return isNaN(value) ? 40 : value;
    }

    return 20;
  }

  function GetMinimumHeight(child: HTMLElement): number {
    if (!child) {
      return 20;
    }

    let tagName = child.tagName.toLowerCase();

    if (tagName === 'fluent-card') {
      return 50;
    }

    if (tagName === 'fluent-button') {
      return 32;
    }

    return 20;
  }

  function GetFromId(id: string): ResizerInstance | null {
    for (let i = 0; i < _resizerComponents.length; ++i) {
      if (_resizerComponents[i].id == id) {
        return _resizerComponents[i];
      }
    }

    return null;
  }
  function BeginResize(id: string, current: Element, e: MouseEvent) {
    e.preventDefault();

    const instance = GetFromId(id);

    if (!instance) {
      return;
    }

    window.addEventListener('mousemove', resize);
    window.addEventListener('mouseup', stopResize);
    window.addEventListener('keydown', cancelResize);

    instance.originalMouseX = e.pageX;
    instance.originalMouseY = e.pageY;
    instance.element.style.backgroundColor = "var(--neutral-layer-1)";

    const localInstance = instance;

    function resize(e: MouseEvent) {
      console.log(e);
    }
    function cancelResize() {
      window.removeEventListener('keydown', cancelResize);
      window.removeEventListener('mousemove', resize);
      window.removeEventListener('mouseup', stopResize);

      localInstance.element.style.width = localInstance.originalWidth + "px";
      localInstance.element.style.height = localInstance.originalHeight + "px";
    }

    function stopResize() {
      window.removeEventListener('keydown', cancelResize);
      window.removeEventListener('mousemove', resize);
      window.removeEventListener('mouseup', stopResize);

      //const value = {
      //  id: instance.id,
      //  orientation: instance.orientation,
      //  originalSize: {
      //    width: instance.originalWidth,
      //    height: instance.originalHeight
      //  },
      //  newSize: {
      //    width: instance.newWidth,
      //    height: instance.newHeight
      //  },
      //  rowSpan: instance.rowSpan,
      //  columnSpan: instance.columnSpan
      //}

      //instance.dotNetHelper.invokeMethodAsync('Resized', value);

      //if (instance.tileGrid != null) {
      //  instance.dropZone.style.backgroundColor = 'transparent';
      //  instance.element.style.width = "100%";
      //  instance.element.style.height = "100%";
      //  const { left, top, width, height } = instance.element.getBoundingClientRect();
      //  instance.originalWidth = width;
      //  instance.originalHeight = height;
      //}
      //else {
      //  instance.element.style.backgroundColor = 'transparent';
      //  const { left, top, width, height } = instance.element.getBoundingClientRect();
      //  instance.originalWidth = width;
      //  instance.originalHeight = height;
      //}
    }
  }

}
