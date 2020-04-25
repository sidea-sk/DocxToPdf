namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal abstract class RLineElement
    {
        protected RLineElement()
        {
        }

        public abstract bool OmitableAtLineBegin { get; }
        public abstract bool OmitableAtLineEnd { get; }
    }
}
