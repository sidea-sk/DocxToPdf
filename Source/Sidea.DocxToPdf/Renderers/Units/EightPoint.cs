using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class EightPoint
    {
        private const double Factor = 8;

        public static XUnit EpToXUnit(this UInt32Value value)
            => value.ToXUnit(Factor);

        public static XUnit EpToXUnit(this Int32Value value)
            => value.ToXUnit(Factor);

        public static XUnit EpToPoint(this int value)
            => value.ToXUnit(Factor);

        public static XUnit EpToPoint(this uint value)
            => value.ToXUnit(Factor);
    }
}
