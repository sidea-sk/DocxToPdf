using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Tables.Elements;
using Sidea.DocxToPdf.Models.Tables.Grids;

namespace Sidea.DocxToPdf.Models.Tables.Grids
{
    internal class GridBorder
    {
        private readonly TableBorderStyle _tableBorderStyle;
        private readonly Grid _grid;

        public GridBorder(
            TableBorderStyle tableBorderStyle,
            Grid grid)
        {
            _tableBorderStyle = tableBorderStyle;
            _grid = grid;
        }

        public void Render(IEnumerable<Cell> cells, IRenderer renderer)
        {
            foreach(var cell in cells)
            {
                var border = _grid.GetBorder(cell.GridPosition);
                this.RenderBorders(renderer, cell.GridPosition, cell.BorderStyle, border);
            }
        }

        private void RenderBorders(
            IRenderer renderer,
            GridPosition gridPosition,
            BorderStyle borderStyle,
            CellBorder borders)
        {
            var topPen = this.TopPen(borderStyle, gridPosition);
            this.RenderBorderLine(renderer, borders.Top, topPen);

            var bottomPen = this.BottomPen(borderStyle, gridPosition);
            this.RenderBorderLine(renderer, borders.Bottom, bottomPen);

            var leftPen = this.LeftPen(borderStyle, gridPosition);
            foreach(var lb in borders.Left)
            {
                this.RenderBorderLine(renderer, lb, leftPen);
            }

            var rightPen = this.RightPen(borderStyle, gridPosition);
            foreach (var rb in borders.Right)
            {
                this.RenderBorderLine(renderer, rb, rightPen);
            }
        }

        private void RenderBorderLine(
            IRenderer renderer,
            BorderLine borderLine,
            Pen pen)
        {
            var page = renderer.GetPage(borderLine.PageNumber);
            var line = borderLine.ToLine(pen);
            page.RenderLine(line);
        }

        private Pen TopPen(BorderStyle border, GridPosition position)
            => border.Top ?? this.DefaultTopPen(position);

        private Pen LeftPen(BorderStyle border, GridPosition position)
            => border.Left ?? this.DefaultLeftPen(position);

        private Pen RightPen(BorderStyle border, GridPosition position)
            => border.Right ?? this.DefaultRightPen(position);

        private Pen BottomPen(BorderStyle border, GridPosition position)
            => border.Bottom ?? this.DefaultBottomPen(position);

        private Pen DefaultTopPen(GridPosition position)
        {
            return position.Row == 0
                ? _tableBorderStyle.Top
                : _tableBorderStyle.InsideHorizontal;
        }

        private Pen DefaultLeftPen(GridPosition position)
        {
            return position.Column == 0
                ? _tableBorderStyle.Left
                : _tableBorderStyle.InsideVertical;
        }

        private Pen DefaultRightPen(GridPosition position)
        {
            return position.Column + position.ColumnSpan == _grid.ColumnCount
                ? _tableBorderStyle.Right
                : _tableBorderStyle.InsideVertical;
        }

        private Pen DefaultBottomPen(GridPosition position)
        {
            return position.Row + position.RowSpan == _grid.RowCount
                ? _tableBorderStyle.Bottom
                : _tableBorderStyle.InsideHorizontal;
        }
    }
}
