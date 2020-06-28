using System.Collections.Generic;
using Sidea.DocxToPdf.Models.Styles;
using Pack = DocumentFormat.OpenXml.Packaging;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Sections.Builders
{
    internal static class SectionBuilder
    {
        public static IEnumerable<Section> SplitToSections(
            this Pack.MainDocumentPart mainDocument,
            IStyleFactory styleFactory)
        {
            var useEvenOdd = mainDocument.DocumentSettingsPart.EvenOddHeadersAndFooters();

            return null;
        }
    }
}
