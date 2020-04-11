using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Sidea.DocxToPdf.Renderers.Documents
{
    internal class DocumentRenderer : IRenderer
    {
        private readonly WordprocessingDocument _docx;

        public DocumentRenderer(WordprocessingDocument docx)
        {
            _docx = docx;
        }

        private void RenderCore(PdfDocument pdf)
        {
            var currentPage = this.CreatePage(pdf);
            foreach (var child in _docx.MainDocumentPart.Document.Body)
            {
            }
        }

        private PdfPage CreatePage(PdfDocument pdf)
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

            pdf.AddPage(page);

            var graphics = XGraphics.FromPdfPage(page);
            graphics.DrawRectangle(XPens.Orange, new XRect(0,0, page.Width, page.Height));

            return page;
        }

        public PdfDocument GeneratedDocument { get; private set; }

        public RenderingStatus Render()
        {
            var pdf = new PdfDocument();
            this.RenderCore(pdf);
            this.GeneratedDocument = pdf;

            return RenderingStatus.Finished;
        }
    }
}
