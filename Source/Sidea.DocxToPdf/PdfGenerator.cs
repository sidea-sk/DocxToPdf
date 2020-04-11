using System.IO;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers.Documents;

namespace Sidea.DocxToPdf
{
    public class PdfGenerator
    {
        public PdfDocument Generate(Stream docxStream)
        {
            using var docx = WordprocessingDocument.Open(docxStream, false);
            var pdf = this.Generate(docx);
            return pdf;
        }

        public PdfDocument Generate(WordprocessingDocument docx)
        {
            var pdfDocument = new DocumentRenderer().Render(docx);
            return pdfDocument;
        }
    }
}
