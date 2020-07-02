using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Tables.Borders;

namespace Sidea.DocxToPdf.Models.Tables.Grids
{
    internal class Grid
    {
        private readonly double[] _columnWidths;
        private readonly GridRow[] _gridRows;
        private readonly List<PageContext> _pageContexts = new List<PageContext>();

        public Grid(
            IEnumerable<double> columnWidths,
            IEnumerable<GridRow> rowHeights)
        {
            _columnWidths = columnWidths.ToArray();
            _gridRows = rowHeights.ToArray();
        }

        public Func<PageNumber, PageContext> PageFactory { get; set; }

        public int ColumnCount => _columnWidths.Length;

        public int RowCount => _gridRows.Length;

        public void ResetPageContexts(PageContext startOn)
        {
            _pageContexts.Clear();
            _pageContexts.Add(startOn);
        }

        public void RegisterPageContext(PageContext pageContext)
        {
            if (_pageContexts.Any(pg => pg.PageNumber == pageContext.PageNumber))
            {
                return;
            }

            _pageContexts.Add(pageContext);
        }

        public PageContext CreatePageContextForCell(GridPosition position)
        {
            var rowPageContext = this.CreateRowPageContext(position);
            var horizontalSpace = this.CalculateCellSpace(position);

            return rowPageContext.Crop(horizontalSpace);
        }

        public PageContext CreatePageContextForCell(GridPosition position, PageNumber onPage)
        {
            var rowPageContext = this.CreateRowPageContext(position);
            while (rowPageContext.PageNumber != onPage)
            {
                rowPageContext = this.GetOrCreatePageContext(rowPageContext.PageNumber.Next());
            }

            var horizontalSpace = this.CalculateCellSpace(position);
            return rowPageContext.Crop(horizontalSpace);
        }

        public void JustifyGridRows(GridPosition position, IReadOnlyCollection<PageRegion> pageRegions)
        {
            var totalHeightOfCell = pageRegions.Sum(pr => pr.Region.Height);

            if (position.RowSpan == 1)
            {
                _gridRows[position.Row].Expand(totalHeightOfCell);
                return;
            }

            var affectedRows = this.GetRowsInPosition(position)
                .ToArray();

            var rowsSum = affectedRows.Sum(r => r.Height);
            if (rowsSum > totalHeightOfCell)
            {
                return;
            }

            var distribution = Distribute(affectedRows.Select(r => r.Height).ToArray(), totalHeightOfCell - rowsSum);
            for (var i = 0; i < distribution.Length; i++)
            {
                affectedRows[i].Expand(distribution[i]);
            }
        }

        private HorizontalSpace CalculateCellSpace(GridPosition position)
        {
            var offset = _columnWidths
               .Take(position.Column)
               .Aggregate(0d, (col, acc) => acc + col);

            var width = _columnWidths
              .Skip(position.Column)
              .Take(position.ColumnSpan)
              .Aggregate(0.0, (col, acc) => acc + col);

            return new HorizontalSpace(offset, width);
        }

        public HorizontalSpace CalculateAbsoluteCellSpace(GridPosition position)
        {
            var space = this.CalculateCellSpace(position);
            var baseX = _pageContexts.FirstOrDefault()?.Region.X ?? 0.0;

            return new HorizontalSpace(baseX + space.X, space.Width);
        }

        public IEnumerable<VerticalSpace> CalcualteAbsouluteVerticalSpaces(GridPosition position)
        {
            var rows = this
                .GetRowsInPosition(position)
                .ToArray();

            var offset = this.RowAbsoluteOffset(position.RowIndeces.ElementAt(0));
            var remainingHeight = rows.Sum(r => r.Height);

            var coordinates = new List<VerticalSpace>();
            foreach (var pg in _pageContexts)
            {
                if (pg.Region.Height < offset)
                {
                    offset -= pg.Region.Height;
                    continue;
                }

                var regionHeight = pg.Region.Height - offset;
                var h = Math.Min(regionHeight, remainingHeight);
                coordinates.Add(new VerticalSpace(pg.PageNumber, pg.Region.Y + offset, h));

                offset = 0;
                remainingHeight -= h;

                if (remainingHeight == 0)
                {
                    break;
                }
            }

            return coordinates;
        }

        private double RowAbsoluteOffset(GridPosition position)
            => this.RowAbsoluteOffset(position.Row);

        private double RowAbsoluteOffset(int rowIndex)
                => _gridRows
                    .Take(rowIndex)
                    .Sum(gr => gr.Height);

        private PageContext CreateRowPageContext(GridPosition position)
        {
            var rowOffset = this.RowAbsoluteOffset(position);

            var pc = _pageContexts.First();
            do
            {
                if (pc.Region.Height > rowOffset)
                {
                    return pc.Crop(rowOffset, 0, 0, 0);
                }

                rowOffset -= pc.Region.Height;
                pc = this.GetOrCreatePageContext(pc.PageNumber.Next());
            } while (true);
        }

        private PageContext GetOrCreatePageContext(PageNumber fromPageNumber)
        {
            var pageContext = _pageContexts.FirstOrDefault(pg => pg.PageNumber == fromPageNumber);
            if (pageContext != null)
            {
                return pageContext;
            }

            pageContext = this.PageFactory(fromPageNumber);
            _pageContexts.Add(pageContext);
            return pageContext;
        }

        private static double[] Distribute(IReadOnlyCollection<double> currentValues, double totalValueToDistribute)
        {
            if (totalValueToDistribute <= 0)
            {
                return currentValues.ToArray();
            }

            var perItem = totalValueToDistribute / currentValues.Count;
            var copy = currentValues
                .Select((v, i) =>
                {
                    var newValue = i < currentValues.Count - 1
                        ? v + perItem
                        : v + (totalValueToDistribute - (i * perItem));

                    return newValue;
                });

            return copy.ToArray();
        }

        private IEnumerable<GridRow> GetRowsInPosition(GridPosition position)
        {
            return _gridRows
                .Skip(position.Row)
                .Take(position.RowSpan);
        }

       
    }
}
