using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Common
{
    public class Margin
    {
        public Margin(XUnit top, XUnit right, XUnit bottom, XUnit left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public XUnit Top { get; }
        public XUnit Right { get; }
        public XUnit Bottom { get; }
        public XUnit Left { get; }

        public XUnit HorizontalMargins => this.Left + this.Right;
    }
}
