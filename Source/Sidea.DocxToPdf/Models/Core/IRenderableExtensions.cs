using System.Collections.Generic;
using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models
{
    internal static class IRenderableExtensions
    {
        public static void Render(this IEnumerable<IRenderable> elements, IRenderer renderer)
        {
            foreach (var e in elements)
            {
                e.Render(renderer);
            }
        }
    }
}
