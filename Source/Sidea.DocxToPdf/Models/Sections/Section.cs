using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Footers;
using Sidea.DocxToPdf.Models.Footers.Builders;
using Sidea.DocxToPdf.Models.Headers;
using Sidea.DocxToPdf.Models.Headers.Builders;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section : PageElement
    {
        private List<Page> _pages = new List<Page>();
        private Dictionary<PageNumber, HeaderBase> _headers = new Dictionary<PageNumber, HeaderBase>();
        private Dictionary<PageNumber, FooterBase> _footers = new Dictionary<PageNumber, FooterBase>();

        private readonly SectionProperties _properties;
        private readonly IImageAccessor _imageAccessor;
        private readonly SectionContent[] _contents;
        private readonly IStyleFactory _styleFactory;

        public Section(
            SectionProperties properties,
            IEnumerable<SectionContent> sectionContents,
            IImageAccessor imageAccessor,
            IStyleFactory styleFactory)
        {
            _properties = properties;
            _imageAccessor = imageAccessor;
            _contents = sectionContents.ToArray();
            _styleFactory = styleFactory;
        }

        public IReadOnlyCollection<IPage> Pages => _pages;
        public HeaderFooterConfiguration HeaderFooterConfiguration => _properties.HeaderFooterConfiguration;

        public void Prepare(
            PageRegion previousSection,
            PageMargin previousSectionMargin,
            DocumentVariables documentVariables)
        {
            var sectionBreak = _properties.StartOnNextPage
                ? SectionContentBreak.Page
                : SectionContentBreak.None;

            IPage contentPageRequest(PageNumber pageNumber) =>
                this.OnNewPage(pageNumber, previousSection.PagePosition.PageNumber, previousSectionMargin, documentVariables);

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

            foreach(var header in _headers.Values)
            {
                header.Render(renderer);
            }

            foreach (var footer in _footers.Values)
            {
                footer.Render(renderer);
            }

            foreach (var content in _contents)
            {
                content.Render(renderer);
            }

            // this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private IPage OnNewPage(
            PageNumber pageNumber,
            PageNumber previousSectionLastPage,
            PageMargin previousSectionMargin,
            DocumentVariables documentVariables)
        {
            this.EnsurePage(pageNumber, previousSectionLastPage, previousSectionMargin);
            var page = _pages.Single(p => p.PageNumber == pageNumber);
            page.DocumentVariables = documentVariables;
            return page;
        }

        private void EnsurePage(PageNumber pageNumber, PageNumber previousSectionLastPage, PageMargin previousSectionMargin)
        {
            if(_pages.Any(p => p.PageNumber == pageNumber))
            {
                return;
            }

            var newPage = new Page(pageNumber, _properties.PageConfiguration);
            newPage.SetHorizontalMargins(_properties.Margin.Left, _properties.Margin.Right);

            this.CreateHeader(newPage, previousSectionLastPage);
            this.CreateFooter(newPage, previousSectionLastPage, previousSectionMargin);

            var topMargin = previousSectionMargin.Top;
            if (_headers.ContainsKey(pageNumber))
            {
                topMargin = _headers[pageNumber].BottomY;
            }

            newPage.SetTopMargins(_properties.Margin.Header, topMargin);

            var footer = _footers[pageNumber];
            newPage.SetBottomMargins(footer.FooterMargin, footer.HeightWithFooterMargin);
            _pages.Add(newPage);
        }

        private void CreateHeader(IPage page, PageNumber previousSectionLastPage)
        {
            if(previousSectionLastPage == page.PageNumber)
            {
                return;
            }

            var header = _properties.HeaderFooterConfiguration
                .FindHeader(page.PageNumber)
                .CreateHeader(_properties.Margin, _imageAccessor, _styleFactory);

            header.Prepare(page);
            _headers.Add(page.PageNumber, header);
        }

        private void CreateFooter(IPage page, PageNumber previousSectionLastPage, PageMargin previousSectionMargin)
        {
            var footer = previousSectionLastPage == page.PageNumber
                ? FooterFactory.CreateInheritedFooter(previousSectionMargin)
                : _properties.HeaderFooterConfiguration
                     .FindFooter(page.PageNumber)
                     .CreateFooter(_properties.Margin, _imageAccessor, _styleFactory);

            footer.Prepare(page);
            _footers.Add(page.PageNumber, footer);
        }
    }
}
