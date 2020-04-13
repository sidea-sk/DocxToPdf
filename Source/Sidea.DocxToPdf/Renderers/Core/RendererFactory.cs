using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Paragraphs;
using Sidea.DocxToPdf.Renderers.Tables;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RendererFactory
    {
        public IRenderer CreateRenderer(OpenXmlCompositeElement forElement)
        {
            return forElement switch
            {
                Table t => new TableRenderer(t),
                //TableRow r => new UnknownElementRenderer(),
                //TableCell c => new UnknownElementRenderer(),
                Header h => new UnknownElementRenderer(),
                Footer f => new UnknownElementRenderer(),
                Paragraph p => new ParagraphRenderer(p),
                Drawing d => new UnknownElementRenderer(),
                _ => new UnknownElementRenderer()
            };
        }
    }
}
