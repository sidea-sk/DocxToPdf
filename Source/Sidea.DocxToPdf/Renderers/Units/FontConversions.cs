using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class FontConversions
    {
        public static XFontStyle BoldStyle(this Bold bold, XFontStyle defaultFontStyle)
        {
            return bold.OnOffTypeToStyle(XFontStyle.Bold, defaultFontStyle & XFontStyle.Bold);
        }

        public static XFontStyle ItalicStyle(this Italic italic, XFontStyle defaultFontStyle)
        {
            return italic.OnOffTypeToStyle(XFontStyle.Italic, defaultFontStyle & XFontStyle.Italic);
        }

        public static XFontStyle StrikeStyle(this Strike strike, XFontStyle defaultFontStyle)
        {
            return strike.OnOffTypeToStyle(XFontStyle.Strikeout, defaultFontStyle & XFontStyle.Strikeout);
        }

        public static XFontStyle UnderlineStyle(this Underline underline, XFontStyle defaultFontStyle)
        {
            if(underline?.Val == null)
            {
                return defaultFontStyle & XFontStyle.Underline;
            }

            return underline.Val.Value != UnderlineValues.None
                ? XFontStyle.Underline
                : XFontStyle.Regular;
        }

        public static XUnit ToXUnit(this FontSize fontSize, XUnit ifNull)
        {
            if(fontSize?.Val == null)
            {
                return ifNull;
            }

            var size = fontSize.Val.HPToPoint(ifNull);
            return size;
        }

        private static XFontStyle OnOffTypeToStyle(this OnOffType onOff, XFontStyle onValue, XFontStyle nullValue)
        {
            if(onOff == null)
            {
                return nullValue;
            }

            return (onOff.Val?.Value ?? true)
                ? onValue
                : XFontStyle.Regular;
        }
    }
}
