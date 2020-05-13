using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class HalfPoint
    {
        private const double Factor = 2;

        public static XUnit HPToPoint(this int value)
            => value.ToXUnit(Factor);
    }
}
