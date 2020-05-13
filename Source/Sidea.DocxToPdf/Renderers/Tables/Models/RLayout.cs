using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RLayout : RendererBase
    {
        private readonly BorderConflictSolver _border;
        private readonly RGrid _grid;
        private readonly RCell[] _orderedCells = new RCell[0];
        private XUnit[] _rowHeights = new XUnit[0];

        public RLayout(RGrid grid, IEnumerable<RCell> cells, TableBorderStyle tableBorderStyle)
        {
            _grid = grid;
            _border = new BorderConflictSolver(grid, tableBorderStyle);
            _orderedCells = cells
                .OrderBy(c => c.GridPosition.Row)
                .ThenBy(c => c.GridPosition.Column)
                .ToArray();
        }

        protected override sealed XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            foreach(var cell in _orderedCells)
            {
                cell.CalculateContentSize(prerenderArea);
            }

            _rowHeights = this.PrecalculateRowHeights();
            var totalWidth = _orderedCells
                .Where(c => c.GridPosition.Row == 0)
                .Aggregate(0d, (agg, c) => agg + c.PrecalulatedSize.Width);

            return new XSize(totalWidth, _rowHeights.Sum());
        }

        protected override sealed RenderResult RenderCore(IRenderArea renderArea)
        {
            this.RenderCells(renderArea);

            var unfinished = this.GetCellsToRenderer();
            if(unfinished.Any())
            {
                return RenderResult.EndOfRenderArea(renderArea.AreaRectangle);
            }

            var totalHeight = _rowHeights.Sum();
            return RenderResult.Done(new XRect(new XSize(this.PrecalulatedSize.Width, totalHeight - this.RenderedSize.Height)));
        }

        private void RenderCells(IRenderArea renderArea)
        {
            var infos = this.GetCellsToRenderer()
                .Select(cell => this.RenderCell(cell, renderArea))
                .Where(i => i.WasRendered)
                .ToArray();

            var rowIndeces = infos
                .SelectMany(i => i.Cell.GridPosition.RowIndeces)
                .Distinct()
                .OrderBy(r => r)
                .ToArray();

            foreach(var rowIndex in rowIndeces)
            {
                this.RenderCellTopBorders(rowIndex, renderArea);
                this.RenderCellSideBorders(rowIndex, renderArea);
                this.RenderCellBottomBorders(rowIndex, renderArea);
            }
        }

        private CellRenderInfo RenderCell(RCell cell, IRenderArea renderArea)
        {
            var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cell, true);
            if (topOffset > renderArea.Height)
            {
                return CellRenderInfo.NotRendered(cell);
            }

            var cellArea = renderArea
                .PanLeftDown(new XSize(leftOffset, topOffset))
                .Restrict(cell.PrecalulatedSize.Width);

            cell.Render(cellArea);
            this.JustifyRowHeights(cell);

            return CellRenderInfo.Rendered(cell);
        }

        private void RenderCellTopBorders(int rowIndex, IRenderArea renderArea)
        {
            var rowOffset = this.RowOffset(rowIndex)
                - this.RenderedSize.Height;

            if (rowOffset < 0)
            {
                return;
            }

            var cells = _orderedCells
                .VerticalFirstCellsOfRow(rowIndex);

            foreach (var cell in cells)
            {
                var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cell, false);
                var cellRenderArea = renderArea.PanLeftDown(leftOffset, topOffset);
                var borderPen = _border.GetTopBorderPen(cell);
                cellRenderArea.DrawLine(borderPen, new XPoint(0, 0), new XPoint(cell.PrecalulatedSize.Width, 0));
            }
        }

        private void RenderCellSideBorders(int rowIndex, IRenderArea renderArea)
        {
            var rowOffset = this.RowOffset(rowIndex);
            var remainingHeight = _rowHeights[rowIndex];
            if (this.RenderedSize.Height > rowOffset)
            {
                remainingHeight = remainingHeight + rowOffset - this.RenderedSize.Height;
            }

            var cellsOfRow = _orderedCells
                .CellsOfRow(rowIndex)
                .ToArray();

            foreach(var cell in cellsOfRow)
            {
                var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cell, false);
                var cellRenderArea = renderArea
                    .PanLeftDown(leftOffset, topOffset);

                var height = Math.Min(remainingHeight, cellRenderArea.Height);
                var rect = new XRect(0, 0, cell.PrecalulatedSize.Width, height);

                var leftBorderPen = _border.GetLeftBorderPen(cell);
                cellRenderArea.DrawLine(leftBorderPen, rect.TopLeft, rect.BottomLeft);

                var rightBorderPen = _border.GetRightBorderPen(cell);
                cellRenderArea.DrawLine(rightBorderPen, rect.TopRight, rect.BottomRight);
            }
        }

        private void RenderCellBottomBorders(int rowIndex, IRenderArea renderArea)
        {
            var rowOffset = this.RowOffset(rowIndex);
            var remainingHeight = _rowHeights[rowIndex];
            if (this.RenderedSize.Height > rowOffset)
            {
                remainingHeight = remainingHeight + rowOffset - this.RenderedSize.Height;
            }

            var cells = _orderedCells
                .LastCellsOfRow(rowIndex);

            foreach (var cell in cells)
            {
                var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cell, false);
                var cellRenderArea = renderArea.PanLeftDown(leftOffset, topOffset);
                if (remainingHeight > cellRenderArea.Height)
                {
                    continue;
                }

                var borderPen = _border.GetBottomBorderPen(cell);
                cellRenderArea.DrawLine(borderPen, new XPoint(0, remainingHeight), new XPoint(cell.PrecalulatedSize.Width, remainingHeight));
            }
        }

        private (XUnit leftOffset, XUnit topOffset) CalculateCellLayoutOffset(RCell cell, bool onCellRender)
        {
            var renderedPartOfCellOffset = onCellRender
                ? cell.RenderedSize.Height
                : cell.RenderedSize.Height - cell.RenderResult.RenderedSize.Height;

            var leftOffset = _grid.CalculateLeftOffset(cell.GridPosition);
            var topOffset = _rowHeights
                .Take(cell.GridPosition.Row)
                .Sum()
                - this.RenderedSize.Height
                + renderedPartOfCellOffset;

            topOffset = Math.Max(0, topOffset);
            return (leftOffset, topOffset);
        }

        private XUnit RowOffset(int rowIndex)
        {
            var o = _rowHeights
                .Take(rowIndex)
                .Sum();
            return o;
        }

        private XUnit[] PrecalculateRowHeights()
        {
            var rowHeights = Enumerable
                .Range(0, _grid.RowsCount)
                .Select(i => _grid.RowHeight(i))
                .ToArray();

            foreach(var cell in _orderedCells.Where(c => c.GridPosition.RowSpan > 0).OrderBy(c => c.GridPosition.RowSpan))
            {
                var cellHeight = new XUnit(cell.PrecalulatedSize.Height);
                var cellRows = rowHeights
                        .SelectWithIndeces(cell.GridPosition.RowIndeces.ToArray())
                        .ToArray();
                var rowsSum = cellRows.Select(r => r.value).Sum();
                if (rowsSum >= cellHeight)
                {
                    continue;
                }

                var updatedRows = Distribute(cellRows, cellHeight - rowsSum);
                rowHeights = rowHeights.Update(updatedRows).ToArray();
            }

            return rowHeights;
        }

        private void JustifyRowHeights(RCell cell)
        {
            if(cell.RenderResult.Status != RenderingStatus.Done)
            {
                return;
            }

            var rowsHeightForCell = this.CellRowsHeightSum(cell);
            if(rowsHeightForCell >= cell.RenderedSize.Height)
            {
                return;
            }

            var lastRowOfCell = cell.GridPosition.Row + cell.GridPosition.RowSpan - 1;
            _rowHeights[lastRowOfCell] += cell.RenderedSize.Height - rowsHeightForCell;
        }

        private XUnit CellRowsHeightSum(RCell cell)
        {
            var rowSpan = _orderedCells
                .Where(c => c.GridPosition.Column == cell.GridPosition.Column && c.GridPosition.Row >= cell.GridPosition.Row)
                .TakeWhile(c => c == cell || c.GridPosition.IsRowMergedCell)
                .Count();

            var sum = _rowHeights
               .Skip(cell.GridPosition.Row)
               .Take(rowSpan)
               .Sum();

            return sum;
        }

        private static (XUnit value, int index)[] Distribute(IReadOnlyCollection<(XUnit value, int index)> currentValues, XUnit totalValueToDistribute)
        {
            if(totalValueToDistribute <= 0)
            {
                return currentValues.ToArray();
            }

            var perItem = totalValueToDistribute / currentValues.Count;
            var copy = currentValues
                .Select((v, i) =>
                {
                    var newValue = i < currentValues.Count - 1
                        ? new XUnit(v.value + perItem)
                        : new XUnit(v.value + (totalValueToDistribute - (i * perItem)));

                    return (newValue, v.index);
                });

            return copy.ToArray();
        }

        private IEnumerable<RCell> GetCellsToRenderer()
        {
            return _orderedCells.Where(c => c.RenderResult.Status.IsNotFinished());
        }

        private class CellRenderInfo
        {
            private CellRenderInfo(
                RCell cell,
                bool wasRendered)
            {
                this.Cell = cell;
                this.WasRendered = wasRendered;
            }

            public RCell Cell { get; }
            public bool WasRendered { get; }

            public static CellRenderInfo Rendered(RCell cell)
            {
                return new CellRenderInfo(cell, true);
            }

            public static CellRenderInfo NotRendered(RCell cell)
            {
                return new CellRenderInfo(cell, false);
            }
        }
    }
}
