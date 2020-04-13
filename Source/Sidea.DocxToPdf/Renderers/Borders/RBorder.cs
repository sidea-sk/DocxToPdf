using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Borders
{
    internal class RBorder : IRenderer
    {
        private readonly XPen _top;
        private readonly XPen _right;
        private readonly XPen _bottom;
        private readonly XPen _left;
        private readonly XRect _rect;

        public RBorder(
            XPen top,
            XPen right,
            XPen bottom,
            XPen left,
            XRect rect)
        {
            _top = top;
            _right = right;
            _bottom = bottom;
            _left = left;
            _rect = rect;
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            renderArea.DrawLine(_top, _rect.TopLeft, _rect.TopRight);
            renderArea.DrawLine(_right, _rect.TopRight, _rect.BottomRight);
            renderArea.DrawLine(_bottom, _rect.BottomRight, _rect.BottomLeft);
            renderArea.DrawLine(_left, _rect.BottomLeft, _rect.TopLeft);

            return new RenderingState(RenderingStatus.Done, new XPoint(_rect.X, _rect.Y));
        }
    }
}
