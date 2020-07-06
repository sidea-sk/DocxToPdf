﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Sections.Columns;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionContent : PageElement
    {
        private readonly PageContextElement[] _childs;
        private readonly ColumnsConfiguration _columnsConfiguration;

        public SectionContent(
            IEnumerable<PageContextElement> childs,
            ColumnsConfiguration columnsConfiguration,
            SectionBreak sectionBreak)
        {
            _childs = childs.ToArray();
            _columnsConfiguration = columnsConfiguration;
            this.SectionBreak = sectionBreak;
        }

        public SectionBreak SectionBreak { get; }

        public void Prepare(
            PageRegion previousSection,
            PageRegion previousContent,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            var context = this.CreateInitialPageContext(previousSection, previousContent, previousBreak, pageFactory);

            Func<PagePosition, PageContextElement, PageContext> childContextRequest = (pagePosition, child)
                => this.OnChildPageContextRequest(pagePosition, pageFactory);

            foreach(var child in _childs)
            {
                child.Prepare(context, childContextRequest);
                var lastRegion = child.LastPageRegion;
                context = this.CreateContextForPagePosition(lastRegion.PagePosition, lastRegion.Region, pageFactory);
            }

            this.ResetPageRegionsFrom(_childs);
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
            this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private PageContext CreateInitialPageContext(
            PageRegion previousSection,
            PageRegion previousContent,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            switch (previousBreak)
            {
                case SectionBreak.Column:
                    {
                        var pp = previousContent.PagePosition.Next();
                        var occupiedRegion = pp.PageNumber == previousSection.PagePosition.PageNumber
                            ? previousSection.Region
                            : Rectangle.Empty;

                        return this.CreateContextForPagePosition(pp, occupiedRegion, pageFactory);
                    }
                case SectionBreak.Page:
                    {
                        var pp = previousContent.PagePosition.NextPage(0, _columnsConfiguration.ColumnsCount);
                        return this.CreateContextForPagePosition(pp, Rectangle.Empty, pageFactory);
                    }
                case SectionBreak.None:
                    {

                        var pp = previousContent.PagePosition.SamePage(0, _columnsConfiguration.ColumnsCount);
                        return this.CreateContextForPagePosition(pp, previousContent.Region, pageFactory);
                    }
                default:
                    throw new RendererException("unhandled section break;");
            }
        }

        private PageContext OnChildPageContextRequest(
            PagePosition pagePosition,
            Func<PageNumber, IPage> pageFactory)
        {
            var nextPosition = pagePosition.Next();
            var context = this.CreateContextForPagePosition(nextPosition, Rectangle.Empty, pageFactory);
            return context;
        }

        private PageContext CreateContextForPagePosition(
            PagePosition pagePosition,
            Rectangle occupiedRegion,
            Func<PageNumber, IPage> pageFactory)
        {
            var page = pageFactory(pagePosition.PageNumber);
            var columnSpace = _columnsConfiguration.CalculateColumnSpace(pagePosition.PageColumn);
            var region = page
                .GetContentRegion()
                .CropHorizontal(columnSpace.X, columnSpace.Width);

            var context = new PageContext(
                pagePosition,
                region,
                page.DocumentVariables);

            var cropY = occupiedRegion.BottomY == 0
                ? 0
                : occupiedRegion.BottomY - page.Margin.Top;

            context = context.CropFromTop(cropY);
            return context;
        }
    }
}
