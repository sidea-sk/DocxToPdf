using System.Collections.Generic;
using System.Linq;
using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Styles;
using OpenXml = DocumentFormat.OpenXml;

namespace Sidea.DocxToPdf.Models.Sections
{
    internal class Section
    {
        private List<IPage> _pages = new List<IPage>();
        private readonly SectionProperties _properties;
        private readonly IStyleFactory _styleFactory;

        private readonly IReadOnlyCollection<OpenXml.OpenXmlCompositeElement> _openXmlElements;
        private ContainerElement[] _childs = new ContainerElement[0];

        public IReadOnlyCollection<IPage> Pages { get; }

        public Section(
            IEnumerable<OpenXml.OpenXmlCompositeElement> openXmlElements,
            SectionProperties properties,
            IStyleFactory styleFactory)
        {
            _openXmlElements = openXmlElements.ToArray();
            _properties = properties;
            _styleFactory = styleFactory;
        }

        public void Initialize()
        {
            _childs = _openXmlElements
                .Select(e => Factory.Create(e, _styleFactory))
                .ToArray();
        }

        public void Prepare(object previousPageInfo)
        {

        }

        public void Update(object previousPageInfo)
        {

        }
    }
}
