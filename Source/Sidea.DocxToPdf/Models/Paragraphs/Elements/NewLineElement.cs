using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal class NewLineElement : TextElement
    {
        public NewLineElement(TextStyle textStyle) : base(string.Empty, "↵", textStyle)
        {
        }
    }
}
