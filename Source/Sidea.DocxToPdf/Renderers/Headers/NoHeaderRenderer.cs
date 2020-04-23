using System;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class NoHeaderRenderer : RendererBase, IHeaderRenderer
    {
        private readonly XUnit _defaultMargin = XUnit.FromCentimeter(2.5);

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, _defaultMargin);
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            return RenderingState.Done(renderArea.Width, _defaultMargin);
        }
    }
}
