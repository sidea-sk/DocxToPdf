using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Tables.Elements;
using Sidea.DocxToPdf.Models.Tables.Grids;

namespace Sidea.DocxToPdf.Models.Tables
{
    internal class Table : ContainerElement
    {
        private readonly Cell[] _cells = new Cell[0];
        private readonly Grid _grid;
        private readonly TableBorderStyle _tableBorder;

        public Table(IEnumerable<Cell> cells, Grid grid, TableBorderStyle tableBorder)
        {
            _cells = cells.ToArray();
            _grid = grid;
            _tableBorder = tableBorder;
        }

        private IEnumerable<Cell> PreparationOrderedCells => _cells
            .OrderBy(c => c.GridPosition.RowSpan) // start with smalles cells
            .ThenBy(c => c.GridPosition.Row)      // from top
            .ThenBy(c => c.GridPosition.Column);  // from left

        public override void Prepare(PageContext pageContext, Func<PagePosition, ContainerElement, PageContext> nextPageContextFactory)
        {
            _grid.ResetPageContexts(pageContext);
            _grid.PageContextFactory = (PagePosition currentPagePosition) => nextPageContextFactory(currentPagePosition, this);

            Func<PagePosition, ContainerElement, PageContext> onNextPageContext = (currentPagePosition, childElement)
                => _grid.CreateNextPageContextForCell(currentPagePosition, ((Cell)childElement).GridPosition);

            Rectangle availableRegion = pageContext.Region;
            foreach (var cell in this.PreparationOrderedCells)
            {
                var cellPageContext = _grid.CreateStartPageContextForCell(cell.GridPosition);
                cell.Prepare(cellPageContext, onNextPageContext);

                _grid.JustifyGridRows(cell.GridPosition, cell.PageRegions);
            }

            _grid.PageContextFactory = null;
            this.ResetPageRegions(_grid.GetPageRegions());
        }

        public override void Render(IRenderer renderer)
        {
            _cells.Render(renderer);
            new GridBorder(_tableBorder, _grid).Render(_cells, renderer);
        }
    }
}
