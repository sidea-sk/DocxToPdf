using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class SpaceElement : TextElement
    {
        public void Stretch()
        {
        }

        public SpaceElement(TextStyle textStyle) : base(" ", "·", textStyle)
        {
        }
    }
}
