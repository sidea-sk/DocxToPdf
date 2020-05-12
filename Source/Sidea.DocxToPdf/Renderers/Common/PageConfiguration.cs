using PdfSharp;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Common
{
    internal class PageConfiguration
    {
        public PageConfiguration(
            XSize size,
            PageMargin margin,
            PageOrientation pageOrientation)
        {
            this.Size = size;
            this.Margin = margin;
            this.PageOrientation = pageOrientation;
        }

        public XSize Size { get; }
        public PageMargin Margin { get; }

        public XUnit Width => this.Size.Width;
        public XUnit Height => this.Size.Height;

        public PageOrientation PageOrientation { get; }
    }
}
