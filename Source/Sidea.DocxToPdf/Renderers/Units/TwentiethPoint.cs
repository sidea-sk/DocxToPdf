using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class TwentiethPoint
    {
        private const double Factor = 20;

        public static XUnit DxaToPoint(this Int32Value value)
            => value.ToXUnit(Factor);

        public static XUnit DxaToPoint(this UInt32Value value)
            => value.ToXUnit(Factor);

        public static XUnit DxaToPoint(this uint value)
            => value.ToXUnit(Factor);

        public static XUnit DxaToPoint(this int value)
            => value.ToXUnit(Factor);

        public static XUnit DxaToPoint(this double value)
            => value.ToXUnit(Factor);
    }
}
