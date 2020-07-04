using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section : PageElement
    {
        private List<IPage> _pages = new List<IPage>();

        private readonly SectionProperties _properties;
        private readonly SectionContent[] _contents;
        private readonly IStyleFactory _styleFactory;

        public IReadOnlyCollection<IPage> Pages => _pages;

        public Section(
            SectionProperties properties,
            IEnumerable<SectionContent> sectionContents,
            IStyleFactory styleFactory)
        {
            _properties = properties;
            _contents = sectionContents.ToArray();
            _styleFactory = styleFactory;
        }

        public void Prepare(IPage lastPageOfPreviosSection, Rectangle occupiedSpace, DocumentVariables documentVariables)
        {
            var pageNumber = lastPageOfPreviosSection.PageNumber.Next();
            var contentLastPosition = PagePosition.None;
            var sectionBreak = SectionBreak.Page;

            foreach (var content in _contents)
            {
                content.Prepare(contentLastPosition, sectionBreak, this.OnNewPage);
                contentLastPosition = content.LastPageRegion.PagePosition;
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

            this.RenderBordersIf(renderer, renderer.Options.SectionRegionBoundaries);
        }

        private IPage OnNewPage(PageNumber pageNumber)
        {
            this.EnsurePage(pageNumber);
            var page = _pages.Single(p => p.PageNumber == pageNumber);
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
