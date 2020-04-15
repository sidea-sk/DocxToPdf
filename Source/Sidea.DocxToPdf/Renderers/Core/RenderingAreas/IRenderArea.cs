using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderArea : ITextMeasuringService
    {
        XFont AreaFont { get; }
        double Width { get; }
        double Height { get; }
        XRect AreaRectangle { get; }

        void DrawText(string text, XFont font, XBrush brush, XPoint position);
        void DrawLine(XPen pen, XPoint start, XPoint end);

        IRenderArea PanLeft(double x);
        IRenderArea PanLeftDown(XSize size);
    }
}
