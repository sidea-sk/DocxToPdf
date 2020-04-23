using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Headers
{
    internal class HeaderRenderer : RendererBase
    {
        private readonly WordprocessingDocument _docx;
        private readonly RenderingOptions _renderingOptions;
        private readonly List<IRenderer> _renderers = new List<IRenderer>();

        private int _precalculatedPage = 0;
        private int _renderedPage = 0;

        public HeaderRenderer(WordprocessingDocument docx, RenderingOptions renderingOptions)
        {
            _docx = docx;
            _renderingOptions = renderingOptions;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return new XSize(prerenderArea.Width, XUnit.FromCentimeter(2.5));
        }

        protected override RenderingState RenderCore(IRenderArea renderArea)
        {
            return RenderingState.Done(renderArea.Width, XUnit.FromCentimeter(2.5));
        }
    }
}
