using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Sections.Columns;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionContent // : ContainerElement
    {
        private readonly ContainerElement[] _childs;
        private readonly ColumnsConfiguration _columnsConfiguration;

        public SectionContent(
            IEnumerable<ContainerElement> childs,
            ColumnsConfiguration columnsConfiguration,
            SectionBreak sectionBreak)
        {
            _childs = childs.ToArray();
            _columnsConfiguration = columnsConfiguration;
            this.SectionBreak = sectionBreak;
        }

        public SectionBreak SectionBreak { get; }

        public PagePosition LastPagePosition { get; private set; } = PagePosition.None;

        // public int LastColumnIndex { get; private set; } = -1;

        //public void Prepare(
        //    PageContext pageContext,
        //    Func<PageNumber, ContainerElement, PageContext> pageFactory)
        //{
        //    var currentPageContext = pageContext;

        //    Func<PageNumber, ContainerElement, PageContext> onNewPage = (pageNumber, childElement) =>
        //    {
        //        var c = pageFactory(pageNumber, this);
        //        currentPageContext = c;
        //        return c;
        //    };

        //    Rectangle availableRegion = pageContext.Region;
        //    foreach (var child in _childs)
        //    {
        //        child.Prepare(new PageContext(currentPageContext.PageNumber, 0, availableRegion, currentPageContext.PageVariables), onNewPage);
        //        var lastPage = child.LastPageRegion.Region;

        //        availableRegion = currentPageContext.Region.Clip(lastPage.BottomLeft);
        //    }

        //    this.ResetPageRegionsFrom(_childs);
        //}

        public void Prepare(
            PagePosition previousContentPosition,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            var context = this.CreateInitialPageContenxt(previousContentPosition, previousBreak, pageFactory);

            Func<PagePosition, ContainerElement, PageContext> childContextRequest = (pagePosition, child)
                => this.OnChildPageContextRequest(pagePosition, pageFactory);

            foreach(var child in _childs)
            {
                child.Prepare(context, childContextRequest);
                var lastRegion = child.LastPageRegion;
                context = this.CreateContextForPagePosition(lastRegion.PagePosition, lastRegion.Region, pageFactory);
            }
        }

        public void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
            // this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private PageContext CreateInitialPageContenxt(
            PagePosition previousContentPosition,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            switch (previousBreak)
            {
                case SectionBreak.Column:
                    {
                        this.LastPagePosition = previousContentPosition.Next();
                        return this.CreateContextForPagePosition(this.LastPagePosition, Rectangle.Empty, pageFactory);
                    }
                case SectionBreak.Page:
                case SectionBreak.None:
                    {
                        var pn = previousContentPosition.PageNumber.Next();
                        this.LastPagePosition = new PagePosition(pn, 0, _columnsConfiguration.ColumnsCount);
                        return this.CreateContextForPagePosition(this.LastPagePosition, Rectangle.Empty, pageFactory);
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
                new DocumentVariables(totalPages: page.PageNumber));

            var cropY = occupiedRegion.BottomY == 0
                ? 0
                : occupiedRegion.BottomY - page.Margin.Top;

            context = context.CropFromTop(cropY);
            return context;
        }
    }
}
