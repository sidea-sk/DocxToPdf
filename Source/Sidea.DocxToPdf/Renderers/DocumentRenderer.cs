using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Renderers.Common;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Core.Services;
using Sidea.DocxToPdf.Renderers.Footers;
using Sidea.DocxToPdf.Renderers.Headers;
using Sidea.DocxToPdf.Renderers.Sections;
using Sidea.DocxToPdf.Renderers.Sections.Builders;

namespace Sidea.DocxToPdf.Renderers
{
    internal class DocumentRenderer
    {
        private readonly WordprocessingDocument _docx;
        private readonly RenderingOptions _renderingOptions;
        private readonly List<IHeaderRenderer> _headerRenderers = new List<IHeaderRenderer>();
        private readonly List<IFooterRenderer> _footerRenderers = new List<IFooterRenderer>();
        private readonly List<SectionRenderer> _sectionRenderers = new List<SectionRenderer>();

        private int _currentPage = 0;

        private XFont _documentFont = new XFont("Calibri", 11, XFontStyle.Regular);

        public DocumentRenderer(WordprocessingDocument docx, RenderingOptions renderingOptions)
        {
            _docx = docx;
            _renderingOptions = renderingOptions;
        }

        public PdfDocument Render()
        {
            _currentPage = 0;

            var pdf = new PdfDocument();
            this.PrepareContent(pdf);
            this.RenderCore(pdf);
            return pdf;
        }

        private void PrepareContent(PdfDocument pdf)
        {
            _headerRenderers.Clear();
            _footerRenderers.Clear();

            _sectionRenderers.Clear();
            _sectionRenderers.AddRange(this.PrepareSectionRenderers(pdf));
        }

        private void RenderCore(PdfDocument pdf)
        {
            IRenderArea renderArea = null;
            foreach (var sectionRenderer in _sectionRenderers)
            {
                if(sectionRenderer.SectionProperties.RenderBehaviour == Sections.Models.RenderBehaviour.NewPage)
                {
                    renderArea = this.CreateNewRenderArea(pdf, sectionRenderer.SectionProperties.PageConfiguration, _documentFont);
                }

                do
                {
                    sectionRenderer.Render(renderArea);
                    renderArea = sectionRenderer.RenderResult.Status != RenderingStatus.Done
                        ? this.CreateNewRenderArea(pdf, sectionRenderer.SectionProperties.PageConfiguration, _documentFont)
                        : renderArea.PanDown(sectionRenderer.RenderResult.RenderedHeight);
                } while (sectionRenderer.RenderResult.Status != RenderingStatus.Done);
            }
        }

        private IEnumerable<SectionRenderer> PrepareSectionRenderers(PdfDocument pdf)
        {
            var sectionRenderers = _docx.MainDocumentPart.Document.Body
                .SplitToSections()
                .Select(sectionData => {
                    var prerenderArea = this.CreatePrerenderArea(pdf, sectionData.Properties.PageConfiguration, _documentFont);

                    var sectionRenderer = new SectionRenderer(sectionData);
                    sectionRenderer.CalculateContentSize(prerenderArea);

                    this.DeletePrerenderPage(pdf);

                    return sectionRenderer;
                })
                .ToArray();

            return sectionRenderers;
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
                : new HeaderRenderer(header, pageMargin);

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
                : new FooterRenderer(footer, pageMargin);

            renderer.CalculateContentSize(renderArea);
            renderer.Render(renderArea);

            return renderer;
        }

        private IPrerenderArea CreatePrerenderArea(PdfDocument pdf, PageConfiguration pageConfiguration, XFont documentDefaultFont)
        {
            var page = this.CreatePage(pdf, pageConfiguration);
            var graphics = XGraphics.FromPdfPage(page);

            return RenderArea.CreateNewPageRenderArea(
                new RenderingContext(0),
                documentDefaultFont,
                graphics,
                new ImageAccessor(_docx.MainDocumentPart),
                _renderingOptions);
        }

        private IRenderArea CreateNewRenderArea(
            PdfDocument pdf,
            PageConfiguration pageConfiguration,
            XFont documentDefaultFont)
        {
            var page = this.CreatePage(pdf, pageConfiguration);
            var graphics = XGraphics.FromPdfPage(page);
            _currentPage++;

            var renderArea = RenderArea.CreateNewPageRenderArea(
                new RenderingContext(_currentPage),
                documentDefaultFont,
                graphics,
                new ImageAccessor(_docx.MainDocumentPart),
                _renderingOptions);

            IRenderArea sectionRenderArea = this.RenderHeaderAndFooter(renderArea);
            return sectionRenderArea;
        }

        private void DeletePrerenderPage(PdfDocument pdf)
        {
            pdf.Pages.RemoveAt(pdf.Pages.Count - 1);
        }

        private PdfPage CreatePage(PdfDocument pdf, PageConfiguration pageConfiguration)
        {
            var page = new PdfPage
            {
                Size = PdfSharp.PageSize.A4,
                Orientation = pageConfiguration.PageOrientation
            };

            pdf.AddPage(page);

            return page;
        }
    }
}
