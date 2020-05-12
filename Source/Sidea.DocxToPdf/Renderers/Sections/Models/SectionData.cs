using System.Collections.Generic;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    internal class SectionData
    {
        public SectionData(
             SectionProperties properties,
             IEnumerable<SectionPart> sectionParts)
        {
            this.Properties = properties;
            this.SectionParts = sectionParts.ToArray();
        }

        public SectionProperties Properties { get; }
        public IEnumerable<SectionPart> SectionParts { get; }
    }
}
