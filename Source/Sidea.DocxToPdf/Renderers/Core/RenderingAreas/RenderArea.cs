using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal class RenderArea :
        IPrerenderArea,
        IRenderArea
    {
        private readonly XGraphics _graphics;
        private readonly XVector _translate;

        public RenderArea(XFont font, XGraphics graphics, XRect area)
        {
            AreaFont = font;
            _graphics = graphics;
            AreaRectangle = area;
            _translate = new XVector(area.X, area.Y);
        }

        public XUnit Width => AreaRectangle.Width;

        public XUnit Height => AreaRectangle.Height;

        public XFont AreaFont { get; }

        public XRect AreaRectangle { get; }

        public void DrawLine(XPen pen, XPoint start, XPoint end)
        {
            _graphics.DrawLine(pen, start + _translate, end + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
            _graphics.DrawString(text, font, brush, position + _translate);
        }

        public void DrawText(string text, XFont font, XBrush brush, XRect layout, XStringFormat stringFormat)
        {
            _graphics.DrawString(text, font, brush, XRect.Offset(layout, _translate), stringFormat);
        }

        public void DrawRectangle(XPen pen, XBrush brush, XRect rect)
        {
            _graphics.DrawRectangle(pen, brush, XRect.Offset(rect, _translate));
        }

        public XSize MeasureText(string text, XFont font) => _graphics.MeasureString(text, font);

        IRenderArea IRenderArea.PanLeft(XUnit unit) => this.PanLeftCore(unit);

        IRenderArea IRenderArea.PanDown(XUnit height) => this.PanLeftDownCore(new XSize(0, height));

        IRenderArea IRenderArea.PanLeftDown(XUnit width, XUnit height) => this.PanLeftDownCore(new XSize(width, height));

        IRenderArea IRenderArea.PanLeftDown(XSize size) => this.PanLeftDownCore(size);

        IPrerenderArea IPrerenderArea.Restrict(XUnit width) => this.RestricCore(width);

        IRenderArea IRenderArea.Restrict(XUnit width) => this.RestricCore(width);

        private RenderArea PanLeftCore(XUnit unit)
        {
            return new RenderArea(AreaFont, _graphics, new XRect(AreaRectangle.X + unit, AreaRectangle.Y, AreaRectangle.Width - unit, AreaRectangle.Height));
        }

        private RenderArea PanLeftDownCore(XSize size)
        {
            // check XRect methods Offset, Inflate, etc.
            return new RenderArea(AreaFont, _graphics, new XRect(AreaRectangle.X + size.Width, AreaRectangle.Y + size.Height, AreaRectangle.Width - size.Width, AreaRectangle.Height - size.Height));
        }

        private RenderArea RestricCore(XUnit width)
        {
            return new RenderArea(
                 AreaFont,
                 _graphics,
                 new XRect(AreaRectangle.X, AreaRectangle.Y, width, AreaRectangle.Height));
        }
    }
}
