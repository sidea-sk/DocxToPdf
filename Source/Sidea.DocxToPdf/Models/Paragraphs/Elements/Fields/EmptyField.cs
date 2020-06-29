using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields
{
    internal class EmptyField : Field
    {
        public EmptyField(TextStyle textStyle) : base(textStyle)
        {
        }

        protected override string GetContent() => string.Empty;

        //protected override void UpdateCore(PageVariables variables)
        //{
        //}
    }
}
