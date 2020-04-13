using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RenderArea : IRenderArea
    {
        private readonly XFont _font;
        private readonly XGraphics _graphics;
        private readonly XRect _area;
        private readonly XVector _translate;

        public RenderArea(XFont font, XGraphics graphics, XRect area)
        {
            _font = font;
            _graphics = graphics;
            _area = area;
            _translate = new XVector(area.X, area.Y);
        }

        public double Width => _area.Width;

        public double Height => _area.Height;

        public XFont AreaFont => _font;

        public void DrawLine(XPen pen, XPoint start, XPoint end)
        {
            _graphics.DrawLine(pen, start + _translate, end + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
            _graphics.DrawString(text, font, brush, position + _translate);
        }

        public XSize MeasureText(string text, XFont font) => _graphics.MeasureString(text, font);

        public IRenderArea PanLeft(double x)
        {
            // check XRect methods Offset, Inflate, etc.
            return new RenderArea(_font, _graphics, new XRect(_area.X + x, _area.Y, _area.Width - x, _area.Height));
        }

        public IRenderArea PanLeftDown(XSize size)
        {
            // check XRect methods Offset, Inflate, etc.
            return new RenderArea(_font, _graphics, new XRect(_area.X + size.Width, _area.Y + size.Height, _area.Width - size.Width, _area.Height - size.Height));
        }
    }
}
