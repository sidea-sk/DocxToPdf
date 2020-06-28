using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields
{
    internal class EmptyField : Field
    {
        public EmptyField(TextStyle textStyle) : base(textStyle)
        {
        }

        public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        {
            throw new System.NotImplementedException();
        }

        protected override string GetContent() => string.Empty;

        //protected override void UpdateCore(PageVariables variables)
        //{
        //}
    }
}
