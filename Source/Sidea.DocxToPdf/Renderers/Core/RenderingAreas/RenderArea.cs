using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal class RenderArea :
        IPrerenderArea,
        IRenderArea
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

        public XRect AreaRectangle => _area;

        public void DrawLine(XPen pen, XPoint start, XPoint end)
        {
            _graphics.DrawLine(pen, start + _translate, end + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
            _graphics.DrawString(text, font, brush, position + _translate);
        }

        public XSize MeasureText(string text, XFont font) => _graphics.MeasureString(text, font);

        IRenderArea IRenderArea.PanLeft(XUnit unit) => this.PanLeftCore(unit);

        IRenderArea IRenderArea.PanLeftDown(XSize size) => this.PanLeftDownCore(size);

        IPrerenderArea IPrerenderArea.PanLeft(XUnit unit) => this.PanLeftCore(unit);

        IPrerenderArea IPrerenderArea.PanLeftDown(XSize size) => this.PanLeftDownCore(size);

        IPrerenderArea IPrerenderArea.Restrict(XUnit width) => this.RestricCore(width);

        IRenderArea IRenderArea.Restrict(XUnit width) => this.RestricCore(width);

        private RenderArea PanLeftCore(XUnit unit)
        {
            return new RenderArea(_font, _graphics, new XRect(_area.X + unit, _area.Y, _area.Width - unit, _area.Height));
        }

        private RenderArea PanLeftDownCore(XSize size)
        {
            // check XRect methods Offset, Inflate, etc.
            return new RenderArea(_font, _graphics, new XRect(_area.X + size.Width, _area.Y + size.Height, _area.Width - size.Width, _area.Height - size.Height));
        }

        private RenderArea RestricCore(XUnit width)
        {
            return new RenderArea(
                 _font,
                 _graphics,
                 new XRect(_area.X, _area.Y, width, _area.Height));
        }
    }
}
