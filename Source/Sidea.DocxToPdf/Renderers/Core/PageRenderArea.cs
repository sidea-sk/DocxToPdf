using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    public class PageRenderArea : IRenderArea
    {
        private readonly XGraphics _graphics;
        private readonly XRect _area;
        private readonly XVector _translate;

        public PageRenderArea(XGraphics graphics, XRect area)
        {
            _graphics = graphics;
            _area = area;
            _translate = new XVector(area.X, area.Y);
        }

        public double Width => _area.Width;

        public double Height => _area.Height;

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
            _graphics.DrawString(text, font, brush, position + _translate);
        }
    }
}
