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
        private SectionPageManager _pageManager;

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
            _pageManager = new SectionPageManager(properties.PageConfiguration, this.OnPageCreated);
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

        public void Prepare(IPage previousPage, Rectangle occupiedSpace)
        {

        }

        public void Update(object previousPageInfo)
        {
        }

        private void OnPageCreated(Page page)
        {
            page.TopMargin = 80;
            page.BottomMargin = 80;
        }
    }
}
