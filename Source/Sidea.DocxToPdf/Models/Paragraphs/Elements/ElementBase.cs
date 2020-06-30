using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class ElementBase : IPageRenderable
    {
        public DocumentPosition Position { get; private set; } = DocumentPosition.None;

        public Size Size { get; protected set; } = Size.Zero;

        public double Width => this.Size.Width;

        public double Height => this.Size.Height;

        public Rectangle PageRegion => new Rectangle(this.Position.Offset, this.Size);

        public virtual void SetPosition(DocumentPosition position)
        {
            this.Position = position;
        }

        public abstract void Render(IRendererPage page);

        protected void RenderBorderIf(IRendererPage page, bool condition)
        {
            if (!condition)
            {
                return;
            }

            var region = this.PageRegion;
            var color = System.Drawing.Color.Orange;
            page.RenderLine(region.TopLine(color));
            page.RenderLine(region.RightLine(color));
            page.RenderLine(region.BottomLine(color));
            page.RenderLine(region.LeftLine(color));
        }
    }
}
