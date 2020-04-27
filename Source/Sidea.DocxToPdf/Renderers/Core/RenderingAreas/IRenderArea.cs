using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.Services;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderArea : IRenderingAreaBase
    {
        RenderingOptions Options { get; }

        void DrawText(string text, XFont font, XBrush brush, XPoint position);
        void DrawText(string text, XFont font, XBrush brush, XRect layout, XStringFormat stringFormat);
        void DrawLine(XPen pen, XPoint start, XPoint end);
        void DrawRectangle(XPen pen, XBrush brush, XRect rect);
        void DrawImage(string imageId, XSize size);

        IRenderArea PanLeft(XUnit width);
        IRenderArea PanDown(XUnit height);
        IRenderArea PanLeftDown(XUnit width, XUnit height);
        IRenderArea PanLeftDown(XSize size);
        IRenderArea Restrict(XUnit width);
        IRenderArea Restrict(XUnit width, XUnit height);
        IRenderArea RestrictFromBottom(XUnit height);
    }
}
