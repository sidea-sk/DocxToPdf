using System.Collections.Generic;
using System.Drawing;
using Sidea.DocxToPdf.Core;
using Draw = DocumentFormat.OpenXml.Drawing;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Styles
{
    internal static class TextStyleExtensions
    {
        public static TextStyle Override(this TextStyle baseStyle, Word.RunProperties runProperties, IReadOnlyCollection<Word.StyleRunProperties> styleRuns)
        {
            if (runProperties == null && styleRuns.Count == 0)
            {
                return baseStyle;
            }

            var font = baseStyle.Font.Override(runProperties, styleRuns);
            var brush = runProperties.EffectiveColor(styleRuns, baseStyle.Brush);
            var background = runProperties?.Highlight.ToColor();

            return baseStyle.WithChanged(font: font, brush: brush, background: background);
        }

        public static TextStyle CreateTextStyle(this Word.RunPropertiesDefault runPropertiesDefault, Draw.Theme theme)
        {
            var typeFace = runPropertiesDefault.GetTypeFace(theme);
            var fontStyle = runPropertiesDefault.RunPropertiesBaseStyle.EffectiveFontStyle();
            var size = runPropertiesDefault.RunPropertiesBaseStyle.FontSize.ToDouble(11);
            var brush = runPropertiesDefault.RunPropertiesBaseStyle.Color.ToColor();

            var font = new Font(typeFace, (float)size, fontStyle);
            return new TextStyle(font, brush, Color.Empty);
        }

        private static string GetTypeFace(this Word.RunPropertiesDefault runPropertiesDefault, Draw.Theme theme)
        {
            return runPropertiesDefault.RunPropertiesBaseStyle.RunFonts.Ascii
                ?? theme.ThemeElements.FontScheme.MinorFont.LatinFont.Typeface;
        }

        private static Font Override(this Font font, Word.RunProperties runProperties, IReadOnlyCollection<Word.StyleRunProperties> styleRuns)
        {
            var typeFace = runProperties.EffectiveTypeFace(styleRuns, font.FontFamily.Name);
            var size = runProperties.EffectiveFontSize(styleRuns, font.Size);
            var fontStyle = runProperties.EffectiveFontStyle(styleRuns, font.Style);

            return new Font(typeFace, size, fontStyle);
        }
    }
}
