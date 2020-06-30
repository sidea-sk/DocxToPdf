using System.Collections.Generic;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal static class IPageRenderableExtensions
    {
        public static void Render(this IEnumerable<IPageRenderable> elements, IRendererPage renderer)
        {
            foreach (var e in elements)
            {
                e.Render(renderer);
            }
        }
    }
}
