using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Tables.Borders;
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

        public override void Prepare(
            PageContext pageContext,
            Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            _grid.ResetPageContexts(pageContext);
            _grid.PageFactory = (PageNumber pn) => pageFactory(pn, this);

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement)
                => _grid.CreatePageContextForCell(((Cell)childElement).GridPosition, pageNumber);

            Rectangle availableRegion = pageContext.Region;
            foreach (var cell in this.PreparationOrderedCells)
            {
                var cellPageContext = _grid.CreatePageContextForCell(cell.GridPosition);
                cell.Prepare(cellPageContext, onNewPage);

                _grid.JustifyGridRows(cell.GridPosition, cell.PageRegions);
            }

            _grid.PageFactory = null;
            this.ResetPageRegionsFrom(_cells);
        }

        public override void Prepare(PageContext pageContext, Func<PagePosition, ContainerElement, PageContext> nextPageContextFactory)
        {
            throw new NotImplementedException();
        }

        public override void Render(IRenderer renderer)
        {
            _cells.Render(renderer);
            new Border(_tableBorder, _grid).Render(_cells, renderer);
        }
    }
}
