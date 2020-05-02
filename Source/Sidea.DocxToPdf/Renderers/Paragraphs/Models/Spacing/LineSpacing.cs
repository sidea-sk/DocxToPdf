using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Spacing
{
    internal abstract class LineSpacing
    {
        public XUnit CalculateSpaceAfterLine(RLine line) => this.CalculateSpaceAfterLine(line.PrecalulatedSize.Height);

        public abstract XUnit CalculateSpaceAfterLine(XUnit lineHeight);
    }
}