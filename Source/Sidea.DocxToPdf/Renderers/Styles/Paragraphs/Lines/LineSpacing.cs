using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal abstract class LineSpacing
    {
        public abstract XUnit CalculateSpaceAfterLine(XUnit lineHeight);
    }
}