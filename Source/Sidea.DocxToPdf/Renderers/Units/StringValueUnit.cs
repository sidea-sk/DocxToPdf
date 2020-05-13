using System;
using System.Linq;
using DocumentFormat.OpenXml;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class StringValueUnit
    {
        private static readonly string[] _units = { "mm", "cm", "in", "pt", "pc", "pi" };

        public static XUnit ToXUnit(this StringValue value, double ifNull = 0)
        {
            if (value == null)
            {
                return new XUnit(ifNull);
            }

            var (v, u) = value.ToValueWithUnit();
            switch (u)
            {
                case "mm":
                    return XUnit.FromMillimeter(v);
                case "cm":
                    return XUnit.FromCentimeter(v);
                case "in":
                    return v.InchToPoint();
                case "pt":
                    return v.DxaToPoint();
                case "pi":
                    return XUnit.FromPresentation(v);
                case "pc":
                default:
                    throw new Exception($"Unhandled string value: {value}");
            }
        }

        public static long ToLong(this StringValue value)
        {
            return Convert.ToInt64(value.Value);
        }

        private static (double v, string unit) ToValueWithUnit(this StringValue stringValue)
        {
            var l = stringValue.Value.Length > 2
                ? stringValue.Value.Length - 2
                : 0;

            var u = stringValue.Value.Substring(l);

            if (!_units.Contains(u))
            {
                l = stringValue.Value.Length;
                u = "pt";
            }

            var v = stringValue.Value.Substring(0, l);
            return (Convert.ToDouble(v), u);
        }
    }
}
