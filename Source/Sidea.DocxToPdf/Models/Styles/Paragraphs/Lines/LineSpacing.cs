using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Models.Styles
{
    internal abstract class LineSpacing
    {
        public abstract double CalculateSpaceAfterLine(double lineHeight);
    }
}