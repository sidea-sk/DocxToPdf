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
            return cell.Border.Top ?? this.DefaultTopBorderPen(cell.GridPosition);
        }

        public XPen GetRightBorderPen(RCell cell)
        {
            return cell.Border.Right ?? this.DefaultRightBorderPen(cell.GridPosition);
        }

        public XPen GetBottomBorderPen(RCell cell)
        {
            return cell.Border.Bottom ?? this.DefaultBottomBorderPen(cell.GridPosition);
        }

        public XPen GetLeftBorderPen(RCell cell)
        {
            return cell.Border.Left ?? this.DefaultLeftBorderPen(cell.GridPosition);
        }

        private XPen DefaultTopBorderPen(GridPosition gridPosition)
        {
            return gridPosition.Row == 0
                ? _tableBorder.Top
                : _tableBorder.InsideHorizontal;
        }

        private XPen DefaultRightBorderPen(GridPosition gridPosition)
        {
            return gridPosition.Column + gridPosition.ColumnSpan == _grid.ColumnsCount
               ? _tableBorder.Right
               : _tableBorder.InsideVertical;
        }

        private XPen DefaultBottomBorderPen(GridPosition gridPosition)
        {
            return gridPosition.Row + gridPosition.RowSpan == _grid.RowsCount
                ? _tableBorder.Bottom
                : _tableBorder.InsideHorizontal;
        }

        private XPen DefaultLeftBorderPen(GridPosition gridPosition)
        {
            return gridPosition.Column == 0
               ? _tableBorder.Left
               : _tableBorder.InsideVertical;
        }
    }
}
