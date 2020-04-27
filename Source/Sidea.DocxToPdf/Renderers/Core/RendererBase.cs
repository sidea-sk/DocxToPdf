using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal abstract class RendererBase : IRenderer
    {
        public XSize PrecalulatedSize { get; private set; } = new XSize(0, 0);

        public XSize RenderedSize { get; private set; } = new XSize(0, 0);

        public RenderResult RenderResult { get; private set; } = RenderResult.Unprepared;

        public void CalculateContentSize(IPrerenderArea prerenderArea)
        {
            this.PrecalulatedSize = this.CalculateContentSizeCore(prerenderArea);
            this.RenderResult = RenderResult.NotStarted;
        }

        public void Render(IRenderArea renderArea)
        {
            if(this.RenderResult.Status == RenderingStatus.Unprepared)
            {
                throw new RendererException("Cannot render content - renderer is not prepared!");
            }

            this.RenderResult = this.RenderCore(renderArea);
            this.RenderedSize = this.RenderedSize
                .ExpandWidthIfBigger(this.RenderResult.RenderedSize.Width)
                .ExpandHeight(this.RenderResult.RenderedSize.Height);
        }

        protected abstract XSize CalculateContentSizeCore(IPrerenderArea prerenderArea);

        protected abstract RenderResult RenderCore(IRenderArea renderArea);
    }
}
