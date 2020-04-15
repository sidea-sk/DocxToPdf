using PdfSharp.Drawing;

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
