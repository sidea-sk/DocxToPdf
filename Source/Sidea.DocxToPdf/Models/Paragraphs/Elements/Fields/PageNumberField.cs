using Sidea.DocxToPdf.Core;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Paragraphs.Elements.Fields
{
    internal class PageNumberField : Field
    {
        public PageNumberField(TextStyle textStyle) : base(textStyle)
        {
        }

        private PageVariables _variables = PageVariables.Empty;

        protected override string GetContent()
            => ((int)_variables.PageNumber).ToString();

        protected override void UpdateCore(PageVariables variables)
            => _variables = variables;
    }
}
