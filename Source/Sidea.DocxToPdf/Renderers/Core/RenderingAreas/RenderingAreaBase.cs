using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal abstract class RenderingAreaBase : IRenderingAreaBase
    {
        protected RenderingAreaBase(XFont font, XGraphics graphics, XRect area)
        {
            this.AreaFont = font;
            this.AreaRectangle = area;
        }

        public XFont AreaFont { get; }

        public double Width => this.AreaRectangle.Width;

        public double Height => this.AreaRectangle.Height;

        public XRect AreaRectangle { get; }

        public XSize MeasureText(string text, XFont font)
        {
            throw new NotImplementedException();
        }
    }
}
