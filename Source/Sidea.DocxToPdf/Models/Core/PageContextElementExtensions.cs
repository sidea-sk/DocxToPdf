﻿using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models
{
    internal static class PageContextElementExtensions
    {
        public static IEnumerable<PageRegion> UnionPageRegions(this IEnumerable<PageContextElement> elements, Margin contentMargin = null)
            => elements.UnionPageRegionsCore(contentMargin ?? Margin.None);

        private static IEnumerable<PageRegion> UnionPageRegionsCore(this IEnumerable<PageContextElement> elements, Margin contentMargin)
        {
            var pageRegions = elements
                .SelectMany(c => c.PageRegions)
                .UnionPageRegions(contentMargin)
                .ToArray();

            return pageRegions;
        }
    }
}
