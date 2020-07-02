using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class Grid
    {
        private readonly double[] _columnWidths;
        private readonly double[] _rowHeights;
        private readonly GridRow[] _gridRows;

        public Grid(
            IEnumerable<double> columnWidths,
            IEnumerable<GridRow> rowHeights)
        {
            _columnWidths = columnWidths.ToArray();
            _gridRows = rowHeights.ToArray();
            _rowHeights = rowHeights.Select(r => r.Height).ToArray();
        }

        public int ColumnsCount => _columnWidths.Length;

        public int RowsCount => _gridRows.Length;

        public HorizontalSpace CalculateCellSpace(GridPosition gridPosition)
        {
            var offset = this.CalculateLeftOffset(gridPosition);
            var width = this.CalculateWidth(gridPosition);
            return new HorizontalSpace(offset, width);
        }

        private double CalculateLeftOffset(GridPosition position)
        {
            var width = _columnWidths
               .Take(position.Column)
               .Aggregate(0d, (col, acc) => acc + col);

            return width; 
        }

        private double CalculateWidth(GridPosition position)
        {
            var width = _columnWidths
               .Skip(position.Column)
               .Take(position.ColumnSpan)
               .Aggregate(0.0, (col, acc) => acc + col);

            return width;
        }

        public double RowAbsoluteOffset(GridPosition position)
            => _rowHeights
                    .Take(position.Row)
                    .Sum();

        public void JustifyGridRows(GridPosition position, IReadOnlyCollection<PageRegion> pageRegions)
        {
            var totalHeightOfCell = pageRegions.Sum(pr => pr.Region.Height);

            _rowHeights[position.Row] = Math.Max(totalHeightOfCell, _rowHeights[position.Row]);
        }
    }
}
