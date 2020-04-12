using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class EmptyRenderingArea : IRenderArea
    {
        public double Width => 0;

        public double Height => 0;

        public void DrawText(string text, XFont font, XBrush brush, XPoint position)
        {
        }
    }
}
