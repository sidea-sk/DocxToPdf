﻿using PdfSharp.Drawing;
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
            this.RenderedSize = UpdateRenderedSize(this.RenderedSize, this.RenderResult.RenderedSize);
        }

        protected abstract XSize CalculateContentSizeCore(IPrerenderArea prerenderArea);

        protected abstract RenderResult RenderCore(IRenderArea renderArea);

        private static XSize UpdateRenderedSize(XSize currentRenderedSize, XSize renderedSize)
        {
            if (renderedSize.Width > currentRenderedSize.Width)
            {
                currentRenderedSize = new XSize(renderedSize.Width, currentRenderedSize.Height);
            }

            return currentRenderedSize.ExpandHeight(renderedSize.Height);
        }
    }
}
