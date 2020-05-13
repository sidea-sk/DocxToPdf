using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class ToXUnitConvert
    {
        public static XUnit ToXUnit(this Int32Value value, double factor)
        {
            if(value == null || !value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.ToXUnit(factor);
        }

        public static XUnit ToXUnit(this UInt32Value value, double factor)
        {
            if (value == null || !value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.ToXUnit(factor);
        }

        public static XUnit ToXUnit(this uint value, double factor)
        {
            return value / factor;
        }

        public static XUnit ToXUnit(this int value, double factor)
        {
            return value / factor;
        }

        public static XUnit ToXUnit(this double value, double factor)
        {
            return value / factor;
        }
    }
}
