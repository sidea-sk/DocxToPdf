using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models
{
    internal class RParagraph : RLineElement
    {
        private const string _paragraph = "¶";

        public override bool OmitableAtLineBegin => false;

        public override bool OmitableAtLineEnd => false;

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            return prerenderArea.MeasureText(_paragraph, prerenderArea.AreaFont);
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            if (renderArea.Options.RenderHiddenChars)
            {
                var rect = new XRect(new XPoint(0, renderArea.Height - this.PrecalulatedSize.Height), this.PrecalulatedSize);
                renderArea.DrawText("¶", renderArea.AreaFont, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            return RenderResult.DoneEmpty;
        }
    }
}
