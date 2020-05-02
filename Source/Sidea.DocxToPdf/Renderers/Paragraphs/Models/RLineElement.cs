using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal abstract class RLineElement: RendererBase
    {
        protected RLineElement()
        {
        }

        public abstract bool OmitableAtLineBegin { get; }

        public abstract bool OmitableAtLineEnd { get; }
    }
}
