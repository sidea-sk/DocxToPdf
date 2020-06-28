namespace Sidea.DocxToPdf.Models.Sections
{
    internal class SectionColumnConfig
    {
        public SectionColumnConfig(double width, double space)
        {
            this.Width = width;
            this.Space = space;
        }

        public double Width { get; }
        public double Space { get; }
    }
}
