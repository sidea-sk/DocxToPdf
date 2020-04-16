using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.Serices;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderingAreaBase : ITextMeasuringService
    {
        XFont AreaFont { get; }
        double Width { get; }
        double Height { get; }
        XRect AreaRectangle { get; }
    }
}
