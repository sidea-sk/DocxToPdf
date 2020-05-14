using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class CompositeRenderer : RendererBase
    {
        private readonly RendererFactory _factory = new RendererFactory();
        private readonly List<IRenderer> _renderers = new List<IRenderer>();
        private readonly IEnumerable<OpenXmlCompositeElement> _childElements;
        private readonly IStyleAccessor _styleAccessor;

        public CompositeRenderer(
            OpenXmlCompositeElement openXmlComposite,
            IStyleAccessor styleAccessor): this(openXmlComposite.RenderableChildren(), styleAccessor)
        {
        }

        public CompositeRenderer(IEnumerable<OpenXmlCompositeElement> childElements, IStyleAccessor styleAccessor)
        {
            _childElements = childElements;
            _styleAccessor = styleAccessor;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return this.CalculateChildrenSize(prerenderArea);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            return this.RenderChildren(renderArea);
        }

        protected XSize CalculateChildrenSize(IPrerenderArea prerenderArea)
        {
            // TODO: padding
            // TODO: margin
            var size = new XSize(prerenderArea.Width, 0);
            foreach (var child in _childElements)
            {
                var renderer = _factory.CreateRenderer(child, _styleAccessor);
                _renderers.Add(renderer);
                renderer.CalculateContentSize(prerenderArea);
                size = size.ExpandHeight(renderer.PrecalulatedSize.Height);
            }

            return size;
        }

        protected RenderResult RenderChildren(IRenderArea renderArea)
        {
            var renderedHeight = XUnit.Zero;
            var status = RenderingStatus.Done;
            var currentRenderArea = renderArea;

            foreach (var renderer in _renderers.Where(r => r.RenderResult.Status != RenderingStatus.Done))
            {
                renderer.Render(currentRenderArea);
                status = renderer.RenderResult.Status;
                renderedHeight += renderer.RenderResult.RenderedHeight;

                if (status == RenderingStatus.ReachedEndOfArea)
                {
                    break;
                }

                currentRenderArea = currentRenderArea.PanDown(renderer.RenderResult.RenderedHeight);
            }

            return RenderResult.FromStatus(status, renderArea.Width, renderedHeight);
        }
    }
}
