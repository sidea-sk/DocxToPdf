using System.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class SectionXMLExtensions
    {
        public static PageOrientationValues GetPageOrientation(this SectionProperties sectionProperties)
        {
            var pageSize = sectionProperties.ChildsOfType<PageSize>().Single();
            return pageSize.Orient?.Value ?? PageOrientationValues.Portrait;
        }

        public static SectionMarkValues GetSectionMark(this SectionProperties sectionProperties)
        {
            var st = sectionProperties.ChildsOfType<SectionType>().SingleOrDefault();
            return st?.Val ?? SectionMarkValues.NextPage;
        }
    }
}
