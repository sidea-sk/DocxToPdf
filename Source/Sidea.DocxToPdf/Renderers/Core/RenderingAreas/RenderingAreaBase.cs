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

        public XUnit Width => this.AreaRectangle.Width;

        public XUnit Height => this.AreaRectangle.Height;

        public XRect AreaRectangle { get; }

        public XSize MeasureText(string text, XFont font)
        {
            throw new NotImplementedException();
        }
    }
}
