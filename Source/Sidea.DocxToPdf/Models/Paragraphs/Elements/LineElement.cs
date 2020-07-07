using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class LineElement : ParagraphElementBase
    {
        public abstract void Justify(DocumentPosition position, double baseLineOffset, Size lineSpace);

        public abstract double GetBaseLineOffset();
    }
}
