using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Bodies
{
    internal class BlockRenderer : CompositeRenderer
    {
        public BlockRenderer(SdtBlock block, IStyleAccessor styleAccessor) : base(block.SdtContentBlock, styleAccessor)
        {
        }
    }
}
