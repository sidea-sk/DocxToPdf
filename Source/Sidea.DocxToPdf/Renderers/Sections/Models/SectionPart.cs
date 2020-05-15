using System.Collections.Generic;
using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionPart : CompositeRenderer
    {
        public SectionPart(
            SectionBreak sectionBreak,
            IEnumerable<OpenXmlCompositeElement> elements,
            IStyleAccessor styleAccessor) : base(elements, styleAccessor)
        {
            this.SectionBreak = sectionBreak;
        }

        public SectionBreak SectionBreak { get; }
    }
}
