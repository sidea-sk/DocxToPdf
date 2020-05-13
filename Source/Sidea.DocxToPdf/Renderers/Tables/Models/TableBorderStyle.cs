using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Tables.Models
{
    internal class TableBorderStyle
    {
        private static readonly XPen _defaultPen = new XPen(XColors.Black, 4.EpToPoint());

        public static readonly TableBorderStyle Default = new TableBorderStyle(_defaultPen);

        public TableBorderStyle(XPen all) : this(all, all, all, all, all, all)
        {
        }

        public TableBorderStyle(XPen top, XPen right, XPen bottom, XPen left, XPen insideHorizontal, XPen insideVertical)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
            this.InsideHorizontal = insideHorizontal;
            this.InsideVertical = insideVertical;
        }

        public XPen Top { get; }
        public XPen Right { get; }
        public XPen Bottom { get; }
        public XPen Left { get; }
        public XPen InsideHorizontal { get; }
        public XPen InsideVertical { get; }
    }
}
