using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Tables.Elements;
using Sidea.DocxToPdf.Models.Tables.Grids;

namespace Sidea.DocxToPdf.Models.Tables.Borders
{
    internal class Border
    {
        private readonly TableBorderStyle _tableBorderStyle;
        private readonly Grid _grid;

        public Border(
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
                var horizontalSpace = _grid.CalculateAbsoluteCellSpace(cell.GridPosition);
                var verticalSpaces = _grid.CalcualteAbsouluteVerticalSpaces(cell.GridPosition).ToArray();

                this.RenderBorders(renderer, cell.BorderStyle, cell.GridPosition, horizontalSpace, verticalSpaces);
            }
        }

        private void RenderBorders(
            IRenderer renderer,
            BorderStyle cellBorder,
            GridPosition gridPosition,
            HorizontalSpace horizontalSpace,
            VerticalSpace[] verticalSpaces)
        {
            this.RenderTopLine(renderer, this.TopPen(cellBorder, gridPosition), horizontalSpace, verticalSpaces.First());

            foreach(var verticalSpace in verticalSpaces)
            {
                this.RenderSideLines(renderer, this.LeftPen(cellBorder, gridPosition), this.RightPen(cellBorder, gridPosition), horizontalSpace, verticalSpace);
            }

            this.RenderBottomLine(renderer, this.BottomPen(cellBorder, gridPosition), horizontalSpace, verticalSpaces.Last());
        }

        private void RenderTopLine(
            IRenderer renderer,
            Pen pen,
            HorizontalSpace horizontalSpace,
            VerticalSpace verticalSpace)
        {
            var topLine = new Line(
                new Core.Point(horizontalSpace.X, verticalSpace.Y),
                new Core.Point(horizontalSpace.RightX, verticalSpace.Y),
                pen);

            var page = renderer.Get(verticalSpace.PageNumber);
            page.RenderLine(topLine);
        }

        private void RenderBottomLine(
            IRenderer renderer,
            Pen pen,
            HorizontalSpace horizontalSpace,
            VerticalSpace verticalSpace)
        {
            var topLine = new Line(
                new Core.Point(horizontalSpace.X, verticalSpace.BottomY),
                new Core.Point(horizontalSpace.RightX, verticalSpace.BottomY),
                pen);

            var page = renderer.Get(verticalSpace.PageNumber);
            page.RenderLine(topLine);
        }

        private void RenderSideLines(
            IRenderer renderer,
            Pen leftPen,
            Pen rightPen,
            HorizontalSpace horizontalSpace,
            VerticalSpace verticalSpace)
        {
            var leftLine = new Line(
                new Core.Point(horizontalSpace.X, verticalSpace.Y),
                new Core.Point(horizontalSpace.X, verticalSpace.BottomY),
                leftPen);

            var rightLine = new Line(
                new Core.Point(horizontalSpace.RightX, verticalSpace.Y),
                new Core.Point(horizontalSpace.RightX, verticalSpace.BottomY),
                rightPen);

            var page = renderer.Get(verticalSpace.PageNumber);
            page.RenderLine(leftLine);
            page.RenderLine(rightLine);
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
