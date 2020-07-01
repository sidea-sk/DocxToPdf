﻿using System;
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

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var currentPageContext = pageContext;

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement)
                => this.OnCellNewPage(pageNumber, (Cell)childElement, pageFactory);

            Rectangle availableRegion = pageContext.Region;
            foreach (var cell in _cells)
            {
                var cellPageContext = this.CreatePageContextForCell(currentPageContext, cell);
                cell.Prepare(cellPageContext, onNewPage);
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
            return pageFactory(pageNumber, this);
        }

        private PageContext CreatePageContextForCell(PageContext fromPageContext, Cell cell)
        {
            var previousCellPageRegion = this.FindTopPreviousCell(cell.GridPosition);
            var horizontalSpace = _grid.CalculateCellSpace(cell.GridPosition);

            var xOffset = fromPageContext.Region.X + horizontalSpace.X;
            var yOffset = fromPageContext.PageNumber == previousCellPageRegion.PageNumber
                ? previousCellPageRegion.Region.Y + previousCellPageRegion.Region.Height
                : fromPageContext.Region.Y;

            var cellPageContext = fromPageContext
                    .Clip(new Point(xOffset, yOffset), horizontalSpace.Width);

            return cellPageContext;
        }

        private PageRegion FindTopPreviousCell(GridPosition toGridPosition)
        {
            var cell = _cells
                .FirstOrDefault(c => c.GridPosition.IsInColumn(toGridPosition.Column) && c.GridPosition.IsInRow(toGridPosition.Row - 1));

            return cell?.LastPageRegion ?? PageRegion.None;
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
