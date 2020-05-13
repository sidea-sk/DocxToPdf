using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class EightPoint
    {
        private const double Factor = 8;

        public static XUnit EpToXUnit(this UInt32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.EpToPoint();
        }

        public static XUnit EpToXUnit(this Int32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.EpToPoint();
        }

        public static XUnit EpToPoint(this int value)
        {
            return value / Factor;
        }

        public static XUnit EpToPoint(this uint value)
        {
            return value / Factor;
        }
    }
}
