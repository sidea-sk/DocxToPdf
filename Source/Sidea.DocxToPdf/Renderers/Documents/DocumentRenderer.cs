using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Documents
{
    internal class DocumentRenderer : IRenderer
    {
        private readonly WordprocessingDocument _docx;
        private readonly RendererFactory _factory = new RendererFactory();

        public DocumentRenderer(WordprocessingDocument docx)
        {
            _docx = docx;
        }

        public PdfDocument GeneratedDocument { get; private set; }

        public RenderingStatus Render()
        {
            var pdf = new PdfDocument();
            this.RenderCore(pdf);
            this.GeneratedDocument = pdf;

            return RenderingStatus.Done;
        }

        private RenderingStatus RenderCore(PdfDocument pdf)
        {
            var childRenderingStatus = new List<RenderingStatus>();
            var currentPage = this.CreatePage(pdf);
            foreach (var child in _docx.MainDocumentPart.Document.Body.ChildElements.OfType<OpenXmlCompositeElement>())
            {
                var activeRenderer = _factory.CreateRenderer(child);

                RenderingStatus renderingStatus;
                do
                {
                    renderingStatus = activeRenderer.Render();
                    switch(renderingStatus)
                    {
                        case RenderingStatus.ReachedEndOfArea:
                            // create new page
                            break;
                        case RenderingStatus.NotStarted:
                            throw new System.Exception("Unexpected rendering status");
                    }
                } while (renderingStatus.IsFinished());

                childRenderingStatus.Add(renderingStatus);
            }

            return childRenderingStatus.All(rs => rs == RenderingStatus.Done)
                ? RenderingStatus.Done
                : RenderingStatus.Error;
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
            graphics.DrawRectangle(XPens.Orange, new XRect(0, 0, page.Width, page.Height));

            // render Header
            // render Footer

            return page;
        }
    }
}
