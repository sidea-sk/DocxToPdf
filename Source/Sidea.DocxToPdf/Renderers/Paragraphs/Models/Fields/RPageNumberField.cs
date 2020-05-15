using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Styles;

namespace Sidea.DocxToPdf.Renderers.Paragraphs.Models.Fields
{
    internal class RPageNumberField : RField
    {
        private readonly TextStyle _style;

        public RPageNumberField(TextStyle style)
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
