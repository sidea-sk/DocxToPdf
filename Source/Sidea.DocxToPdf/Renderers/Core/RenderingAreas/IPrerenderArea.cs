using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core.RenderingAreas
{
    internal interface IPrerenderArea : IRenderingAreaBase
    {
        IPrerenderArea Restrict(XUnit width);
        IPrerenderArea Restrict(XUnit width, XUnit height);
    }
}
