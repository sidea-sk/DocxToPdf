using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal static class ContainerElementExtensions
    {
        public static IEnumerable<PageRegion> UnionPageRegions(this IEnumerable<ContainerElement> elements, Margin contentMargin = null)
            => elements.UnionPageRegionsCore(contentMargin ?? Margin.None);

        private static IEnumerable<PageRegion> UnionPageRegionsCore(this IEnumerable<ContainerElement> elements, Margin contentMargin)
        {
            var pageRegions = elements
                .SelectMany(c => c.PageRegions)
                .GroupBy(pr => pr.PagePosition)
                .Select(grp =>
                {
                    var rectangle = Rectangle.Union(grp.Select(r => r.Region));
                    return new PageRegion(grp.Key, rectangle);
                })
                .Select((pr, i) =>
                {
                    var top = i == 0
                        ? contentMargin.Top
                        : 0;
                    var npr = new PageRegion(pr.PagePosition, pr.Region.Inflate(top, contentMargin.Right, 0, contentMargin.Left));
                    return npr;
                })
                .ToArray();

            var last = pageRegions.Last();
            pageRegions[pageRegions.Length - 1] = new PageRegion(last.PagePosition, last.Region.Inflate(0, 0, contentMargin.Bottom, 0));

            return pageRegions;
        }
    }
}
