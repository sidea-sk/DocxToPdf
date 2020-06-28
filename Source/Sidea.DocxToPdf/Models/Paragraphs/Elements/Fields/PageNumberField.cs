using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields
{
    internal class PageNumberField : Field
    {
        public PageNumberField(TextStyle textStyle) : base(textStyle)
        {
        }

        public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        {
        }

        //private DocumentVariables _variables;

        //public PageNumberField(PageVariables variables, TextStyle textStyle) : base(textStyle)
        //{
        //    _variables = variables;
        //}

        //protected override void UpdateCore(PageVariables variables)
        //{
        //    _variables = variables;
        //}

        protected override string GetContent()
            => string.Empty; // ((int)_variables.PageNumber).ToString();
    }
}
