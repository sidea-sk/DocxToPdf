namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Fields
{
    internal abstract class RField : RLineElement
    {
        public override bool OmitableAtLineBegin => false;

        public override bool OmitableAtLineEnd => false;
    }
}
