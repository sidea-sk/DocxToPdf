using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Common
{
    internal class PageMargin : Margin
    {
        public PageMargin(XUnit top, XUnit right, XUnit bottom, XUnit left, XUnit header, XUnit footer) : base(top, right, bottom, left)
        {
            this.Header = header;
            this.Footer = footer;
        }

        public XUnit Header { get; }
        public XUnit Footer { get; }
    }
}
