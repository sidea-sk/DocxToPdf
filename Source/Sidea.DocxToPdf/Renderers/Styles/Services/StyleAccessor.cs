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
            return new StyleAccessor(mainDocumentPart, TextStyle.Default, ParagraphStyle.Default);
        }

        public ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties)
        {
            return _paragraphStyle;
        }

        public TextStyle EffectiveStyle(RunProperties runProperties)
        {
            return _textStyle;
        }

        //public ParagraphPropertiesBaseStyle GetParagraphProperties(string styleId)
        //{
        //    var pp = _mainDocumentPart
        //        .StyleDefinitionsPart
        //        .Styles
        //        .DocDefaults
        //        .ParagraphPropertiesDefault
        //        .ParagraphPropertiesBaseStyle
        //        ;

        //    return pp;
        //}

        //public Style GetStyle(string styleId)
        //{
        //    var s = _mainDocumentPart
        //        .StyleDefinitionsPart
        //        .Styles
        //        .Cast<Style>()
        //        .SingleOrDefault(s => s.StyleId == styleId);

        //    throw new NotImplementedException();
        //}
    }
}
