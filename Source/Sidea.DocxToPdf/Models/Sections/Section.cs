using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;
using OpenXml = DocumentFormat.OpenXml;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section
    {
        // private SectionPageManager _pageManager;

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
            //var nextPageNumber = lastPageOfPreviosSection.PageNumber.Next();
            //_pageManager.EnsurePage(nextPageNumber);
            //var page = _pageManager.GetPage(nextPageNumber);
            //_pages.Add(page);

            //var availableRegion = page.GetContentRegion();

            var pageContext = this.OnNewPage(lastPageOfPreviosSection.PageNumber.Next());
            foreach(var child in _childs)
            {
                child.Prepare(pageContext, this.OnNewPage);
            }
        }

        public void Update(object previousPageInfo)
        {
        }

        private PageContext OnNewPage(PageNumber pageNumber)
        {
            if(_pages.All(p => p.PageNumber != pageNumber))
            {
                var newPage = new Page(pageNumber, _properties.PageConfiguration);
                newPage.Margin = new Common.Margin(80, 80, 80, 80);
                _pages.Add(newPage);
            }

            var page = _pages.Single(p => p.PageNumber == pageNumber);
            return new PageContext(pageNumber, page.GetContentRegion());
        }
    }
}
