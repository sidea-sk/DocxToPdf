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
        private readonly List<PageContext> _pageContexts = new List<PageContext>();

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

        public Func<PageNumber, PageContext> PageFactory { get; set; }

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

        public void ResetPageContexts(PageContext startOn)
        {
            _pageContexts.Clear();
            _pageContexts.Add(startOn);
        }

        public void RegisterPageContext(PageContext pageContext)
        {
            if(_pageContexts.Any(pg => pg.PageNumber == pageContext.PageNumber))
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

            _rowHeights[position.Row] = Math.Max(totalHeightOfCell, _rowHeights[position.Row]);
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
    }
}
