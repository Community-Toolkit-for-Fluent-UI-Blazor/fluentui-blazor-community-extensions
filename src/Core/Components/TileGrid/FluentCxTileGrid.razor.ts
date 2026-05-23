import { DotNet } from "../../d-ts/Microsoft.JSInterop";

export namespace FluentUI.Blazor.Community.TileGrid {
  interface TileGridInstance {
    id: string,
    dotNetHelper: DotNet.DotNetObject,
    resizeObserver: ResizeObserver,
  }

  const _tileGridComponents = [] as TileGridInstance[];

  function computeVisualRows(
    rowHeights: number[],
    items: { row: number; rowSpan: number }[],
    rowGap: number
  ): number[] {

    const spans: number[] = new Array(rowHeights.length).fill(1);

    for (const item of items) {
      const start = item.row;
      const span = item.rowSpan;
      spans[start] = Math.max(spans[start], span);
    }

    const visualRows: number[] = [];
    let r = 0;

    while (r < rowHeights.length) {
      const span = spans[r];
      let height = 0;

      for (let i = 0; i < span; i++) {
        height += rowHeights[r + i] ?? 0;
      }

      height += rowGap;
      visualRows.push(height);
      r += span;
    }

    return visualRows;
  }


  function getGridRowHeights(grid: HTMLElement): number[]
  {
    const computed = getComputedStyle(grid);
    const rows = computed.getPropertyValue("grid-template-rows").trim().split(" ").map(x => parseFloat(x));

    return rows;
  }

  function computeGridData(element: HTMLElement) {
    const computed = getComputedStyle(element);
    const templateCols = computed.getPropertyValue("grid-template-columns");
    const colCount = templateCols.trim().split(" ").length;
    const colGap = parseFloat(computed.getPropertyValue("column-gap")) || 0;
    const rowGap = parseFloat(computed.getPropertyValue("row-gap")) || 0;
    const rect = element.getBoundingClientRect();
    const gridWidth = rect.width;
    const totalGap = colGap * (colCount - 1);
    const cellWidth = (gridWidth - totalGap) / colCount;

    const firstItem = element.querySelector(".tile-grid-item") as HTMLElement | null;
    let cellHeight = 0;

    if (firstItem)
    {
      const itemRect = firstItem.getBoundingClientRect();
      const rowSpan = parseInt(firstItem.style.gridRowEnd.replace("span ", "")) || 1;

      cellHeight = (itemRect.height - (rowGap * (rowSpan - 1))) / rowSpan;
    }

    const rowHeights = getGridRowHeights(element);
    const items = Array.from(element.querySelectorAll(".tile-grid-item-container"))
      .map(el => (
        {
          row: parseInt(getComputedStyle(el).getPropertyValue("grid-row-start"), 10) - 1,
          rowSpan: parseInt(getComputedStyle(el).getPropertyValue("grid-row-end").replace("span ", ""), 10)
        }));

    const visualRows = computeVisualRows(rowHeights, items, rowGap);

    return {
      columnCount: colCount,
      cellWidth: cellWidth,
      cellHeight: cellHeight,
      rowHeights: visualRows      
    };
  }

  export function Initialize(id: string, dotNetHelper: DotNet.DotNetObject): void {
    const element = document.getElementById(id) as HTMLElement;
    if (!element) {
      return;
    }

    const tileGrid = element.getElementsByClassName("fluentcx-tile-grid")[0] as HTMLElement;

    const resizeObserver = new ResizeObserver(() => {
      const data = computeGridData(tileGrid);
      dotNetHelper.invokeMethodAsync("OnGridComputed", data);
    });

    resizeObserver.observe(tileGrid);

    const initialData = computeGridData(tileGrid);
    dotNetHelper.invokeMethodAsync("OnGridComputed", initialData);

    _tileGridComponents.push({
      id,
      dotNetHelper,
      resizeObserver
    });
  }

  export function Dispose(id: string): void {
    const index = _tileGridComponents.findIndex(t => t.id === id);
    if (index !== -1) {
      const instance = _tileGridComponents[index];
      instance.resizeObserver.disconnect();
      _tileGridComponents.splice(index, 1);
    }
  }
}
