using DocumentFormat.OpenXml;
using Sidea.DocxToPdf.Models.Styles;
using Word = DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models
{
    internal static class Factory
    {
        public static ContainerElement Create(OpenXmlCompositeElement openXmlComposite, IStyleFactory styleFactory)
        {
            return openXmlComposite switch
            {
                //Word.Paragraph p => new Paragraphs.Paragraph(p, styleFactory),
                //Word.Table t => new Tables.Table(t, styleFactory),
                _ => throw new RendererException("Unhandled element")
            };
        }
    }
}
