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
        private readonly ParagraphStyle _paragraphStyle;

        private StyleAccessor(
            MainDocumentPart mainDocumentPart,
            TextStyle textStyle,
            ParagraphStyle paragraphStyle)
        {
            _mainDocumentPart = mainDocumentPart;
            _textStyle = textStyle;
            _paragraphStyle = paragraphStyle;
        }

        public static StyleAccessor Default(MainDocumentPart mainDocumentPart)
        {
            var docDefaults = mainDocumentPart.StyleDefinitionsPart.Styles.DocDefaults;
            var paragraph = ParagraphStyle.From(docDefaults.ParagraphPropertiesDefault.ParagraphPropertiesBaseStyle);

            return new StyleAccessor(mainDocumentPart, TextStyle.Default, paragraph);
        }

        public ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties)
        {
            var styles = this.GetParagraphStyles(paragraphProperties)
                .ToArray();

            return _paragraphStyle.Override(paragraphProperties, styles);
        }

        public TextStyle EffectiveStyle(RunProperties runProperties)
        {
            return _textStyle;
        }

        private IEnumerable<StyleParagraphProperties> GetParagraphStyles(ParagraphProperties paragraphProperties)
        {
            if(paragraphProperties?.ParagraphStyleId?.Val == null)
            {
                yield break;
            }

            var styleId = paragraphProperties.ParagraphStyleId.Val;
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
