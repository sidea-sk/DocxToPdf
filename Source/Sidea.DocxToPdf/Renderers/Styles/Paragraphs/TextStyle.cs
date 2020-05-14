using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal class TextStyle
    {
        public static readonly TextStyle Default = new TextStyle(new XFont("Arial", 11), XBrushes.Black);

        public TextStyle(XFont font, XBrush brush)
        {
            this.Font = font;
            this.Brush = brush;
        }

        public XFont Font { get; }
        public XBrush Brush { get; }
    }
}
