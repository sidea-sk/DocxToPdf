using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderArea
    {
        double Width { get; }
        double Height { get; }
        void DrawText(string text, XFont font, XBrush brush, XPoint position);
    }
}
