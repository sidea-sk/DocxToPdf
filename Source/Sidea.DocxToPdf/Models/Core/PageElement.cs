using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal abstract class PageElement : IRenderable
    {
        public IReadOnlyCollection<PageRegion> PageRegions { get; private set; } = new PageRegion[0];
        public PageRegion LastPageRegion => this.PageRegions.LastOrDefault() ?? PageRegion.None;

        public abstract void Render(IRenderer renderer);

        protected void ClearPageRegions()
        {
            this.PageRegions = new PageRegion[0];
        }

        protected void SetPageRegion(PageRegion pageRegion)
        {
            this.PageRegions = this.PageRegions
                .Where(pr => pr.PagePosition != pageRegion.PagePosition)
                .Union(new[] { pageRegion })
                .OrderBy(pr => pr.PagePosition)
                .ToArray();
        }

        protected void ResetPageRegions(IEnumerable<PageRegion> pageRegions)
        {
            this.PageRegions = pageRegions.ToArray();
        }

        protected void ResetPageRegionsFrom(IEnumerable<PageContextElement> children, Margin contentMargin = null)
        {
            this.PageRegions = children
                .UnionPageRegions(contentMargin)
                .ToArray();
        }

        protected void RenderBordersIf(IRenderer renderer, bool condition, Point pageOffset = null)
        {
            if (!condition)
            {
                return;
            }

            var index = -1;
            foreach (var pageRegion in this.PageRegions)
            {
                index++;
                var page = renderer.GetPage(pageRegion.PagePosition.PageNumber, pageOffset ?? Point.Zero);
                this.RenderBorder(page, pageRegion.Region, index == 0, index == this.PageRegions.Count - 1);
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
