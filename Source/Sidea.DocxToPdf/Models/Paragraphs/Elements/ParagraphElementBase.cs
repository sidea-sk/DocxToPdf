using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class ParagraphElementBase : IPageRenderable
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
            var pen = new System.Drawing.Pen(System.Drawing.Color.Orange, 0.5f);

            page.RenderLine(region.TopLine(pen));
            page.RenderLine(region.RightLine(pen));
            page.RenderLine(region.BottomLine(pen));
            page.RenderLine(region.LeftLine(pen));
        }
    }
}
