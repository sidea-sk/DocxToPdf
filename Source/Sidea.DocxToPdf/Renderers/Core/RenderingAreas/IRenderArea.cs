using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderArea : IRenderingAreaBase
    {
        void DrawText(string text, XFont font, XBrush brush, XPoint position);
        void DrawLine(XPen pen, XPoint start, XPoint end);

        IRenderArea PanLeft(XUnit unit);
        IRenderArea PanLeftDown(XSize size);
        IRenderArea Restrict(XUnit width);
    }
}
