using System.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf
{
    internal static class FontConversions
    {
        public static FontStyle BoldStyle(this Bold bold, FontStyle defaultFontStyle)
        {
            return bold.OnOffTypeToStyle(FontStyle.Bold, defaultFontStyle & FontStyle.Bold);
        }

        public static FontStyle ItalicStyle(this Italic italic, FontStyle defaultFontStyle)
        {
            return italic.OnOffTypeToStyle(FontStyle.Italic, defaultFontStyle & FontStyle.Italic);
        }

        public static FontStyle StrikeStyle(this Strike strike, FontStyle defaultFontStyle)
        {
            return strike.OnOffTypeToStyle(FontStyle.Strikeout, defaultFontStyle & FontStyle.Strikeout);
        }

        public static FontStyle UnderlineStyle(this Underline underline, FontStyle defaultFontStyle)
        {
            if(underline?.Val == null)
            {
                return defaultFontStyle & FontStyle.Underline;
            }

            return underline.Val.Value != UnderlineValues.None
                ? FontStyle.Underline
                : FontStyle.Regular;
        }

        public static double ToDouble(this FontSize fontSize, double ifNull)
        {
            if(fontSize?.Val == null)
            {
                return ifNull;
            }

            var size = fontSize.Val.HPToPoint(ifNull);
            return size;
        }

        public static float ToFloat(this FontSize fontSize, float ifNull)
        {
            var size = fontSize.ToDouble(ifNull);
            return (float)size;
        }

        private static FontStyle OnOffTypeToStyle(this OnOffType onOff, FontStyle onValue, FontStyle nullValue)
        {
            if(onOff == null)
            {
                return nullValue;
            }

            return (onOff.Val?.Value ?? true)
                ? onValue
                : FontStyle.Regular;
        }
    }
}
