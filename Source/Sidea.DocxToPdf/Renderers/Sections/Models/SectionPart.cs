using System.Collections.Generic;
using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionPart : CompositeRenderer
    {
        public SectionPart(
            SectionBreak sectionBreak,
            IEnumerable<OpenXmlCompositeElement> elements) : base(elements)
        {
            this.SectionBreak = sectionBreak;
        }

        public SectionBreak SectionBreak { get; }
    }
}
