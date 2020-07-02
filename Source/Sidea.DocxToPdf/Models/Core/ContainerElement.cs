using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal abstract class ContainerElement : IRenderable
    {
        private PageRegion[] _pageRegions = new PageRegion[0];

        public IReadOnlyCollection<PageRegion> PageRegions => _pageRegions;
        public PageRegion LastPageRegion => _pageRegions.LastOrDefault() ?? PageRegion.None;

        public abstract void Prepare(
            PageContext pageContext,
            Func<PageNumber, ContainerElement, PageContext> pageFactory);

        public abstract void Render(IRenderer renderer);

        protected void ClearPageRegions()
        {
            _pageRegions = new PageRegion[0];
        }

        protected void SetPageRegion(PageRegion pageRegion)
        {
            _pageRegions = _pageRegions
                .Where(pr => pr.PageNumber != pageRegion.PageNumber)
                .Union(new[] { pageRegion })
                .OrderBy(pr => pr.PageNumber)
                .ToArray();
        }

        protected void ResetPageRegionsFrom(IEnumerable<ContainerElement> children, Margin contentMargin = null)
        {
            _pageRegions = children
                .UnionPageRegions(contentMargin)
                .ToArray();
        }

        protected void RenderBordersIf(IRenderer renderer, bool condition)
        {
            if (!condition)
            {
                return;
            }

            var index = -1;
            foreach(var pageRegion in _pageRegions)
            {
                index++;
                var page = renderer.Get(pageRegion.PageNumber);
                this.RenderBorder(page, pageRegion.Region, index == 0, index == _pageRegions.Length - 1);
            }
        }

        private void RenderBorder(IRendererPage page, Rectangle region, bool isFirst, bool isLast)
        {
            var pen = new System.Drawing.Pen(System.Drawing.Color.Orange, 0.5f);
            if (isFirst)
            {
                page.RenderLine(region.TopLine(pen));
            }

            page.RenderLine(region.RightLine(pen));

            if (isLast)
            {
                page.RenderLine(region.BottomLine(pen));
            }

            page.RenderLine(region.LeftLine(pen));
        }
    }
}
