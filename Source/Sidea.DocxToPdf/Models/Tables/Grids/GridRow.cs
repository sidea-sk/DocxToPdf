using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class GridRow
    {
        public GridRow(double defaultHeight, HeightRuleValues heightRule)
        {
            this.Height = defaultHeight;
            this.HeightRule = heightRule;
        }

        public double Height { get; }
        public HeightRuleValues HeightRule { get; }

        public List<PageRegion> PageRegions { get; } = new List<PageRegion>();
    }
}
