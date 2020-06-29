using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class LineElement : ElementBase
    {
        public abstract void Justify(DocumentPosition position, double baseLineOffset, double lineHeight);

        public abstract double GetBaseLineOffset();
    }
}
