using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal interface IStyleAccessor
    {
        // IStyleAccessor ForTable();
        // IStyleAccessor ForTableCell();
        // IStyleAccessor ForSection();
        // IStyleAccessor ForParagraph();

        ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties);
        TextStyle EffectiveStyle(RunProperties runProperties);
    }
}
