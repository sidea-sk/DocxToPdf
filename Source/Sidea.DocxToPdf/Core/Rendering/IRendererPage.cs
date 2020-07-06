using System.IO;
using Drawing = System.Drawing;

namespace Sidea.DocxToPdf.Core
{
    internal interface IRendererPage
    {
        RenderingOptions Options { get; }

        void RenderText(string text, TextStyle textStyle, Rectangle layout);
        void RenderRectangle(Rectangle rectangle, Drawing.Color brush);
        void RenderLine(Line line);
        void RenderImage(Stream imageStream, Point position, Size size);

        IRendererPage Offset(Point vector);
    }
}
