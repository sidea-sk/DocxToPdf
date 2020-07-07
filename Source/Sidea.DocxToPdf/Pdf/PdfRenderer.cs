using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Pdf
{
    internal class PdfRenderer : IRenderer
    {
        private readonly PdfDocument _pdfDocument;

        private Dictionary<PageNumber, PdfRendererPage> _pages = new Dictionary<PageNumber, PdfRendererPage>();

        public PdfRenderer(PdfDocument pdfDocument, RenderingOptions renderingOptions)
        {
            _pdfDocument = pdfDocument;
            this.Options = renderingOptions;
        }

        public RenderingOptions Options { get; }

        public void CreatePage(PageNumber pageNumber, PageConfiguration configuration)
        {
            if (_pages.ContainsKey(pageNumber))
            {
                return;
            }

            var pdfPage = new PdfPage
            {
                Orientation = (PdfSharp.PageOrientation)configuration.PageOrientation
            };

            pdfPage.Width = configuration.Size.Width;
            pdfPage.Height = configuration.Size.Height;

            _pdfDocument.AddPage(pdfPage);
            _pages.Add(pageNumber, new PdfRendererPage(pageNumber, XGraphics.FromPdfPage(pdfPage), this.Options));
        }

        public IRendererPage GetPage(PageNumber pageNumber)
            => this.GetPage(pageNumber, Point.Zero);
        
        public IRendererPage GetPage(PageNumber pageNumber, Point offsetRendering)
        {
            var page = _pages[pageNumber];
            return offsetRendering == Point.Zero
                ? page
                : page.Offset(offsetRendering);
        }
    }
}
