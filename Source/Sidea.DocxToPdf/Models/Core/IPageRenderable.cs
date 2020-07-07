using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal interface IPageRenderable
    {
        void Render(IRendererPage page);
    }
}
