using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class RTableGrid : IGridPositionService
    {
        private readonly XUnit[] _gridColumns;

        public RTableGrid(IEnumerable<XUnit> gridColumns)
        {
            _gridColumns = gridColumns.ToArray();
        }

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
