using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Sidea.DocxToPdf.Renderers.Documents
{
    internal class DocumentRenderer
    {
        public PdfDocument Render(WordprocessingDocument docx)
        {
            var pdf = new PdfDocument();
            var page = this.CreatePage();
            pdf.AddPage(page);

            return pdf;
        }

        private PdfPage CreatePage()
        {
            var margin = XUnit.FromCentimeter(2.5);

            var page = new PdfPage
            {
                Size = PdfSharp.PageSize.A4,
                TrimMargins = new TrimMargins
                {
                    Top = margin,
                    Right = margin,
                    Bottom = margin,
                    Left = margin,
                }
            };

            return page;
        }
    }
}
