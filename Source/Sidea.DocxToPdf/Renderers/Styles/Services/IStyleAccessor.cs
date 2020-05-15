using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal interface IStyleAccessor
    {
        // IStyleAccessor ForTableCell();
        // IStyleAccessor ForSection();

        ParagraphStyle ParagraphStyle { get; }

        IStyleAccessor ForParagraph(ParagraphProperties paragraphProperties);
        IStyleAccessor ForTable(TableProperties tableProperties);

        ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties);
        TextStyle EffectiveStyle(RunProperties runProperties);
    }
}
