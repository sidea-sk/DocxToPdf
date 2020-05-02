using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class FooterXmlExtensions
    {
        public static Footer FindFooterForPage(this MainDocumentPart mainDocumentPart, int pageNumber)
        {
            var sectionProperties = mainDocumentPart.Document.Body
                .ChildsOfType<SectionProperties>()
                .SingleOrDefault();

            var footerReference = sectionProperties
                .ChildsOfType<FooterReference>()
                .ToArray()
                .ChooseHeaderReference(pageNumber);

            if(footerReference == null)
            {
                return null;
            }

            var footerPart = (FooterPart)mainDocumentPart.GetPartById(footerReference.Id);
            return footerPart.Footer;
        }

        private static FooterReference ChooseHeaderReference(this IReadOnlyCollection<FooterReference> references, int pageNumber)
        {
            if(pageNumber == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.First)
                    ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
            }

            if(pageNumber % 2 == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
            }

            return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Even)
                ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
        }
    }
}
