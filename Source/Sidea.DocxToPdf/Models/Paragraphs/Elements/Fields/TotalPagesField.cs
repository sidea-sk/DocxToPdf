using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields
{
    internal class TotalPagesField : Field
    {
        public TotalPagesField(TextStyle textStyle) : base(textStyle)
        {
        }

        public override void SetLineBoundingBox(Rectangle rectangle, double baseLineOffset)
        {
        }

        //private PageVariables _variables;

        //public TotalPagesField(PageVariables variables, TextStyle textStyle) : base(textStyle)
        //{
        //    _variables = variables;
        //}

        //protected override void UpdateCore(PageVariables variables)
        //{
        //    _variables = variables;
        //}

        protected override string GetContent()
            => string.Empty; // _variables.TotalPages.ToString();
    }
}
