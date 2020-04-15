using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Borders
{
    internal class RBorder
    {
        private readonly XPen _top;
        private readonly XPen _right;
        private readonly XPen _bottom;
        private readonly XPen _left;

        public RBorder(
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

        public RenderingState Render(IRenderArea renderArea)
        {
            var rect = renderArea.AreaRectangle;

            renderArea.DrawLine(_top, rect.TopLeft, rect.TopRight);
            renderArea.DrawLine(_right, rect.TopRight, rect.BottomRight);
            renderArea.DrawLine(_bottom, rect.BottomRight, rect.BottomLeft);
            renderArea.DrawLine(_left, rect.BottomLeft, rect.TopLeft);

            return new RenderingState(RenderingStatus.Done, new XPoint(rect.X, rect.Y));
        }

        public void Render(IRenderArea renderArea, XRect rect)
        {
            renderArea.DrawLine(_top, rect.TopLeft, rect.TopRight);
            renderArea.DrawLine(_right, rect.TopRight, rect.BottomRight);
            renderArea.DrawLine(_bottom, rect.BottomRight, rect.BottomLeft);
            renderArea.DrawLine(_left, rect.BottomLeft, rect.TopLeft);
        }
    }
}
