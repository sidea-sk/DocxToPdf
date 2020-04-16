using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RTableLayout
    {
        private int _totalRows = 0;
        private XUnit[] _rowHeights = new XUnit[0];

        private RCell[] _cells = new RCell[0];
        //private XUnit _currentVerticalPosition = new XUnit(0);
        //private int _currentRowIndex = 0;
        private readonly IGridPositionService _gridPositionService;

        public RTableLayout(IGridPositionService gridPositionService, IEnumerable<RCell> cells)
        {
            _gridPositionService = gridPositionService;
            _cells = cells.ToArray();
            this.PrepareData();
        }

        public XSize TotalSize { get; private set; } = new XSize(0, 0);

        public void Render(IRenderArea renderArea)
        {
            for (var rowIndex = 0; rowIndex < _totalRows; rowIndex++)
            {
                foreach (var cell in this.CellsInRow(rowIndex))
                {
                    if (cell.TotalArea.Height == 0)
                    {
                        // merge continuation, layout for upper cell is rendered
                        continue;
                    }

                    this.RenderCellBorders(cell, renderArea);
                }
            }
        }

        private void PrepareData()
        {
            var totalHeight = 0d;
            for(var col = 0; col < _gridPositionService.ColumnsCount; col++)
            {
                var cellsInCol = _cells
                    .Where(c => col >= c.GridPosition.Column && col <= c.GridPosition.Column + c.GridPosition.Span - 1);

                var colHeight = cellsInCol
                    .Aggregate(0d, (agg, cell) => agg + cell.TotalArea.Height);

                totalHeight = Math.Max(totalHeight, colHeight);
            }

            var rows = _cells
                .GroupBy(c => c.GridPosition.Row, c => c)
                .ToArray();

            var totalWidth = _cells
                .Where(c => c.GridPosition.Row == 0)
                .Aggregate(0d, (agg, c) => agg + c.TotalArea.Width);

            _totalRows = rows.Count();

            var aggr = 0d;
            _rowHeights = Enumerable
                .Range(0, _totalRows)
                .Select((r, i) =>
                {
                    if (i < _totalRows - 1)
                    {
                        var rowHeight = totalHeight / _totalRows;
                        aggr += rowHeight;
                        return new XUnit(rowHeight);
                    }
                    else
                    {
                        return new XUnit(totalHeight - aggr);
                    }
                })
                .ToArray();

            this.TotalSize = this.TotalSize.Expand(totalWidth, totalHeight);
        }

        private IEnumerable<RCell> CellsInRow(int rowIndex)
            => _cells.Where(c => c.GridPosition.Row == rowIndex && c.GridPosition.RowSpan > 0);

        private void RenderCellBorders(RCell cell, IRenderArea renderArea)
        {
            var leftOffset = _gridPositionService.CalculateLeftOffset(cell.GridPosition);
            var topOffset = _rowHeights
                .Take(cell.GridPosition.Row)
                .Sum();

            var rowSpan = _cells
                .Where(c => c.GridPosition.Column == cell.GridPosition.Column && c.GridPosition.Row >= cell.GridPosition.Row)
                .TakeWhile(c => c == cell || c.GridPosition.RowSpan == 0)
                .Count();

            var height = _rowHeights
                .Skip(cell.GridPosition.Row)
                .Take(rowSpan)
                .Sum();

            var rect = new XRect(leftOffset, topOffset, cell.TotalArea.Width, height);
            renderArea.DrawLine(XPens.Black, rect.TopLeft, rect.TopRight);
            renderArea.DrawLine(XPens.Black, rect.TopRight, rect.BottomRight);
            renderArea.DrawLine(XPens.Black, rect.BottomRight, rect.BottomLeft);
            renderArea.DrawLine(XPens.Black, rect.BottomLeft, rect.TopLeft);
        }
    }
}
