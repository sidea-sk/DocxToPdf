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
            return _paragraphStyle;
        }

        public TextStyle EffectiveStyle(RunProperties runProperties)
        {
            return _textStyle;
        }
    }
}
