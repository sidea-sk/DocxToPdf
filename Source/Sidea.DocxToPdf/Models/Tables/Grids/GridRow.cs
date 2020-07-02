using System;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Models.Tables.Elements
{
    internal class GridRow
    {
        public GridRow(double defaultHeight, HeightRuleValues heightRule)
        {
            this.MinimalHeight = defaultHeight;
            this.Height = defaultHeight;
            this.HeightRule = heightRule;
        }

        public double MinimalHeight { get; private set; }
        public double Height { get; private set; }

        public HeightRuleValues HeightRule { get; }

        public void Expand(double height)
        {
            this.Height = Math.Max(height, this.Height);
        }
    }
}
