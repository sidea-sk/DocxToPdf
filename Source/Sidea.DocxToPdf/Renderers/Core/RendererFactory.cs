using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Bodies;
using Sidea.DocxToPdf.Renderers.Paragraphs;
using Sidea.DocxToPdf.Renderers.Styles;
using Sidea.DocxToPdf.Renderers.Tables;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RendererFactory
    {
        public IRenderer CreateRenderer(OpenXmlCompositeElement forElement, IStyleAccessor styleAccessor)
        {
            return forElement switch
            {
                SdtBlock b => new BlockRenderer(b, styleAccessor),
                Paragraph p => new ParagraphRenderer(p),
                Table t => new TableRenderer(t, styleAccessor),
                _ => new UnknownElementRenderer()
            };
        }
    }
}
