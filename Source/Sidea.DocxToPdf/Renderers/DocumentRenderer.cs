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
using Sidea.DocxToPdf.Renderers.Sections.Models;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers
{
    internal class DocumentRenderer
    {
        private readonly WordprocessingDocument _docx;
        private readonly RenderingOptions _renderingOptions;
        private readonly List<IHeaderRenderer> _headerRenderers = new List<IHeaderRenderer>();
        private readonly List<IFooterRenderer> _footerRenderers = new List<IFooterRenderer>();
        private readonly List<SectionRenderer> _sectionRenderers = new List<SectionRenderer>();

        private readonly StyleAccessor _styleAccessor;
        private int _currentPage = 0;

        public DocumentRenderer(WordprocessingDocument docx, RenderingOptions renderingOptions)
        {
            _docx = docx;
            _renderingOptions = renderingOptions;
            _styleAccessor = StyleAccessor.Default(_docx.MainDocumentPart);
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
                if(sectionRenderer.SectionProperties.RenderBehaviour == RenderBehaviour.NewPage)
                {
                    renderArea = this.CreateNewRenderArea(pdf, sectionRenderer.SectionProperties);
                }

                do
                {
                    sectionRenderer.Render(renderArea);
                    renderArea = sectionRenderer.RenderResult.Status != RenderingStatus.Done
                        ? this.CreateNewRenderArea(pdf, sectionRenderer.SectionProperties)
                        : renderArea.PanDown(sectionRenderer.RenderResult.RenderedHeight);
                } while (sectionRenderer.RenderResult.Status != RenderingStatus.Done);
            }
        }

        private IEnumerable<SectionRenderer> PrepareSectionRenderers(PdfDocument pdf)
        {
            var sectionRenderers = _docx.MainDocumentPart.Document.Body
                .SplitToSections(_styleAccessor)
                .Select(sectionData => {
                    var prerenderArea = this.CreatePrerenderArea(pdf, sectionData.Properties.PageConfiguration);

                    var sectionRenderer = new SectionRenderer(sectionData);
                    sectionRenderer.CalculateContentSize(prerenderArea);

                    this.DeletePrerenderPage(pdf);

                    return sectionRenderer;
                })
                .ToArray();

            return sectionRenderers;
        }

        private IRenderArea RenderHeaderAndFooter(RenderArea renderArea, SectionProperties sectionProperties)
        {
            var page = _headerRenderers.Count;

            var headerRenderer = this.CreateAndRenderHeader(renderArea, page + 1, sectionProperties);
            _headerRenderers.Add(headerRenderer);

            var footerRenderer = this.CreateAndRenderFooter(renderArea, page + 1, sectionProperties);
            _footerRenderers.Add(footerRenderer);

            var bodyRenderArea = ((IRenderArea)renderArea)
                .PanDown(headerRenderer.RenderedSize.Height)
                .RestrictFromBottom(footerRenderer.RenderedSize.Height);

            return bodyRenderArea;
        }

        private IHeaderRenderer CreateAndRenderHeader(
            RenderArea renderArea,
            int pageNumber,
            SectionProperties sectionProperties)
        {
            var header = _docx.MainDocumentPart.FindHeaderForPage(pageNumber, sectionProperties.HeaderFooterConfiguration.HasTitlePage);

            var renderer = header == null
                ? (IHeaderRenderer)new NoHeaderRenderer(sectionProperties.PageConfiguration)
                : new HeaderRenderer(header, sectionProperties.PageConfiguration, _styleAccessor);

            renderer.CalculateContentSize(renderArea);
            renderer.Render(renderArea);

            return renderer;
        }

        private IFooterRenderer CreateAndRenderFooter(RenderArea renderArea, int pageNumber, SectionProperties sectionProperties)
        {
            var footer = _docx.MainDocumentPart.FindFooterForPage(pageNumber, sectionProperties.HeaderFooterConfiguration.HasTitlePage);
            var renderer = footer == null
                ? (IFooterRenderer)new NoFooterRenderer(sectionProperties.PageConfiguration)
                : new FooterRenderer(footer, sectionProperties.PageConfiguration, _styleAccessor);

            renderer.CalculateContentSize(renderArea);
            renderer.Render(renderArea);

            return renderer;
        }

        private IPrerenderArea CreatePrerenderArea(PdfDocument pdf, PageConfiguration pageConfiguration)
        {
            var page = this.CreatePage(pdf, pageConfiguration);
            var graphics = XGraphics.FromPdfPage(page);

            return RenderArea.CreateNewPageRenderArea(
                new RenderingContext(0),
                graphics,
                new ImageAccessor(_docx.MainDocumentPart),
                _renderingOptions);
        }

        private IRenderArea CreateNewRenderArea(
            PdfDocument pdf,
            SectionProperties sectionProperties)
        {
            var page = this.CreatePage(pdf, sectionProperties.PageConfiguration);
            var graphics = XGraphics.FromPdfPage(page);
            _currentPage++;

            var renderArea = RenderArea.CreateNewPageRenderArea(
                new RenderingContext(_currentPage),
                graphics,
                new ImageAccessor(_docx.MainDocumentPart),
                _renderingOptions);

            IRenderArea sectionRenderArea = this.RenderHeaderAndFooter(
                renderArea,
                sectionProperties);
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
