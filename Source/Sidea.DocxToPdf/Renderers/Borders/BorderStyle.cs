using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Borders
{
    internal class BorderStyle
    {
        protected static readonly XPen DefaultPen = new XPen(XColors.Black, 4.EpToPoint());

        public static readonly BorderStyle Default = new BorderStyle(DefaultPen);

        public BorderStyle(XPen all) : this(all, all, all, all)
        {
        }

        public BorderStyle(XPen top, XPen right, XPen bottom, XPen left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public XPen Top { get; }
        public XPen Right { get; }
        public XPen Bottom { get; }
        public XPen Left { get; }
    }
}
