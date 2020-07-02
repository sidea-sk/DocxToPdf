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
        private readonly GridRow[] _gridRows;
        private readonly List<PageContext> _pageContexts = new List<PageContext>();

        public Grid(
            IEnumerable<double> columnWidths,
            IEnumerable<GridRow> rowHeights)
        {
            _columnWidths = columnWidths.ToArray();
            _gridRows = rowHeights.ToArray();
        }

        public int ColumnsCount => _columnWidths.Length;

        public int RowsCount => _gridRows.Length;

        public Func<PageNumber, PageContext> PageFactory { get; set; }

        

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

        private HorizontalSpace CalculateCellSpace(GridPosition gridPosition)
        {
            var offset = _columnWidths
               .Take(gridPosition.Column)
               .Aggregate(0d, (col, acc) => acc + col);

            var width = _columnWidths
              .Skip(gridPosition.Column)
              .Take(gridPosition.ColumnSpan)
              .Aggregate(0.0, (col, acc) => acc + col);

            return new HorizontalSpace(offset, width);
        }

        private double RowAbsoluteOffset(GridPosition position)
            => _gridRows
                    .Take(position.Row)
                    .Sum(gr => gr.Height);

        public PageContext CreatePageContextForCell(GridPosition position)
        {
            var rowPageContext = this.CreateRowPageContext(position);
            var horizontalSpace = this.CalculateCellSpace(position);

            return rowPageContext.Crop(horizontalSpace);
        }

        public PageContext CreatePageContextForCell(GridPosition position, PageNumber onPage)
        {
            var rowPageContext = this.CreateRowPageContext(position);
            while(rowPageContext.PageNumber != onPage)
            {
                rowPageContext = this.GetPageContext(rowPageContext.PageNumber.Next());
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

            var affectedRows = _gridRows
                .Skip(position.Row)
                .Take(position.RowSpan)
                .ToArray();

            var rowsSum = affectedRows.Sum(r => r.Height);
            if(rowsSum > totalHeightOfCell)
            {
                return;
            }

            var distribution = Distribute(affectedRows.Select(r => r.Height).ToArray(), totalHeightOfCell - rowsSum);
            for(var i = 0; i < distribution.Length; i++)
            {
                affectedRows[i].Expand(distribution[i]);
            }
        }

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
                pc = this.GetPageContext(pc.PageNumber.Next());
            } while (true);
        }

        private PageContext GetPageContext(PageNumber fromPageNumber)
        {
            var pageContext = _pageContexts.FirstOrDefault(pg => pg.PageNumber == fromPageNumber);
            if(pageContext != null)
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
    }
}
