﻿using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Pdf
{
    internal class PdfRenderer : IRenderer
    {
        private readonly PdfDocument _pdfDocument;
        private readonly RenderingOptions _renderingOptions;

        private Dictionary<PageNumber, PdfRendererPage> _pages = new Dictionary<PageNumber, PdfRendererPage>();

        public PdfRenderer(PdfDocument pdfDocument, RenderingOptions renderingOptions)
        {
            _pdfDocument = pdfDocument;
            _renderingOptions = renderingOptions;
        }

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
            _pages.Add(pageNumber, new PdfRendererPage(pageNumber, XGraphics.FromPdfPage(pdfPage), _renderingOptions));
        }

        public IRendererPage Get(PageNumber pageNumber)
        {
            return _pages[pageNumber];
        }
    }
}