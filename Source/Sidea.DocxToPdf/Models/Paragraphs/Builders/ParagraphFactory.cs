using System.Linq;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Paragraphs.Builders
{
    internal static class ParagraphFactory
    {
        public static Paragraph Create(Word.Paragraph paragraph, IStyleFactory styleFactory)
        {
            var paragraphStyleFactory = styleFactory.ForParagraph(paragraph.ParagraphProperties);
            var fixedDrawings = paragraph
                .CreateFixedDrawingElements()
                .OrderBy(d => d.BoundingBox.Y)
                .ToArray();

            var elements = paragraph
                .CreateParagraphElements(paragraphStyleFactory);

            return new Paragraph(elements, fixedDrawings, paragraphStyleFactory);
        }
    }
}
