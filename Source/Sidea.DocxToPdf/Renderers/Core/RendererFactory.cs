using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Bodies;
using Sidea.DocxToPdf.Renderers.Paragraphs;
using Sidea.DocxToPdf.Renderers.Tables;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RendererFactory
    {
        public IRenderer CreateRenderer(OpenXmlCompositeElement forElement, RenderingOptions renderingOptions)
        {
            return forElement switch
            {
                SdtBlock b => new BlockRenderer(b, renderingOptions),
                Paragraph p => new ParagraphRenderer(p),
                Table t => new TableRenderer(t, renderingOptions),
                // Header h => new UnknownElementRenderer(),
                // Footer f => new UnknownElementRenderer(),
                // Drawing d => new UnknownElementRenderer(),
                _ => new UnknownElementRenderer()
            };
        }
    }
}
