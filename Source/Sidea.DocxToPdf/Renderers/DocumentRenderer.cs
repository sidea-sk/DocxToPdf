using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers.Bodies;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Footers;
using Sidea.DocxToPdf.Renderers.Headers;

namespace Sidea.DocxToPdf.Renderers
{
    internal class DocumentRenderer
    {
        private readonly WordprocessingDocument _docx;
        private readonly RenderingOptions _renderingOptions;
        private readonly List<IHeaderRenderer> _headerRenderers = new List<IHeaderRenderer>();
        private readonly List<IFooterRenderer> _footerRenderers = new List<IFooterRenderer>();
        private BodyRenderer _bodyRenderer;

        private XFont _documentFont = new XFont("Calibri", 11, XFontStyle.Regular);

        public DocumentRenderer(WordprocessingDocument docx, RenderingOptions renderingOptions)
        {
            _docx = docx;
            _renderingOptions = renderingOptions;
        }

        public PdfDocument Render()
        {
            var pdf = new PdfDocument();
            this.PrepareContent(pdf);
            this.RenderCore(pdf);
            return pdf;
        }

        private void PrepareContent(PdfDocument pdf)
        {
            var prerenderPage = this.CreateNewPagePrerenderArea(pdf, _documentFont);
            var prerenderArea = prerenderPage;

            _headerRenderers.Clear();

            _bodyRenderer = new BodyRenderer(_docx.MainDocumentPart.Document.Body, _renderingOptions);
            _bodyRenderer.CalculateContentSize(prerenderArea);

            this.DeletePrerenderPage(pdf);
        }

        private void RenderCore(PdfDocument pdf)
        {
            var currentRenderingArea = this.PrepareNewPageRenderingArea(pdf, _documentFont);

            while(_bodyRenderer.CurrentRenderingState.Status != RenderingStatus.Done)
            {
                _bodyRenderer.Render(currentRenderingArea);

                if(_bodyRenderer.CurrentRenderingState.Status == RenderingStatus.ReachedEndOfArea)
                {
                    currentRenderingArea = this.PrepareNewPageRenderingArea(pdf, _documentFont);
                }
            }
        }

        private IRenderArea PrepareNewPageRenderingArea(PdfDocument pdf, XFont documentDefaultFont)
        {
            var page = this.CreatePage(pdf);
            var graphics = XGraphics.FromPdfPage(page);

            var pageMargin = _docx.MainDocumentPart.GetPageMargin();
            var leftMargin = pageMargin.Left.ToXUnit();
            var rightMargin = pageMargin.Right.ToXUnit();
            var renderArea = new RenderArea(documentDefaultFont, graphics, new XRect(leftMargin, 0, page.Width - leftMargin - rightMargin, page.Height));

            IRenderArea bodyRenderArea = this.RenderHeaderAndFooter(renderArea);
            graphics.DrawRectangle(XPens.Orange, bodyRenderArea.AreaRectangle);
            return bodyRenderArea;
        }

        private IRenderArea RenderHeaderAndFooter(RenderArea renderArea)
        {
            var page = _headerRenderers.Count;

            var headerRenderer = this.CreateAndRenderHeader(renderArea, page + 1);
            _headerRenderers.Add(headerRenderer);

            var footerRenderer = this.CreateAndRenderFooter(renderArea, page + 1);
            _footerRenderers.Add(footerRenderer);

            var bodyRenderArea = ((IRenderArea)renderArea)
                .PanDown(headerRenderer.RenderedSize.Height)
                .RestrictFromBottom(footerRenderer.RenderedSize.Height);

            return bodyRenderArea;
        }

        private IHeaderRenderer CreateAndRenderHeader(RenderArea renderArea, int pageNumber)
        {
            var header = _docx.MainDocumentPart.FindHeaderForPage(pageNumber);
            var pageMargin = _docx.MainDocumentPart.GetPageMargin();

            var renderer = header == null
                ? (IHeaderRenderer)new NoHeaderRenderer(pageMargin)
                : new HeaderRenderer(header, pageMargin, _renderingOptions);

            renderer.CalculateContentSize(renderArea);
            renderer.Render(renderArea);

            return renderer;
        }

        private IFooterRenderer CreateAndRenderFooter(RenderArea renderArea, int pageNumber)
        {
            var footer = _docx.MainDocumentPart.FindFooterForPage(pageNumber);
            var pageMargin = _docx.MainDocumentPart.GetPageMargin();

            var renderer = footer == null
                ? (IFooterRenderer)new NoFooterRenderer(pageMargin)
                : new FooterRenderer(footer, pageMargin, _renderingOptions);

            renderer.CalculateContentSize(renderArea);
            renderer.Render(renderArea);

            return renderer;
        }

        private IPrerenderArea CreateNewPagePrerenderArea(PdfDocument pdf, XFont documentDefaultFont)
        {
            var page = pdf.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var margin = XUnit.FromCentimeter(2.5);
            var contentArea = new XRect(margin, margin, page.Width - 2 * margin, page.Height - 2 * margin);
            return new RenderArea(documentDefaultFont, graphics, contentArea);
        }

        private void DeletePrerenderPage(PdfDocument pdf)
        {
            pdf.Pages.RemoveAt(pdf.Pages.Count - 1);
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
