using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IPrerenderArea : IRenderingAreaBase
    {
        //IPrerenderArea PanLeft(XUnit unit);
        //IPrerenderArea PanLeftDown(XSize size);
        IPrerenderArea Restrict(XUnit width);
    }
}
