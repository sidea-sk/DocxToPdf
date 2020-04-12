using DocumentFormat.OpenXml.Wordprocessing;
using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Paragraphs
{
    internal class ParagraphRenderer : IRenderer
    {
        private readonly Paragraph _paragraph;

        public ParagraphRenderer(Paragraph paragraph)
        {
            _paragraph = paragraph;
        }

        public RenderingState Render(IRenderArea renderArea)
        {
            return new RenderingState(RenderingStatus.Error, new XPoint(0, 0));
        }
    }
}
