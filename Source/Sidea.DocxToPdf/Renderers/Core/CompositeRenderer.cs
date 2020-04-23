﻿using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class CompositeRenderer : RendererBase
    {
        private readonly OpenXmlCompositeElement _openXmlComposite;
        private readonly RenderingOptions _renderingOptions;
        private readonly RendererFactory _factory = new RendererFactory();
        private readonly List<IRenderer> _renderers = new List<IRenderer>();

        public CompositeRenderer(OpenXmlCompositeElement openXmlComposite, RenderingOptions renderingOptions)
        {
            _openXmlComposite = openXmlComposite;
            _renderingOptions = renderingOptions;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return this.CalculateChildrenSize(prerenderArea);
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            return this.RenderChildren(renderArea);
        }

        protected XSize CalculateChildrenSize(IPrerenderArea prerenderArea)
        {
            // TODO: padding
            // TODO: margin
            var size = new XSize(prerenderArea.Width, 0);
            foreach (var child in _openXmlComposite.RenderableChildren())
            {
                var renderer = _factory.CreateRenderer(child, _renderingOptions);
                _renderers.Add(renderer);
                renderer.CalculateContentSize(prerenderArea);
                size = size.ExpandHeight(renderer.PrecalulatedSize.Height);
            }

            return size;
        }

        protected RenderingState RenderChildren(IRenderArea renderArea)
        {
            var renderedHeight = XUnit.Zero;
            var status = RenderingStatus.Done;
            var currentRenderArea = renderArea;

            foreach (var renderer in _renderers.Where(r => r.CurrentRenderingState.Status != RenderingStatus.Done))
            {
                renderer.Render(currentRenderArea);
                status = renderer.CurrentRenderingState.Status;
                renderedHeight += renderer.CurrentRenderingState.RenderedHeight;

                if (status == RenderingStatus.ReachedEndOfArea)
                {
                    break;
                }

                currentRenderArea = currentRenderArea.PanDown(renderer.CurrentRenderingState.RenderedHeight);
            }

            return RenderingState.FromStatus(status, renderArea.Width, renderedHeight);
        }
    }
}