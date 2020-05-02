using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class HeaderXmlExtensions
    {
        public static Header FindHeaderForPage(this MainDocumentPart mainDocumentPart, int pageNumber)
        {
            var sectionProperties = mainDocumentPart.Document.Body
                .ChildsOfType<SectionProperties>()
                .SingleOrDefault();

            var headerReference = sectionProperties
                .ChildsOfType<HeaderReference>()
                .ToArray()
                .ChooseHeaderReference(pageNumber);

            if(headerReference == null)
            {
                return null;
            }

            var headerPart = (HeaderPart)mainDocumentPart.GetPartById(headerReference.Id);
            return headerPart.Header;
        }

        private static HeaderReference ChooseHeaderReference(this IReadOnlyCollection<HeaderReference> references, int pageNumber)
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
