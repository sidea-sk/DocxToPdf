using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RLayout
    {
        private readonly RGrid _grid;
        private readonly RCell[] _orderedCells = new RCell[0];

        private XUnit[] _rowHeights = new XUnit[0];

        public RLayout(RGrid grid, IEnumerable<RCell> cells)
        {
            _grid = grid;
            _orderedCells = cells
                .OrderBy(c => c.GridPosition.Row)
                .ThenBy(c => c.GridPosition.Column)
                .ToArray();

            this.PrepareData();
        }

        public XSize TotalSize { get; private set; } = new XSize(0, 0);

        public void Render(IRenderArea renderArea)
        {
            for (var rowIndex = 0; rowIndex < _grid.RowsCount; rowIndex++)
            {
                foreach (var cell in this.CellsInRow(rowIndex))
                {
                    if (cell.TotalArea.Height == 0)
                    {
                        // merge continuation, layout for upper cell is rendered
                        continue;
                    }

                    this.RenderCell(cell, renderArea);
                }
            }
        }

        private void PrepareData()
        {
            _rowHeights = this.CalculateRowHeights();
            var totalWidth = _orderedCells
                .Where(c => c.GridPosition.Row == 0)
                .Aggregate(0d, (agg, c) => agg + c.TotalArea.Width);

            this.TotalSize = this.TotalSize.Expand(totalWidth, _rowHeights.Sum());
        }

        private IEnumerable<RCell> CellsInRow(int rowIndex)
            => _orderedCells.Where(c => c.GridPosition.Row == rowIndex && c.GridPosition.RowSpan > 0);

        private void RenderCell(RCell cell, IRenderArea renderArea)
        {
            var leftOffset = _grid.CalculateLeftOffset(cell.GridPosition);
            var topOffset = _rowHeights
                .Take(cell.GridPosition.Row)
                .Sum();

            var rowSpan = _orderedCells
                .Where(c => c.GridPosition.Column == cell.GridPosition.Column && c.GridPosition.Row >= cell.GridPosition.Row)
                .TakeWhile(c => c == cell || c.GridPosition.RowSpan == 0)
                .Count();

            var height = _rowHeights
                .Skip(cell.GridPosition.Row)
                .Take(rowSpan)
                .Sum();

            var rect = new XRect(leftOffset, topOffset, cell.TotalArea.Width, height);
            var pen = new XPen(XPens.Black);
            pen.Width = XUnit.FromPoint(0.5d);

            renderArea.DrawLine(pen, rect.TopLeft, rect.TopRight);
            renderArea.DrawLine(pen, rect.TopRight, rect.BottomRight);
            renderArea.DrawLine(pen, rect.BottomRight, rect.BottomLeft);
            renderArea.DrawLine(pen, rect.BottomLeft, rect.TopLeft);

            var cellArea = renderArea
                .PanLeftDown(new XSize(leftOffset, topOffset))
                .Restrict(cell.TotalArea.Width);

            cell.Render(cellArea);
        }

        private XUnit[] CalculateRowHeights()
        {
            var rowHeights = Enumerable
                .Range(0, _grid.RowsCount)
                .Select(i => _grid.RowHeight(i))
                .ToArray();

            foreach(var cell in _orderedCells)
            {
                var cellHeight = new XUnit(cell.TotalArea.Height);
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
    }
}
