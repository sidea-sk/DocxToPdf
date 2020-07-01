using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class GridRow
    {
        public GridRow(double height, HeightRuleValues heightRule)
        {
            this.Height = height;
            this.HeightRule = heightRule;
        }

        public double Height { get; }
        public HeightRuleValues HeightRule { get; }
    }
}
