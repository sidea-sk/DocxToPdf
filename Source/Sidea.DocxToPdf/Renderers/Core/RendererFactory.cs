using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RendererFactory
    {
        public IRenderer CreateRenderer(OpenXmlCompositeElement forElement)
        {
            return forElement switch
            {
                Table t => new UnknownElementRenderer(),
                TableRow r => new UnknownElementRenderer(),
                TableCell c => new UnknownElementRenderer(),
                Header h => new UnknownElementRenderer(),
                Footer f => new UnknownElementRenderer(),
                Paragraph p => new UnknownElementRenderer(),
                Drawing d => new UnknownElementRenderer(),
                _ => new UnknownElementRenderer()
            };
        }
    }
}
