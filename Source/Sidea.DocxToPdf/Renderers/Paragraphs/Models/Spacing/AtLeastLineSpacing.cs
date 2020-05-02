using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing
{
    internal class AtLeastLineSpacing : LineSpacing
    {
        private readonly XUnit _lineSpace;

        public AtLeastLineSpacing(XUnit lineSpace)
        {
            _lineSpace = lineSpace;
        }

        public override XUnit CalculateSpaceAfterLine(XUnit lineHeight)
        {
            return Math.Max(_lineSpace, lineHeight);
        }
    }
}
