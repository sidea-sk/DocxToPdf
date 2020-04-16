using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RGrid : IGridPositionService
    {
        private readonly XUnit[] _gridColumns;
        private readonly RGridRow[] _gridRows;

        public RGrid(
            IEnumerable<XUnit> gridColumns,
            IEnumerable<RGridRow> gridRows)
        {
            _gridColumns = gridColumns.ToArray();
            _gridRows = gridRows.ToArray();

            this.PredefinedHeight = _gridRows
                .Select(r => r.Height)
                .Sum();

            this.TotalWidth = _gridColumns.Sum();
        }

        public XUnit TotalWidth { get; }

        public XUnit PredefinedHeight { get; }

        public int ColumnsCount => _gridColumns.Length;

        public int RowsCount => _gridRows.Length;

        public XUnit RowHeight(int rowIndex) => _gridRows[rowIndex].Height;

        public XUnit CalculateLeftOffset(GridPosition gridPosition)
        {
            var width = _gridColumns
                .Take(gridPosition.Column)
                .Aggregate(0d, (col, acc) => acc + col);

            return width;
        }

        public XUnit CalculateWidth(GridPosition description)
        {
            var width = _gridColumns
               .Skip(description.Column)
               .Take(description.Span)
               .Aggregate(XUnit.Zero, (col, acc) => acc + col);

            return width;
        }
    }
}
