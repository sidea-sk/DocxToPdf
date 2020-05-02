using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Borders
{
    internal class BorderStyle
    {
        private readonly XPen _top;
        private readonly XPen _right;
        private readonly XPen _bottom;
        private readonly XPen _left;

        public BorderStyle(
            XPen top,
            XPen right,
            XPen bottom,
            XPen left)
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
        }
    }
}
