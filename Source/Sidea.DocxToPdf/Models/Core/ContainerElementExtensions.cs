using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal static class ContainerElementExtensions
    {
        public static IEnumerable<PageRegion> UnionPageRegions(this IEnumerable<ContainerElement> _elements)
        {
            var pageRegions = _elements
                .SelectMany(c => c.PageRegions)
                .GroupBy(pr => pr.PageNumber)
                .Select(grp =>
                {
                    var rectangle = Rectangle.Union(grp.Select(r => r.Region));
                    return new PageRegion(grp.Key, rectangle);
                })
                .ToArray();

            return pageRegions;
        }

        
    }
}
