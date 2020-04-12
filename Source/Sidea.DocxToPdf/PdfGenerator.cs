using System.IO;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers;
using Sidea.DocxToPdf.Renderers.Core;
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
            var renderer = new DocumentRenderer(docx);
            renderer.Render(new EmptyRenderArea());
            return renderer.GeneratedDocument;
        }
    }
}
