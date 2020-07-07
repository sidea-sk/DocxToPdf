using System.IO;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Pdf;
using Sidea.DocxToPdf.Models;

namespace Sidea.DocxToPdf
{
    public class PdfGenerator
    {
        public PdfDocument Generate(Stream docxStream, RenderingOptions options = null)
        {
            using var docx = WordprocessingDocument.Open(docxStream, false);
            var pdf = this.Generate(docx, options);
            return pdf;
        }

        public MemoryStream GenerateAsStream(Stream docxStream, RenderingOptions options = null)
        {
            var pdf = this.Generate(docxStream, options);
            var ms = new MemoryStream();
            pdf.Save(ms);
            return ms;
        }

        public byte[] GenerateAsByteArray(Stream docxStream, RenderingOptions options = null)
        {
            using var ms = this.GenerateAsStream(docxStream, options);
            return ms.ToArray();
        }

        private PdfDocument Generate(WordprocessingDocument docx, RenderingOptions options = null)
        {
            var renderingOptions = options ?? RenderingOptions.Default;

            var pdfDocument = new PdfDocument();
            var renderer = new PdfRenderer(pdfDocument, renderingOptions);
            var document = new Document(docx);
            document.Render(renderer);
            return pdfDocument;
        }
    }
}
