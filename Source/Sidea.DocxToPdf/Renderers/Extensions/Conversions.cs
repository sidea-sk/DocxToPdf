using System;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class Conversions
    {
        private const double PCT = 50;
        private const double DXA = 20;
        private const double IN = 72;
        private const double CM = IN * 2.54d;
        private const double EMU = 914400;

        public static XUnit EmuToXUnit(this Int64Value value)
        {
            return value / EMU * IN ;
        }

        public static XUnit EmuToXUnit(this UInt32Value value)
        {
            // 914400 / 72 / 20
            return XUnit.Zero;
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

        public static XUnit ToXUnit(this StringValue value)
        {
            var v = Convert.ToInt32(value.Value);
            return XUnit.FromPoint(v / 20d);
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
                    return outOf * p / PCT / 100;
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

        public static XUnit DxaToPoint(this uint value)
        {
            return value / DXA;
        }

        public static XUnit DxaToPoint(this int value)
        {
            return value / DXA;
        }

        /// <summary>
        /// HalfPoint to Point
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XUnit HPToPoint(this int value) {
            return value / 2d;
        }

        //private static XUnit TwentiethToUnit(this double value)
        //{
        //    return value / 20d;
        //}
    }
}
