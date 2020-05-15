using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Styles
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
