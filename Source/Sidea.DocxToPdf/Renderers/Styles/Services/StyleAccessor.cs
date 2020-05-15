using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal class StyleAccessor : IStyleAccessor
    {
        private readonly MainDocumentPart _mainDocumentPart;
        private readonly TextStyle _textStyle;

        private StyleAccessor(
            MainDocumentPart mainDocumentPart,
            TextStyle textStyle,
            ParagraphStyle paragraphStyle)
        {
            _mainDocumentPart = mainDocumentPart;
            _textStyle = textStyle;
            ParagraphStyle = paragraphStyle;
        }

        public ParagraphStyle ParagraphStyle { get; }

        public static StyleAccessor Default(MainDocumentPart mainDocumentPart)
        {
            var docDefaults = mainDocumentPart.StyleDefinitionsPart.Styles.DocDefaults;
            var paragraph = ParagraphStyle.From(docDefaults.ParagraphPropertiesDefault.ParagraphPropertiesBaseStyle);
            var text = TextStyle.From(docDefaults.RunPropertiesDefault.RunPropertiesBaseStyle);

            return new StyleAccessor(mainDocumentPart, text, paragraph);
        }

        public IStyleAccessor ForParagraph(ParagraphProperties paragraphProperties)
        {
            var ps = this.EffectiveStyle(paragraphProperties);
            var ts = this.TextStyleFromParagraph(paragraphProperties);

            return new StyleAccessor(_mainDocumentPart, ts, ps);
        }

        public IStyleAccessor ForTable(TableProperties tableProperties)
        {
            var paragraphStyles = this.GetParagraphStyles(tableProperties?.TableStyle?.Val).ToArray();
            var runStyles = this.GetRunStyles(tableProperties?.TableStyle?.Val).ToArray();

            var ps = this.ParagraphStyle.Override(null, paragraphStyles);
            var ts = _textStyle.Override(null, runStyles);
            return new StyleAccessor(_mainDocumentPart, ts, ps);
        }

        public ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties)
        {
            var styles = this.GetParagraphStyles(paragraphProperties?.ParagraphStyleId?.Val)
                .ToArray();

            return ParagraphStyle.Override(paragraphProperties, styles);
        }

        public TextStyle EffectiveStyle(RunProperties runProperties)
        {
            var styleRuns = this.GetRunStyles(runProperties?.RunStyle?.Val).ToArray();
            return _textStyle.Override(runProperties, styleRuns);
        }

        private TextStyle TextStyleFromParagraph(ParagraphProperties paragraphProperties)
        {
            var styles = this.GetRunStyles(paragraphProperties)
                .ToArray();

            var ts = _textStyle.Override(null, styles);
            return ts;
        }

        private IEnumerable<StyleParagraphProperties> GetParagraphStyles(StringValue firstStyleId)
        {
            if(string.IsNullOrWhiteSpace(firstStyleId?.Value))
            {
                yield break;
            }

            var styleId = firstStyleId;
            do
            {
                var style = this.FindStyle(styleId);
                if (style.StyleParagraphProperties != null)
                {
                    yield return style.StyleParagraphProperties;
                }

                styleId = style.BasedOn?.Val;
            } while (styleId != null);
        }

        private IEnumerable<StyleRunProperties> GetRunStyles(ParagraphProperties paragraphProperties)
        {
            if (paragraphProperties?.ParagraphStyleId?.Val == null)
            {
                yield break;
            }

            var styleId = paragraphProperties.ParagraphStyleId.Val;
            do
            {
                var style = this.FindStyle(styleId);
                if (style.StyleRunProperties != null)
                {
                    yield return style.StyleRunProperties;
                }

                styleId = style.BasedOn?.Val;
            } while (styleId != null);
        }

        private IEnumerable<StyleRunProperties> GetRunStyles(StringValue firstStyleId)
        {
            if (string.IsNullOrWhiteSpace(firstStyleId?.Value))
            {
                yield break;
            }

            var styleId = firstStyleId;
            do
            {
                var style = this.FindStyle(styleId);
                if (style.StyleRunProperties != null)
                {
                    yield return style.StyleRunProperties;
                }

                styleId = style.BasedOn?.Val;
            } while (styleId != null);
        }

        private Style FindStyle(StringValue styleId)
        {
            var style = _mainDocumentPart
                .StyleDefinitionsPart
                .Styles
                .OfType<Style>()
                .SingleOrDefault(s => s.StyleId == styleId);

            return style;
        }
    }
}
