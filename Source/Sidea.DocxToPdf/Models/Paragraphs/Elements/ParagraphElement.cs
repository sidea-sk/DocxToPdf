using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal abstract class ParagraphElement
    {
        public Rectangle BoundingBox { get; set; } = Rectangle.Empty;

        public abstract void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset);

        public abstract double GetBaseLineOffset();
    }
}
