using PdfSharp.Pdf;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Pdf
{
    internal class PdfRendererPage : IRendererPage
    {
        private readonly PdfPage _page;

        public PdfRendererPage(PageNumber pageNumber, PdfPage page, RenderingOptions options)
        {
            _page = page;
            this.PageNumber = pageNumber;
            this.Options = options;
        }

        public PageNumber PageNumber { get; }
        public RenderingOptions Options { get; }
    }
}
