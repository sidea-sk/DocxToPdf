using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal static class ContainerElementExtensions
    {
        public static IEnumerable<PageRegion> UnionPageRegions(this IEnumerable<ContainerElement> elements)
            => elements.UnionPageRegions(Margin.None);

        public static IEnumerable<PageRegion> UnionPageRegions(this IEnumerable<ContainerElement> elements, Margin margin)
        {
            var pageRegions = elements
                .SelectMany(c => c.PageRegions)
                .GroupBy(pr => pr.PageNumber)
                .Select(grp =>
                {
                    var rectangle = Rectangle.Union(grp.Select(r => r.Region.Inflate(margin.Top, margin.Right, margin.Bottom, margin.Left)));
                    return new PageRegion(grp.Key, rectangle);
                })
                .ToArray();

            return pageRegions;
        }

        
    }
}
