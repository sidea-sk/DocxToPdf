using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class CellBorderStyle
    {
        public static readonly CellBorderStyle Inherit = new CellBorderStyle(null);

        public CellBorderStyle(XPen all) : this(all, all, all, all)
        {
        }

        public CellBorderStyle(XPen top, XPen right, XPen bottom, XPen left)
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
