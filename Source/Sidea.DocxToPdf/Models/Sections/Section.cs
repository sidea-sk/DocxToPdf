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
        private readonly IStyleFactory _styleFactory;

        private readonly IReadOnlyCollection<OpenXml.OpenXmlCompositeElement> _openXmlElements;
        private ContainerElement[] _childs = new ContainerElement[0];

        public IReadOnlyCollection<IPage> Pages => _pages;

        public Section(
            IEnumerable<OpenXml.OpenXmlCompositeElement> openXmlElements,
            SectionProperties properties,
            IStyleFactory styleFactory)
        {
            // _pageManager = new SectionPageManager(properties.PageConfiguration, this.OnPageCreated);
            _openXmlElements = openXmlElements.ToArray();
            _properties = properties;
            _styleFactory = styleFactory;
        }

        public void Initialize()
        {
            _childs = _openXmlElements
                .OfType<Word.Paragraph>()
                .Select(e => {
                    var p = Factory.Create(e, _styleFactory);
                    p.Initialize();
                    return p;
                })
                .ToArray();
        }

        public void Prepare(IPage lastPageOfPreviosSection, Rectangle occupiedSpace)
        {
            var pageContext = this.OnNewPage(lastPageOfPreviosSection.PageNumber.Next());

            foreach(var child in _childs)
            {
                child.Prepare(pageContext, this.OnNewPage);
                var lastPage = child.LastPageRegion;
                pageContext = this.CreatePageContext(lastPage.PageNumber, lastPage.Region);
            }
        }

        public void Update(IPage lastPageOfPreviosSection, Rectangle occupiedSpace)
        {
        }

        public void Render(IRenderer renderer)
        {
            foreach(var child in _childs)
            {
                child.Render(renderer);
            }
        }

        private PageContext OnNewPage(PageNumber pageNumber)
        {
            if(_pages.All(p => p.PageNumber != pageNumber))
            {
                var newPage = new Page(pageNumber, _properties.PageConfiguration);
                newPage.Margin = new Margin(80, _properties.Margin.Right, 80, _properties.Margin.Left);
                _pages.Add(newPage);
            }

            var page = _pages.Single(p => p.PageNumber == pageNumber);
            return new PageContext(pageNumber, page.GetContentRegion(), new Variables(totalPages: _pages.Count));
        }

        private PageContext CreatePageContext(PageNumber pageNumber, Rectangle occupiedRegion)
        {
            var page = _pages.Single(p => p.PageNumber == pageNumber);
            var content = page
                .GetContentRegion()
                .Clip(occupiedRegion.BottomLeft);
            
            return new PageContext(pageNumber, content, new Variables(totalPages: _pages.Count));
        }
    }
}
