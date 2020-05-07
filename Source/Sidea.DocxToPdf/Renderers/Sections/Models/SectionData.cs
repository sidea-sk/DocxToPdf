using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionData
    {
        public SectionData(
             SectionProperties properties,
            IEnumerable<OpenXmlCompositeElement> elements)
        {
            this.Properties = properties;
            this.Elements = elements.ToArray();
        }

        public SectionProperties Properties { get; }
        public IReadOnlyCollection<OpenXmlCompositeElement> Elements { get; }
    }
}
