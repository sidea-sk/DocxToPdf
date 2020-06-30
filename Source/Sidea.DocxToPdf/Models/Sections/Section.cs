using System;
using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;
using Sidea.DocxToPdf.Models.Styles;
using OpenXml = DocumentFormat.OpenXml;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section
    {
        private List<IPage> _pages = new List<IPage>();

        private readonly SectionProperties _properties;
        private readonly SectionColumn[] _columns;
        private readonly IStyleFactory _styleFactory;

        public IReadOnlyCollection<IPage> Pages => _pages;

        public Section(
            SectionProperties properties,
            IEnumerable<SectionColumn> columns,
            IStyleFactory styleFactory)
        {
            _properties = properties;
            _columns = columns.ToArray();
            _styleFactory = styleFactory;
        }

        public void Initialize()
        {
            foreach(var column in _columns)
            {
                column.Initialize();
            }
        }

        public void Prepare(IPage lastPageOfPreviosSection, Rectangle occupiedSpace)
        {
            var pageNumber = lastPageOfPreviosSection.PageNumber.Next();
            this.EnsurePage(pageNumber);

            for (var i = 0; i < _columns.Length; i++)
            {
                // todo: handle column and page breaks
                var context = this.CreateContextForColumn(pageNumber, Rectangle.Empty, i);
                var column = _columns[i];
                column.Prepare(context, this.OnNewPage);

                switch (column.SectionBreak)
                {
                    case SectionBreak.None:
                        break;
                    case SectionBreak.Column:
                        break;
                    case SectionBreak.Page:
                        // create new Page for the Next Column
                        pageNumber = column.LastPageRegion.PageNumber.Next();
                        this.EnsurePage(pageNumber);
                        break;
                }
            }
        }

        public void Render(IRenderer renderer)
        {
            foreach(var child in _columns)
            {
                child.Render(renderer);
            }
        }

        private PageContext OnNewPage(PageNumber pageNumber, ContainerElement requestingColumn)
        {
            this.EnsurePage(pageNumber);

            var index = _columns.IndexOf(c => c == requestingColumn);
            if(index == -1)
            {
                throw new RendererException("Column not found");
            }

            var page = _pages.Single(p => p.PageNumber == pageNumber);

            return new PageContext(pageNumber, page.GetContentRegion(), new Variables(totalPages: _pages.Count));
        }

        private PageContext CreateContextForColumn(PageNumber pageNumber, Rectangle occupiedRegion, int columnIndex)
        {
            var page = _pages.Single(p => p.PageNumber == pageNumber);
            var xOffset = _properties.ColumnOffset(columnIndex) + page.Margin.Left;
            var width = _properties.ColumnWidth(columnIndex);

            var y = Math.Max(page.Margin.Top, occupiedRegion.BottomY);

            var content = page
                .GetContentRegion()
                .Clip(new Point(0, y))
                .RestrictLeftWidth(xOffset, width);

            return new PageContext(pageNumber, content, new Variables(totalPages: _pages.Count));
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
