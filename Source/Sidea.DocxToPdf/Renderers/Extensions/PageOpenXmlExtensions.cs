using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class PageOpenXmlExtensions
    {
        public static PageMargin GetPageMargin(this MainDocumentPart mainDocumentPart)
        {
            var sectionProperties = mainDocumentPart.Document.Body
                .ChildsOfType<SectionProperties>()
                .Single();

            var pageMargin = sectionProperties.ChildsOfType<PageMargin>().Single();
            return pageMargin;
        }
    }
}
