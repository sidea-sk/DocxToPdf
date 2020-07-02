using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;
using Sidea.DocxToPdf.Models.Tables.Builders;

using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class Cell : ContainerElement
    {
        private readonly BorderStyle _borderStyle;
        private readonly Margin _contentMargin;
        private ContainerElement[] _childs = new ContainerElement[0];

        private Cell(IEnumerable<ContainerElement> childs, GridPosition gridPosition, BorderStyle borderStyle)
        {
            _contentMargin = new Margin(0, 0, 0, 0);
            _childs = childs.ToArray();
            _borderStyle = borderStyle;

            this.GridPosition = gridPosition;
        }

        public GridPosition GridPosition { get; }

        public override void Prepare(PageContext pageContext, Func<PageNumber, ContainerElement, PageContext> pageFactory)
        {
            var currentPageContext = pageContext
                .Crop(_contentMargin);

            Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement) =>
            {
                var c = pageFactory(pageNumber, this);
                currentPageContext = c.Crop(_contentMargin);
                return c;
            };

            Rectangle availableRegion = currentPageContext.Region;
            foreach (var child in _childs)
            {
                child.Prepare(new PageContext(currentPageContext.PageNumber, availableRegion, currentPageContext.PageVariables), onNewPage);
                var lastPage = child.LastPageRegion.Region;

                availableRegion = currentPageContext.Region.Clip(lastPage.BottomLeft);
            }

            // fix this!
            this.ResetPageRegionsFrom(_childs, _contentMargin);
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
            this.RenderBordersIf(renderer, true);
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
    }
}
