using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;

namespace Sidea.DocxToPdf.Renderers.Documents
{
    internal class DocumentRenderer
    {
        public PdfDocument Render(WordprocessingDocument docx)
        {
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            return pdf;
        }
    }
}
