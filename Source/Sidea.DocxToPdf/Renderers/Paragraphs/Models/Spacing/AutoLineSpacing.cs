using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing
{
    internal class AutoLineSpacing : LineSpacing
    {
        public const long Default = 240; // 1.15 * 240
        public const long Unit = 240;
        private readonly long _lineSpace;

        public AutoLineSpacing() : this(Default)
        {
        }

        public AutoLineSpacing(long lineSpace)
        {
            _lineSpace = lineSpace;
        }

        public override XUnit CalculateSpaceAfterLine(XUnit lineHeight)
        {
            var spaceAfterLine = lineHeight * _lineSpace / Unit - lineHeight;
            return spaceAfterLine;
        }
    }
}
