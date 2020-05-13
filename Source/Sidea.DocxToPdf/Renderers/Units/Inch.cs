using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Inch
    {
        private const double Factor = 72;

        public static XUnit InchToPoint(this double value)
            => value.ToXUnit(Factor);
    }
}
