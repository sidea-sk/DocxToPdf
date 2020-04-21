using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal abstract class RendererBase : IRenderer
    {
        public XSize PrecalulatedSize { get; private set; } = new XSize(0, 0);

        public XSize RenderedSize { get; private set; } = new XSize(0, 0);

        public RenderingState CurrentRenderingState { get; private set; } = RenderingState.Unprepared;

        public void CalculateContentSize(IPrerenderArea prerenderArea)
        {
            this.PrecalulatedSize = this.CalculateContentSizeCore(prerenderArea);
            this.CurrentRenderingState = RenderingState.NotStarted;
        }

        public void Render(IRenderArea renderArea)
        {
            if(this.CurrentRenderingState.Status == RenderingStatus.Unprepared)
            {
                throw new RendererException("Cannot render content - renderer is not prepared!");
            }

            this.CurrentRenderingState = this.RenderCore(renderArea);
            this.RenderedSize = UpdateRenderedSize(this.RenderedSize, this.CurrentRenderingState.RenderedArea);
        }

        protected abstract XSize CalculateContentSizeCore(IPrerenderArea prerenderArea);

        protected abstract RenderingState RenderCore(IRenderArea renderArea);

        private static XSize UpdateRenderedSize(XSize currentRenderedSize, XRect renderedArea)
        {
            if (renderedArea.Width > currentRenderedSize.Width)
            {
                currentRenderedSize = new XSize(renderedArea.Width, currentRenderedSize.Height);
            }

            return currentRenderedSize.ExpandHeight(renderedArea.Height);
        }
    }
}
