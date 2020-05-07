using PdfSharp;
using Sidea.DocxToPdf.Renderers.Common;

namespace Sidea.DocxToPdf.Renderers.Sections.Models
{
    public class SectionProperties
    {
        public SectionProperties(
            Margin margins,
            PageOrientation pageOrientation,
            RenderBehaviour renderBehaviour)
        {
            this.Margin = margins;
            this.PageOrientation = pageOrientation;
            this.RenderBehaviour = renderBehaviour;
        }

        public Margin Margin { get; }
        public PageOrientation PageOrientation { get; }
        public RenderBehaviour RenderBehaviour { get; }
    }
}
