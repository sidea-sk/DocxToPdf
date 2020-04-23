using System.IO;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers;

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

        public PdfDocument Generate(WordprocessingDocument docx, RenderingOptions options = null)
        {
            var renderingOptions = options ?? RenderingOptions.Default;
            var renderer = new DocumentRenderer(docx, renderingOptions);
            var pdfDocument = renderer.Render();
            return pdfDocument;
        }
    }
}
