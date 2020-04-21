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
        private readonly XPen _cellBorderPen = new XPen(XPens.Black)
        {
            Width = XUnit.FromPoint(0.5d)
        };

        private readonly RGrid _grid;
        private readonly RCell[] _orderedCells = new RCell[0];
        private XUnit _alreadyRendered = XUnit.Zero;
        private XUnit[] _rowHeights = new XUnit[0];

        public RLayout(RGrid grid, IEnumerable<RCell> cells)
        {
            _grid = grid;
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

            _rowHeights = this.CalculateRowHeights();
            var totalWidth = _orderedCells
                .Where(c => c.GridPosition.Row == 0)
                .Aggregate(0d, (agg, c) => agg + c.PrecalulatedSize.Width);

            return new XSize(totalWidth, _rowHeights.Sum());
        }

        protected override sealed RenderingState RenderCore(IRenderArea renderArea)
        {
            this.RenderCells(renderArea);

            var unfinished = this.GetCellsToRenderer();
            if(unfinished.Any())
            {
                _alreadyRendered += renderArea.AreaRectangle.Height;
                return RenderingState.EndOfRenderArea(renderArea.AreaRectangle);
            }

            return RenderingState.Done(new XRect(new XSize(this.PrecalulatedSize.Width, this.PrecalulatedSize.Height - _alreadyRendered)));
        }

        private IEnumerable<RCell> GetCellsToRenderer()
        {
            return _orderedCells.Where(c => c.CurrentRenderingState.Status.IsNotFinished());
        }

        private void RenderCells(IRenderArea renderArea)
        {
            var infos = this.GetCellsToRenderer()
                .Select(cell => this.RenderCell(cell, renderArea))
                .ToArray();

            foreach (var info in infos.Where(i => i.WasRendered))
            {
                this.RenderCellBorders(info, renderArea);
            }
        }

        private CellRenderInfo RenderCell(RCell cell, IRenderArea renderArea)
        {
            var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cell, true);
            if (topOffset > renderArea.Height)
            {
                return CellRenderInfo.NotRendered(cell);
            }

            // var rowSpan = this.CellRowSpan(cell);

            // total height for cell defined by grid - height already rendered
            // var minimalCellBoxHeight = this.CellRowsHeightSum(cell);
            //     - cell.RenderedSize.Height;

            var cellArea = renderArea
                .PanLeftDown(new XSize(leftOffset, topOffset))
                .Restrict(cell.PrecalulatedSize.Width);

            var renderTopBorder = cell.CurrentRenderingState.Status == RenderingStatus.NotStarted
                && cell.GridPosition.RowSpan != 0;

            // this.RenderCellTopBorder(cell, cellArea);

            var previousRenderedHeight = cell.RenderedSize.Height;

            cell.Render(cellArea);
            this.JustifyRowHeights(cell);

            var renderBottomBorder = cell.CurrentRenderingState.Status == RenderingStatus.Done;

            // this.RenderCellSideAndBottomBorders(cell, cellArea, minimalCellBoxHeight);

            return CellRenderInfo.Rendered(cell, previousRenderedHeight, renderTopBorder, renderBottomBorder);
        }

        private void RenderCellBorders(CellRenderInfo cellInfo, IRenderArea renderArea)
        {
            var (leftOffset, topOffset) = this.CalculateCellLayoutOffset(cellInfo.Cell, false);

            // total height for cell defined by grid - height already rendered
            var minimalCellBoxHeight = this.CellRowsHeightSum(cellInfo.Cell)
                 - cellInfo.PreviousRenderedHeight;

            var cellRenderArea = renderArea
                .PanLeftDown(new XSize(leftOffset, topOffset))
                .Restrict(cellInfo.Cell.PrecalulatedSize.Width);

            if (cellInfo.RenderTopBorder)
            {
                cellRenderArea.DrawLine(_cellBorderPen, new XPoint(0, 0), new XPoint(cellRenderArea.AreaRectangle.Width, 0));
            }

            this.RenderCellSideAndBottomBorders(cellInfo.Cell, cellRenderArea, minimalCellBoxHeight);
        }

        //private void RenderCellTopBorder(RCell cell, IRenderArea cellRenderArea)
        //{
        //    if (cell.CurrentRenderingState.Status != RenderingStatus.NotStarted && cell.GridPosition.RowSpan == 0)
        //    {
        //        return;
        //    }

        //    cellRenderArea.DrawLine(_cellBorderPen, new XPoint(0, 0), new XPoint(cellRenderArea.AreaRectangle.Width, 0));
        //}

        private void RenderCellSideAndBottomBorders(RCell cell, IRenderArea cellRenderArea, XUnit minimalCellBoxHeight)
        {
            var height = Math.Min(
                Math.Max(cell.CurrentRenderingState.RenderedArea.Height, minimalCellBoxHeight),
                cellRenderArea.Height);
            
            var rect = new XRect(0, 0, cellRenderArea.AreaRectangle.Width, height);

            cellRenderArea.DrawLine(_cellBorderPen, rect.TopRight, rect.BottomRight);
            cellRenderArea.DrawLine(_cellBorderPen, rect.BottomLeft, rect.TopLeft);
            if (cell.CurrentRenderingState.Status == RenderingStatus.Done)
            {
                cellRenderArea.DrawLine(_cellBorderPen, rect.BottomRight, rect.BottomLeft);
            }
        }

        //private void RenderCellBorder(RCell cell, IRenderArea cellRenderArea, XUnit totalCellHeight)
        //{
        //    var height = Math.Min(totalCellHeight - cell.CurrentRenderingState.RenderedArea.Height, cellRenderArea.Height);
        //    var rect = new XRect(0,0, cellRenderArea.AreaRectangle.Width, height);

        //    cellRenderArea.DrawLine(_cellBorderPen, rect.TopRight, rect.BottomRight);

        //    if(totalCellHeight - cell.CurrentRenderingState.RenderedArea.Height <= cellRenderArea.Height)
        //    {
        //        cellRenderArea.DrawLine(_cellBorderPen, rect.BottomRight, rect.BottomLeft);
        //    }

        //    cellRenderArea.DrawLine(_cellBorderPen, rect.BottomLeft, rect.TopLeft);
        //}

        private (XUnit leftOffset, XUnit topOffset) CalculateCellLayoutOffset(RCell cell, bool onCellRender)
        {
            var renderedPartOfCellOffset = onCellRender
                ? cell.RenderedSize.Height
                : cell.RenderedSize.Height - cell.CurrentRenderingState.RenderedArea.Height;

            var leftOffset = _grid.CalculateLeftOffset(cell.GridPosition);
            var topOffset = _rowHeights
                .Take(cell.GridPosition.Row)
                .Sum()
                - _alreadyRendered
                + renderedPartOfCellOffset;

            return (leftOffset, topOffset);
        }

        private XUnit[] CalculateRowHeights()
        {
            var rowHeights = Enumerable
                .Range(0, _grid.RowsCount)
                .Select(i => _grid.RowHeight(i))
                .ToArray();

            foreach(var cell in _orderedCells)
            {
                var cellHeight = new XUnit(cell.PrecalulatedSize.Height);
                var cellRowIndeces = this.RowIndecesOfCell(cell);
                var cellRows = rowHeights
                    .SelectWithIndeces(cellRowIndeces)
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
            if(cell.CurrentRenderingState.Status != RenderingStatus.Done)
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
                .TakeWhile(c => c == cell || c.GridPosition.RowSpan == 0)
                .Count();

            var sum = _rowHeights
               .Skip(cell.GridPosition.Row)
               .Take(rowSpan)
               .Sum();

            return sum;
        }

        private int[] RowIndecesOfCell(RCell cell)
        {
            var x = _orderedCells
                .SkipWhile(c => c != cell) // c.GridPosition.Row < cell.GridPosition.Row && c.GridPosition.Column < cell.GridPosition.Column)
                .Where(c => c.GridPosition.Column == cell.GridPosition.Column)
                .TakeWhile(c => c == cell || c.GridPosition.RowSpan == 0)
                .Select(c => c.GridPosition.Row)
                .ToArray();

            return x;
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

        private class CellRenderInfo
        {
            private CellRenderInfo(
                RCell cell,
                XUnit previousRenderedHeight,
                bool wasRendered,
                bool renderTopBorder,
                bool renderBottomBorder)
            {
                this.Cell = cell;
                this.PreviousRenderedHeight = previousRenderedHeight;
                this.WasRendered = wasRendered;
                this.RenderTopBorder = renderTopBorder;
                this.RenderBottomBorder = renderBottomBorder;
            }

            public RCell Cell { get; }
            public XUnit PreviousRenderedHeight { get; }
            public bool WasRendered { get; }
            public bool RenderTopBorder { get; }
            public bool RenderBottomBorder { get; }

            public static CellRenderInfo Rendered(RCell cell, XUnit previousRenderedHeight, bool topBorder, bool bottomBorder)
            {
                return new CellRenderInfo(cell, previousRenderedHeight, true, topBorder, bottomBorder);
            }

            public static CellRenderInfo NotRendered(RCell cell)
            {
                return new CellRenderInfo(cell, XUnit.Zero, false, false, false);
            }
        }
    }
}
