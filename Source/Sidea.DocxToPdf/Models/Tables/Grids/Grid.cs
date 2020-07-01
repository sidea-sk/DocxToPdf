using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class Grid
    {
        private readonly double[] _columnWidths;
        private readonly GridRow[] _gridRows;

        public Grid(
            IEnumerable<double> columnWidths,
            IEnumerable<GridRow> rowHeights)
        {
            _columnWidths = columnWidths.ToArray();
            _gridRows = rowHeights.ToArray();

            var w = _columnWidths.Sum();
            var h = _gridRows.Select(r => r.Height).Sum();

            this.Size = new Size(w, h);
        }

        public Size Size { get; }

        public int ColumnsCount => _columnWidths.Length;

        public int RowsCount => _gridRows.Length;

        public HorizontalSpace CalculateCellSpace(GridPosition gridPosition)
        {
            var offset = this.CalculateLeftOffset(gridPosition);
            var width = this.CalculateWidth(gridPosition);
            return new HorizontalSpace(offset, width);
        }

        public double CalculateLeftOffset(GridPosition position)
        {
            var width = _columnWidths
               .Take(position.Column)
               .Aggregate(0d, (col, acc) => acc + col);

            return width; 
        }

        public double CalculateWidth(GridPosition position)
        {
            var width = _columnWidths
               .Skip(position.Column)
               .Take(position.ColumnSpan)
               .Aggregate(0.0, (col, acc) => acc + col);

            return width;
        }

        public double RowHeight(int rowIndex) => _gridRows[rowIndex].Height;
    }
}
