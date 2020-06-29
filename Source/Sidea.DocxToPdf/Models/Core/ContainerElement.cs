using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal abstract class ContainerElement
    {
        private PageRegion[] _pageRegions = new PageRegion[0];

        public IReadOnlyCollection<PageRegion> PageRegions { get; }
        public PageRegion LastPageRegion => _pageRegions.LastOrDefault() ?? PageRegion.None;

        public abstract void Initialize();

        public abstract void Prepare(
            PageContext pageContext,
            Func<PageNumber, PageContext> pageFactory);

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
    }
}
