using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing
{
    internal class ExactLineSpacing : LineSpacing
    {
        private readonly XUnit _lineSpace;

        public ExactLineSpacing(XUnit lineSpace)
        {
            _lineSpace = lineSpace;
        }

        public override XUnit CalculateSpaceAfterLine(XUnit lineHeight) => _lineSpace;
    }
}
