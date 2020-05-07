using PdfSharp.Drawing;
using Sidea.DocxToPdf.Renderers.Core;
using Sidea.DocxToPdf.Renderers.Core.RenderingAreas;
using Sidea.DocxToPdf.Renderers.Sections.Models;

namespace Sidea.DocxToPdf.Renderers.Sections
{
    internal class SectionRenderer : CompositeRenderer
    {
        public SectionRenderer(SectionData sectionData): base(sectionData.Elements)
        {
            this.SectionProperties = sectionData.Properties;
        }

        public SectionProperties SectionProperties { get; }

        protected override XSize CalculateContentSizeCore(IPrerenderArea prerenderArea)
        {
            var contentRenderArea = prerenderArea.Restrict(prerenderArea.Width - this.SectionProperties.Margin.HorizontalMargins);
            var size = base.CalculateContentSizeCore(contentRenderArea);
            return size;
        }

        protected override RenderResult RenderCore(IRenderArea renderArea)
        {
            var sectionRenderArea = renderArea
                .PanLeft(this.SectionProperties.Margin.Left)
                .Restrict(renderArea.Width - this.SectionProperties.Margin.HorizontalMargins);

            sectionRenderArea.DrawRectangle(XPens.Orange, XBrushes.Transparent, new XRect(sectionRenderArea.AreaRectangle.Size));

            var result = base.RenderCore(sectionRenderArea);
            return result;
        }
    }
}
