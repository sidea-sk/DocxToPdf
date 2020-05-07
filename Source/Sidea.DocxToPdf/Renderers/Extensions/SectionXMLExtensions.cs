using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class SectionXMLExtensions
    {
        public static SectionMarkValues GetSectionMark(this SectionProperties sectionProperties)
        {
            var st = sectionProperties.ChildsOfType<SectionType>().SingleOrDefault();
            return st?.Val ?? SectionMarkValues.NextPage;
        }
    }
}
