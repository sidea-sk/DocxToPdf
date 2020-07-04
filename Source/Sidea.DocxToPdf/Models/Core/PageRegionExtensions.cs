using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal static class PageRegionExtensions
    {
        public static IEnumerable<PageRegion> UnionPageRegionsCore(
            this IEnumerable<PageRegion> pageRegions,
            Margin contentMargin)
        {
            var unioned = pageRegions
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

            var last = unioned.Last();
            unioned[unioned.Length - 1] = new PageRegion(last.PagePosition, last.Region.Inflate(0, 0, contentMargin.Bottom, 0));

            return unioned;
        }

        public static IEnumerable<PageRegion> UnionThroughColumns(
            this IEnumerable<PageRegion> pageRegions)
        {
            var unioned = pageRegions
                .GroupBy(pr => pr.PagePosition.PageNumber)
                .Select(grp =>
                {
                    var rectangle = Rectangle.Union(grp.Select(r => r.Region));
                    return new PageRegion(new PagePosition(grp.Key, 0, 1), rectangle);
                })
                .ToArray();

            return unioned;
        }
    }
}
