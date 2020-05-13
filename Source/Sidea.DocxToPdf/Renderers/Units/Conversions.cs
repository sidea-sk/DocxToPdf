using System;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Conversions
    {
        private const double HalfPoint = 2;
        private const double DXA = 20;
        private const double PERCENT = 50;
        private const double INCH = 72;
        private const double CM = INCH * 2.54d;
        private const double EMU = 914400;

        private static readonly string[] _units = { "mm", "cm", "in", "pt", "pc", "pi" };

        public static XUnit EmuToXUnit(this Int64Value value)
        {
            return value.Value.EmuToXUnit();
        }

        public static XUnit EmuToXUnit(this long value)
        {
            return value / EMU * INCH;
        }

        public static XUnit ToXUnit(this UInt32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.DxaToPoint();
        }

        public static XUnit ToXUnit(this Int32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.DxaToPoint();
        }

        public static XUnit ToXUnit(this StringValue value, double ifNull = 0)
        {
            if(value == null)
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
                    return  v.DxaToPoint();
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

        public static XUnit ToXUnit(this TableWidthType width) => width.ToXUnit(XUnit.Zero);

        public static XUnit ToXUnit(this TableWidthType width, XUnit outOf)
        {
            // Nil = 0,
            // Summary:
            //     Width in Fiftieths of a Percent.
            //     When the item is serialized out as xml, its value is "pct".
            // Pct = 1,
            // Summary:
            //     
            //     When the item is serialized out as xml, its value is "dxa".
            // Dxa = 2,
            // Summary:
            //     Automatically Determined Width.
            //     When the item is serialized out as xml, its value is "auto".
            // Auto = 3

            switch (width.Type.Value)
            {
                case TableWidthUnitValues.Nil:
                    break;
                case TableWidthUnitValues.Pct:
                    var p = Convert.ToInt32(width.Width.Value);
                    return outOf * p / PERCENT / 100;
                case TableWidthUnitValues.Dxa:
                    var v = Convert.ToInt32(width.Width.Value);
                    return v.DxaToPoint();
                case TableWidthUnitValues.Auto:
                    break;
                default:
                    break;
            }

            return XUnit.FromPoint(0);
        }

        public static XBrush ToXBrush(this Color color)
        {
            var hex = color?.Val?.Value ?? "000000";
            var r = Convert.ToInt32(hex.Substring(0, 2), 16);
            var g = Convert.ToInt32(hex.Substring(2, 2), 16);
            var b = Convert.ToInt32(hex.Substring(4, 2), 16);

            XBrush brush = new XSolidBrush(XColor.FromArgb(r, g, b));
            return brush;
        }

        public static XUnit InchToPoint(this double value)
        {
            return value / INCH;
        }

        public static XUnit DxaToPoint(this Int32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.DxaToPoint();
        }

        public static XUnit DxaToPoint(this UInt32Value value)
        {
            if (!value.HasValue)
            {
                return XUnit.Zero;
            }

            return value.Value.DxaToPoint();
        }

        public static XUnit DxaToPoint(this uint value)
        {
            return value / DXA;
        }

        public static XUnit DxaToPoint(this int value)
        {
            return value / DXA;
        }

        public static XUnit DxaToPoint(this double value)
        {
            return value / DXA;
        }

        /// <summary>
        /// HalfPoint to Point
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XUnit HPToPoint(this int value) {
            return value / HalfPoint;
        }

        public static XUnit ToXUnit(this PositionOffset positionOffset)
        {
            var offset = Convert.ToInt64(positionOffset.Text);
            return offset.EmuToXUnit();
        }

        public static XColor ToXColor(this StringValue color)
        {
            var hex = color?.Value;
            if (hex == null || hex == "auto")
            {
                return XColor.FromArgb(255, 0, 0, 0);
            }

            var r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(0, 2)}");
            var g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(2, 2)}");
            var b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);// Convert.ToInt32($"0x{color.Value.Substring(4, 2)}");

            return XColor.FromArgb(255, r, g, b);
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
