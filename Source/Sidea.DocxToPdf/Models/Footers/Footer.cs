﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Paragraphs;

namespace Sidea.DocxToPdf.Models.Footers
{
    internal class Footer : FooterBase
    {
        private readonly PageContextElement[] _childs;

        public Footer(
            IEnumerable<PageContextElement> childs,
            PageMargin pageMargin) : base(pageMargin)
        {
            _childs = childs.ToArray();
        }

        public override void Prepare(IPage page)
        {
            var pagePosition = new PagePosition(page.PageNumber);
            var region = page
                .GetPageRegion()
                .Crop(this.PageMargin.Top, this.PageMargin.Right, this.PageMargin.Footer, this.PageMargin.Left);

            var context = new PageContext(pagePosition, region, page.DocumentVariables);

            PageContext childContextRequest(PagePosition pagePosition, PageContextElement child)
                => this.OutOfPageContextFactory(page);

            var absoluteHeight = page.Configuration.Height;

            var spaceAfterPrevious = 0.0;
            foreach (var child in _childs)
            {
                child.Prepare(context, childContextRequest);
                var lastRegion = child.LastPageRegion;
                spaceAfterPrevious = child.CalculateSpaceAfter(_childs);

                var cropFromTop = Math.Min(lastRegion.Region.Height + spaceAfterPrevious, absoluteHeight - 0.001);
                context = context.CropFromTop(cropFromTop);
            }

            var boundingRegion = _childs
                .SelectMany(c => c.PageRegions.Where(pr => pr.PagePosition.PageNumber == page.PageNumber))
                .UnionPageRegions(Margin.None)
                .ToArray()
                .Single();

            //if (boundingRegion.Region.BottomY < this.PageMargin.Top)
            //{
            //    var resized = new Rectangle(
            //        boundingRegion.Region.TopLeft,
            //        boundingRegion.Region.Width,
            //        this.PageMargin.MinimalHeaderHeight);

            //    boundingRegion = new PageRegion(
            //        boundingRegion.PagePosition, resized);
            //}

            this.ResetPageRegions(new[] { boundingRegion });
        }

        public override void Render(IRenderer renderer)
        {
            _childs.Render(renderer);
            this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private PageContext OutOfPageContextFactory(IPage page)
        {
            var region = new Rectangle(0, page.Configuration.Height + 1, 1000000, 1000000);
            return new PageContext(PagePosition.None, region, new DocumentVariables(0));
        }
    }
}
