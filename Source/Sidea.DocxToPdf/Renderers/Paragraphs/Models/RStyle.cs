using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RStyle
    {
        public RStyle(XFont font, XBrush brush)
        {
            this.Font = font;
            this.Brush = brush;
        }

        public XFont Font { get; }
        public XBrush Brush { get; }
    }
}
