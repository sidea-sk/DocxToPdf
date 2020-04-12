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

        public RenderingState Render(IRenderArea renderArea)
        {
            var pdf = new PdfDocument();
            var state = this.RenderCore(pdf);
            this.GeneratedDocument = pdf;

            return state;
        }

        private RenderingState RenderCore(PdfDocument pdf)
        {
            var documentFont = new XFont("Calibri", 11, XFontStyle.Regular);
            var childRenderingStatus = new List<RenderingStatus>();
            var currentRenderingArea = this.CreateNewPageRenderingArea(pdf, documentFont);
            foreach (var child in _docx.MainDocumentPart.Document.Body.ChildElements.OfType<OpenXmlCompositeElement>())
            {
                var activeRenderer = _factory.CreateRenderer(child);

                RenderingState renderingState = new RenderingState(RenderingStatus.NotStarted, new XPoint(0, 0));
                while(renderingState.Status.IsNotFinished())
                {
                    renderingState = activeRenderer.Render(currentRenderingArea);
                    switch(renderingState.Status)
                    {
                        case RenderingStatus.ReachedEndOfArea:
                            currentRenderingArea = this.CreateNewPageRenderingArea(pdf, documentFont);
                            break;
                        case RenderingStatus.NotStarted:
                            throw new System.Exception("Unexpected rendering status");
                        default:
                            currentRenderingArea = currentRenderingArea.PanLeftDown(new XSize(0, renderingState.FinishedAtPosition.Y));
                            break;
                    }
                }

                childRenderingStatus.Add(renderingState.Status);
            }

            
            var aggregatedStatus = childRenderingStatus.All(rs => rs == RenderingStatus.Done)
                ? RenderingStatus.Done
                : RenderingStatus.Error;

            return new RenderingState(aggregatedStatus, new XPoint(0, 0));
        }

        private IRenderArea CreateNewPageRenderingArea(PdfDocument pdf, XFont documentDefaultFont)
        {
            var page = this.CreatePage(pdf);
            var graphics = XGraphics.FromPdfPage(page);

            // render Header
            // render Footer
            var margin = XUnit.FromCentimeter(2.5);
            var contentArea = new XRect(margin, margin, page.Width - 2 * margin, page.Height - 2 * margin);
            graphics.DrawRectangle(XPens.Orange, contentArea);
            return new RenderArea(documentDefaultFont, graphics, contentArea);
        }

        private PdfPage CreatePage(PdfDocument pdf)
        {
            var page = new PdfPage
            {
                Size = PdfSharp.PageSize.A4,
            };

            pdf.AddPage(page);

           

            return page;
        }
    }
}
