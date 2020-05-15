using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.Services;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IRenderingAreaBase :
        ITextMeasuringService
    {
        RenderingOptions Options { get; }

        XUnit Width { get; }
        XUnit Height { get; }
        XRect AreaRectangle { get; }
    }
}
