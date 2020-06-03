using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class FooterXmlExtensions
    {
        public static Footer FindFooterForPage(this MainDocumentPart mainDocumentPart, int pageNumber, bool hasTitlePage)
        {
            var useEvenOdd = mainDocumentPart.DocumentSettingsPart.EvenOddHeadersAndFooters();

            var sectionProperties = mainDocumentPart.Document.Body
                .ChildsOfType<SectionProperties>()
                .SingleOrDefault();

            var footerReference = sectionProperties
                .ChildsOfType<FooterReference>()
                .ToArray()
                .ChooseFooterReference(pageNumber, hasTitlePage, useEvenOdd);

            if(footerReference == null)
            {
                return null;
            }

            var footerPart = (FooterPart)mainDocumentPart.GetPartById(footerReference.Id);
            return footerPart.Footer;
        }

        private static FooterReference ChooseFooterReference(
            this IReadOnlyCollection<FooterReference> references,
            int pageNumber,
            bool hasTitlePage,
            bool useEvenOdd)
        {
            if (hasTitlePage && pageNumber == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.First)
                    ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
            }

            if(!useEvenOdd || pageNumber % 2 == 1)
            {
                return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
            }

            return references.FirstOrDefault(r => r.Type == HeaderFooterValues.Even)
                ?? references.FirstOrDefault(r => r.Type == HeaderFooterValues.Default);
        }
    }
}
