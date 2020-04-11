using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers.Documents;

namespace Sidea.DocxToPdf
{
    public class PdfGenerator
    {
        public PdfDocument Generate(WordprocessingDocument wordDocument)
        {
            var pdfDocument = new DocumentRenderer().Render(wordDocument);
            return pdfDocument;
        }
    }
}
