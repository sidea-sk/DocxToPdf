using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal interface IRenderable
    {
        void Render(IRenderer renderer);
    }
}
