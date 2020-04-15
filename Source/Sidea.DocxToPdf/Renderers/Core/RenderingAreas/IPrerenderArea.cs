using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IPrerenderArea : ITextMeasuringService
    {
        XFont AreaFont { get; }
        double Width { get; }
        double Height { get; }
        XRect AreaRectangle { get; }

        IPrerenderArea PanLeft(double x);
        IPrerenderArea PanLeftDown(XSize size);
    }
}
