using System.Drawing;
using System.Globalization;
using OpenXml = DocumentFormat.OpenXml;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf
{
    internal static class ColorConversions
    {
        public static Color ToColor(this Word.Highlight highlight)
        {
            var colorName = highlight?.Val ?? Word.HighlightColorValues.None;
            
            if(colorName == Word.HighlightColorValues.None)
            {
                return Color.Empty;
            }

            var c = Color.FromName(colorName.InnerText);
            return c;
        }

        public static Color ToColor(this Word.Color color)
        {
            if(color == null)
            {
                return Color.Black;
            }

            return color.Val.ToColor();
        }

        public static Color ToColor(this OpenXml.StringValue color)
        {
            var hex = color?.Value;
            var result = hex.ToColor();
            return result;
        }

        private static Color ToColor(this string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex == "auto")
            {
                return Color.FromArgb(0, 0, 0);
            }

            var (r, g, b) = hex.ToRgb();
            return Color.FromArgb(r, g, b);
        }

        private static (int r, int g, int b) ToRgb(this string hex)
        {
            var r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            return (r, g, b);
        }
    }
}
