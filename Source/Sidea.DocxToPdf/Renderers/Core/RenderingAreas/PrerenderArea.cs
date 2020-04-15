using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal class PrerenderArea : IRenderArea
    {
        public XFont AreaFont => throw new NotImplementedException();

        public double Width => throw new NotImplementedException();

        public double Height => throw new NotImplementedException();

        public XRect AreaRectangle => throw new NotImplementedException();

        public void DrawLine(XPen pen, XPoint start, XPoint end)
        {
        }

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
        }

        public XSize MeasureText(string text, XFont font)
        {
            throw new NotImplementedException();
        }

        public IRenderArea PanLeft(double x)
        {
            throw new NotImplementedException();
        }

        public IRenderArea PanLeftDown(XSize size)
        {
            throw new NotImplementedException();
        }
    }
}
