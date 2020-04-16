using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal abstract class RendererBase : IRenderer
    {
        public XSize TotalArea { get; private set; } = new XSize(0, 0);

        public RenderingState CurrentRenderingState { get; private set; } = RenderingState.Unprepared;

        public XSize CalculateContentSize(IPrerenderArea prerenderArea)
        {
            this.TotalArea = this.CalculateContentSizeCore(prerenderArea);
            this.CurrentRenderingState = RenderingState.NotStarted;
            return this.TotalArea;
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            if(this.CurrentRenderingState.Status == RenderingStatus.Unprepared)
            {
                throw new RendererException("Cannot render content - renderer is not prepared!");
            }

            this.CurrentRenderingState = this.RenderCore(renderArea);
            return this.CurrentRenderingState;
        }

        protected abstract XSize CalculateContentSizeCore(IPrerenderArea prerenderArea);

        protected abstract RenderingState RenderCore(IRenderArea renderArea);
    }
}
