using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section : PageElement
    {
        private List<Page> _pages = new List<Page>();

        private readonly SectionProperties _properties;
        private readonly SectionContent[] _contents;
        private readonly IStyleFactory _styleFactory;

        public Section(
            SectionProperties properties,
            IEnumerable<SectionContent> sectionContents,
            IStyleFactory styleFactory)
        {
            _properties = properties;
            _contents = sectionContents.ToArray();
            _styleFactory = styleFactory;
        }

        public IReadOnlyCollection<IPage> Pages => _pages;
        public HeaderFooterConfiguration HeaderFooterConfiguration => _properties.HeaderFooterConfiguration;

        public void Prepare(
            PageRegion previousSection,
            DocumentVariables documentVariables)
        {
            var sectionBreak = _properties.StartOnNextPage
                ? SectionContentBreak.Page
                : SectionContentBreak.None;

            IPage contentPageRequest(PageNumber pageNumber) =>
                this.OnNewPage(pageNumber, documentVariables);

            var contentLastPosition = PageRegion.None;
            foreach (var content in _contents)
            {
                content.Prepare(previousSection, contentLastPosition, sectionBreak, contentPageRequest);
                contentLastPosition = content.LastPageRegion;
                sectionBreak = content.SectionBreak;
            }

            var pr = _contents
                .SelectMany(c => c.PageRegions)
                .UnionThroughColumns()
                .ToArray();
            this.ResetPageRegions(pr);
        }

        public override void Render(IRenderer renderer)
        {
            //// page content borders
            //foreach (var p in _pages)
            //{
            //    var r = p.GetContentRegion();
            //    var rp = renderer.Get(p.PageNumber);
            //    var pen = new System.Drawing.Pen(System.Drawing.Color.Orange, 0.5f);

            //    rp.RenderLine(r.TopLine(pen));
            //    rp.RenderLine(r.RightLine(pen));
            //    rp.RenderLine(r.BottomLine(pen));
            //    rp.RenderLine(r.LeftLine(pen));
            //}

            foreach (var content in _contents)
            {
                content.Render(renderer);
            }

            // this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private IPage OnNewPage(PageNumber pageNumber, DocumentVariables documentVariables)
        {
            this.EnsurePage(pageNumber);
            var page = _pages.Single(p => p.PageNumber == pageNumber);
            page.DocumentVariables = documentVariables;
            return page;
        }

        private void EnsurePage(PageNumber pageNumber)
        {
            if(_pages.Any(p => p.PageNumber == pageNumber))
            {
                return;
            }

            var newPage = new Page(pageNumber, _properties.PageConfiguration)
            {
                Margin = new Margin(80, _properties.Margin.Right, 80, _properties.Margin.Left)
            };
            
            _pages.Add(newPage);
        }
    }
}
