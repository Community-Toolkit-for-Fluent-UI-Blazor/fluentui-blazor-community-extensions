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
    minimumHeight: number,
    newWidth: number,
    newHeight: number
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
      minimumHeight: GetMinimumHeight(child),
      newWidth: 0,
      newHeight: 0
    };

    _resizerComponents.push(instance);

    for (let i = 0; i < resizers.length; i++) {
      const current = resizers[i] as HTMLElement;
      current.addEventListener('mousedown', function (e: MouseEvent) {
        BeginResize(id, current, e);
      })
    }
  }
  export function Dispose(id: string) {
    for (let i = _resizerComponents.length - 1; i > 0; i--) {
      if (_resizerComponents[i].id == id) {
        for (let j = 0; j < _resizerComponents[i].resizers.length; ++j) {
          _resizerComponents[i].resizers[j].removeEventListener("mousedown", function (e: MouseEvent) {
            BeginResize(id, _resizerComponents[i].resizers[j], e);
          });
        }

        _resizerComponents.splice(i, 1);
      }
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

    const checkInstance = GetFromId(id);

    if (!checkInstance) {
      return;
    }

    const instance = checkInstance;

    window.addEventListener('mousemove', resize);
    window.addEventListener('mouseup', stopResize);
    window.addEventListener('keydown', cancelResize);

    instance.originalMouseX = e.pageX;
    instance.originalMouseY = e.pageY;
    instance.element.style.backgroundColor = "var(--neutral-layer-1)";


    function resize(e: MouseEvent) {
      console.log(e);

      if (instance.tileGrid != null &&
        instance.tileGrid !== undefined &&
        instance.dropzone != null &&
        instance.dropzone !== undefined) {

        //let width = instance.originalWidth + (e.pageX - instance.originalMouseX);
        //let height = instance.originalHeight + (e.pageY - instance.originalMouseY);
        //const rect = instance.dropzone.getBoundingClientRect();
        //const columnEnd = parseInt(instance.dropzone.style.gridColumnEnd.replace('span', '').trim());
        //const rowEnd = parseInt(instance.dropzone.style.gridRowEnd.replace('span', '').trim());
        //const gridColumnWidth = rect.width / columnEnd;
        //const gridRowHeight = rect.height / rowEnd;
        //const maxSize = instance.tileGrid.getBoundingClientRect();
        //instance.dropzone.style.backgroundColor = "var(--neutral-layer-1)";

        //if (current.classList.contains('fluentcx-resizer-handler-cursor-nwse')) {
        //  instance.orientation = 2;
        //  resizeHorizontally(width, gridColumnWidth, maxSize.width);
        //  resizeVertically(height, gridRowHeight, maxSize.height);
        //}
        //else if (current.classList.contains('fluentcx-resizer-handler-cursor-ns')) {
        //  instance.orientation = 1;
        //  resizeVertically(height, gridRowHeight, maxSize.height);
        //}
        //else if (current.classList.contains('fluentcx-resizer-handler-cursor-ew')) {
        //  instance.orientation = 0;
        //  resizeHorizontally(width, gridColumnWidth, maxSize.width);
        //}
      }
      else {
        const width = instance.originalWidth + (e.pageX - instance.originalMouseX);
        const height = instance.originalHeight + (e.pageY - instance.originalMouseY);

        if (current.classList.contains('fluentcx-resizer-handler-cursor-nwse')) {
          instance.orientation = 2;

          if (width > instance.minimumWidth) {
            instance.newWidth = width;
            instance.element.style.width = width + "px";
          }

          if (height > instance.minimumHeight) {
            instance.newHeight = height;
            instance.element.style.height = height + "px";
          }
        }
        else if (current.classList.contains('fluentcx-resizer-handler-cursor-ns')) {
          instance.orientation = 1;

          if (height > instance.minimumHeight) {
            instance.newHeight = height;

            if (instance.element) {
              instance.element.style.height = height + "px";
              instance.element.style.width = instance.originalWidth + "px";
            }
          }
        }
        else if (current.classList.contains('fluentcx-resizer-handler-cursor-ew')) {
          instance.orientation = 0;

          if (width > instance.minimumWidth) {
            instance.newWidth = width;

            if (instance.element) {
              instance.element.style.height = instance.originalHeight + "px";
              instance.element.style.width = width + "px";
            }
          }
        }
      }

    }
    function cancelResize() {
      window.removeEventListener('keydown', cancelResize);
      window.removeEventListener('mousemove', resize);
      window.removeEventListener('mouseup', stopResize);

      instance.element.style.width = instance.originalWidth + "px";
      instance.element.style.height = instance.originalHeight + "px";
    }

    function stopResize() {
      window.removeEventListener('keydown', cancelResize);
      window.removeEventListener('mousemove', resize);
      window.removeEventListener('mouseup', stopResize);

      const value = {
        id: instance.id,
        orientation: instance.orientation,
        originalSize: {
          width: instance.originalWidth,
          height: instance.originalHeight
        },
        newSize: {
          width: instance.newWidth,
          height: instance.newHeight
        },
        rowSpan: instance.rowSpan,
        columnSpan: instance.columnSpan
      }

      instance.dotNetHelper.invokeMethodAsync('Resized', value);

      if (instance.tileGrid != null) {
        if (instance.dropzone != null) {
          instance.dropzone.style.backgroundColor = 'transparent';
        }

        instance.element.style.width = "100%";
        instance.element.style.height = "100%";

        const { left, top, width, height } = instance.element.getBoundingClientRect();
        instance.originalWidth = width;
        instance.originalHeight = height;
      }
      else {
        instance.element.style.backgroundColor = 'transparent';
        const { left, top, width, height } = instance.element.getBoundingClientRect();
        instance.originalWidth = width;
        instance.originalHeight = height;
      }
    }
  }
}
