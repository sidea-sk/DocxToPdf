using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Fields
{
    internal class RPageNumberField : RField
    {
        private readonly RStyle _style;

        public RPageNumberField(RStyle style)
        {
            _style = style;
        }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var size = prerenderArea.MeasureText("000", _style.Font);
            return size;
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var rect = new XRect(new XPoint(0, renderArea.Height - this.PrecalulatedSize.Height), this.PrecalulatedSize);
            var pageNumber = renderArea.Context.CurrentPage;
            renderArea.DrawText(pageNumber.ToString(), _style.Font, _style.Brush, rect, XStringFormats.TopLeft);
            return RenderResult.Done(this.PrecalulatedSize);
        }
    }
}
