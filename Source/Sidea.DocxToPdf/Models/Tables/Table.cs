using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;
using Sidea.DocxToPdf.Models.Tables.Builders;
using Sidea.DocxToPdf.Models.Tables.Elements;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables
{
    internal class Table : ContainerElement
    {
        private readonly Cell[] _cells = new Cell[0];
        private readonly Grid _grid;

        private Table(IEnumerable<Cell> cells, Grid grid)
        {
            _cells = cells.ToArray();
            _grid = grid;
        }

        private IEnumerable<Cell> PreparationOrderedCells => _cells
            .OrderBy(c => c.GridPosition.RowSpan) // start with smalles cells
            .ThenBy(c => c.GridPosition.Row)      // from top
            .ThenBy(c => c.GridPosition.Column);  // from left

        public override void Prepare(
            PageContext pageContext,
            Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var currentPageContext = pageContext;

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement)
                => this.OnCellNewPage(pageNumber, (Cell)childElement, pageFactory);

            Rectangle availableRegion = pageContext.Region;
            foreach (var cell in this.PreparationOrderedCells)
            {
                var cellPageContext = this.CreatePageContextForCell(currentPageContext, cell, pageFactory);
                cell.Prepare(cellPageContext, onNewPage);

                _grid.JustifyGridRows(cell.GridPosition, cell.PageRegions);
            }

            // update rowHeights
            // update cells

            this.ResetPageRegionsFrom(_cells);
        }

        public override void Render(IRenderer renderer)
        {
            _cells.Render(renderer);
        }

        private PageContext OnCellNewPage(PageNumber pageNumber, Cell cell, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var pageContext = pageFactory(pageNumber, this);
            return this.CreatePageContextForCell(pageContext, cell, pageFactory);
        }

        private PageContext CreatePageContextForCell(
            PageContext fromPageContext,
            Cell cell,
            Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var absoluteOffset = _grid.RowAbsoluteOffset(cell.GridPosition);
            var rowContext = this.GetPageForRowOffset(absoluteOffset, fromPageContext, pageFactory);

            var horizontalSpace = _grid.CalculateCellSpace(cell.GridPosition);

            return rowContext.Crop(horizontalSpace);
        }

        private PageContext GetPageForRowOffset(double rowOffset, PageContext currentPageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var remainingOffset = rowOffset;
            var ct = currentPageContext;
            do
            {
                if (ct.Region.Height > remainingOffset)
                {
                    return ct.Crop(remainingOffset, 0, 0, 0);
                }

                remainingOffset -= ct.Region.Height;
                ct = pageFactory(currentPageContext.PageNumber, this);
            } while (true);
        }

        public static Table From(Word.Table wordTable, IStyleFactory styleFactory)
        {
            var grid = wordTable.InitializeGrid();
            var cells = wordTable
                .InitializeCells(styleFactory)
                .OrderBy(c => c.GridPosition.Row)
                .ThenBy(c => c.GridPosition.Column)
                .ToArray();

            return new Table(cells, grid);
        }
    }
}
