using System;
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
            PagePosition previousContentPosition,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            var context = this.CreateInitialPageContenxt(previousContentPosition, previousBreak, pageFactory);

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

        private PageContext CreateInitialPageContenxt(
            PagePosition previousContentPosition,
            SectionBreak previousBreak,
            Func<PageNumber, IPage> pageFactory)
        {
            switch (previousBreak)
            {
                case SectionBreak.Column:
                    {
                        var pp = previousContentPosition.Next();
                        return this.CreateContextForPagePosition(pp, Rectangle.Empty, pageFactory);
                    }
                case SectionBreak.Page:
                case SectionBreak.None:
                    {
                        var pn = previousContentPosition.PageNumber.Next();
                        return this.CreateContextForPagePosition(new PagePosition(pn, 0, _columnsConfiguration.ColumnsCount), Rectangle.Empty, pageFactory);
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
