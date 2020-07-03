﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;
using Sidea.DocxToPdf.Models.Tables.Builders;
using Sidea.DocxToPdf.Models.Tables.Grids;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class Cell : ContainerElement
    {
        private readonly Margin _contentMargin;
        private ContainerElement[] _childs = new ContainerElement[0];

        private Cell(IEnumerable<ContainerElement> childs, GridPosition gridPosition, BorderStyle borderStyle)
        {
            _contentMargin = new Margin(0.5, 4, 0.5, 4);
            _childs = childs.ToArray();

            this.GridPosition = gridPosition;
            this.BorderStyle = borderStyle;
        }

        public GridPosition GridPosition { get; }
        public BorderStyle BorderStyle { get; }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var currentPageContext = pageContext
                .Crop(_contentMargin.Top, _contentMargin.Right, 0, _contentMargin.Left);

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement) =>
            {
                currentPageContext = pageFactory(pageNumber, this);
                return currentPageContext.Crop(0, _contentMargin.Right, 0, _contentMargin.Left);
            };

            Rectangle availableRegion = currentPageContext.Region;

            foreach (var child in _childs)
            {
                child.Prepare(new PageContext(currentPageContext.PagePosition, availableRegion, currentPageContext.PageVariables), onNewPage);
                var lastPage = child.LastPageRegion.Region;

                availableRegion = currentPageContext
                    .Region
                    .Clip(lastPage.BottomLeft);
            }

            this.ResetPageRegionsFrom(_childs, _contentMargin);
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
            // this.RenderBordersIf(renderer, true);
        }

        public static Cell From(
            Word.TableCell wordCell,
            GridPosition gridPosition,
            IStyleFactory styleFactory)
        {
            var childs = wordCell
                .RenderableChildren()
                .CreateInitializeElements(styleFactory)
                .ToArray();

            var borderStyle = wordCell.GetBorderStyle();

            return new Cell(childs, gridPosition, borderStyle);
        }

        public override void Prepare(PageContext pageContext, Func<PagePosition, ContainerElement, PageContext> nextPageContextFactory)
        {
            throw new NotImplementedException();
        }
    }
}
