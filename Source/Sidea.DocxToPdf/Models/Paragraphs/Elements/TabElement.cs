using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class TabElement : TextElement
    {
        public TabElement(TextStyle textStyle) : base("    ", "····", textStyle)
        {
        }
    }
}
