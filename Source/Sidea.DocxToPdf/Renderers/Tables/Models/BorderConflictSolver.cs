using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class BorderConflictSolver
    {
        private readonly RGrid _grid;
        private readonly TableBorderStyle _tableBorder;

        public BorderConflictSolver(
            RGrid grid,
            TableBorderStyle tableBorder)
        {
            _grid = grid;
            _tableBorder = tableBorder;
        }

        public XPen GetTopBorderPen(RCell cell)
        {
            return cell.GridPosition.Row == 0
                ? _tableBorder.Top
                : _tableBorder.InsideHorizontal;
        }

        public XPen GetRightBorderPen(RCell cell)
        {
            return cell.GridPosition.Column + cell.GridPosition.ColumnSpan == _grid.ColumnsCount
                ? _tableBorder.Right
                : _tableBorder.InsideVertical;
        }

        public XPen GetBottomBorderPen(RCell cell)
        {
            return cell.GridPosition.Row + cell.GridPosition.RowSpan == _grid.RowsCount
                ? _tableBorder.Bottom
                : _tableBorder.InsideHorizontal;
        }

        public XPen GetLeftBorderPen(RCell cell)
        {
            return cell.GridPosition.Column == 0
                ? _tableBorder.Left
                : _tableBorder.InsideVertical;
        }
    }
}
