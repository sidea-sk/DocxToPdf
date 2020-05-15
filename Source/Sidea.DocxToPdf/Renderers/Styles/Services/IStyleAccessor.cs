using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Styles
{
    internal interface IStyleAccessor
    {
        // IStyleAccessor ForTable();
        // IStyleAccessor ForTableCell();
        // IStyleAccessor ForSection();

        ParagraphStyle ParagraphStyle { get; }

        IStyleAccessor ForParagraph(ParagraphProperties paragraphProperties);

        ParagraphStyle EffectiveStyle(ParagraphProperties paragraphProperties);
        TextStyle EffectiveStyle(RunProperties runProperties);
    }
}
