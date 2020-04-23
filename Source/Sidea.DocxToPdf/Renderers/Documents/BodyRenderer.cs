using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Documents
{
    internal class BodyRenderer : CompositeRenderer
    {
        public BodyRenderer(Body body, RenderingOptions renderingOptions) : base(body, renderingOptions)
        {
        }
    }
}
