using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal interface IRenderArea : ITextMeasuringService
    {
        XFont AreaFont { get; }
        double Width { get; }
        double Height { get; }
        void DrawText(string text, XFont font, XBrush brush, XPoint position);

        IRenderArea PanLeft(double x);
        IRenderArea PanLeftDown(XSize size);
    }
}
