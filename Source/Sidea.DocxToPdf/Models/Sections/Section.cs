using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section
    {
        private List<IPage> _pages = new List<IPage>();

        private readonly SectionProperties _properties;
        private readonly SectionContent[] _contents;
        private readonly IStyleFactory _styleFactory;

        public IReadOnlyCollection<PageRegion> PageRegions { get; private set; } = new PageRegion[0];
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
                contentLastPosition = content.LastPagePosition;
                sectionBreak = content.SectionBreak;
            }
        }

        //public void Prepare(IPage lastPageOfPreviosSection, Rectangle occupiedSpace, Variables documentVariables)
        //{
        //    var pageNumber = lastPageOfPreviosSection.PageNumber.Next();
        //    this.EnsurePage(pageNumber);

        //    for (var i = 0; i < _contents.Length; i++)
        //    {
        //        // todo: handle column and page breaks
        //        var context = this.CreateContextForColumn(pageNumber, Rectangle.Empty, i, documentVariables);
        //        var column = _contents[i];
        //        column.Prepare(context, this.OnNewPage);

        //        switch (column.SectionBreak)
        //        {
        //            case SectionBreak.None:
        //                break;
        //            case SectionBreak.Column:
        //                break;
        //            case SectionBreak.Page:
        //                // create new Page for the Next Column
        //                pageNumber = column.LastPageRegion.PageNumber.Next();
        //                this.EnsurePage(pageNumber);
        //                break;
        //        }
        //    }

        //    this.PageRegions = _contents.UnionPageRegions().ToArray();
        //}

        public void Render(IRenderer renderer)
        {
            foreach(var content in _contents)
            {
                content.Render(renderer);
            }
        }

        private IPage OnNewPage(PageNumber pageNumber)
        {
            this.EnsurePage(pageNumber);
            var page = _pages.Single(p => p.PageNumber == pageNumber);
            return page;
        }

        //private PageContext OnNewPage(PageNumber pageNumber, SectionContent requestingColumn)
        //{
        //    this.EnsurePage(pageNumber);

        //    var index = _contents.IndexOf(c => c == requestingColumn);
        //    if(index == -1)
        //    {
        //        throw new RendererException("Column not found");
        //    }

        //    var page = _pages.Single(p => p.PageNumber == pageNumber);

        //    return new PageContext(pageNumber, 0, page.GetContentRegion(), new DocumentVariables(totalPages: _pages.Count));
        //}

        //private PageContext CreateContextForColumn(
        //    PageNumber pageNumber,
        //    Rectangle occupiedRegion,
        //    int columnIndex,
        //    Variables documentVariables)
        //{
        //    var page = _pages.Single(p => p.PageNumber == pageNumber);
        //    var columnSpace = _properties.CalculateColumnSpace(columnIndex);

        //    var y = Math.Max(page.Margin.Top, occupiedRegion.BottomY);

        //    var content = page
        //        .GetContentRegion()
        //        .CropHorizontal(columnSpace.X, columnSpace.Width);

        //    return new PageContext(pageNumber, 0, content, documentVariables);
        //}

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
