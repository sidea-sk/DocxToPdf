using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class ElementBase
    {
        public DocumentPosition Position { get; private set; } = DocumentPosition.None;
        public Size Size { get; protected set; } = Size.Zero;

        public Rectangle PageRegion => new Rectangle(this.Position.Offset, this.Size);

        public virtual void SetPosition(DocumentPosition position)
        {
            this.Position = position;
        }
    }
}
