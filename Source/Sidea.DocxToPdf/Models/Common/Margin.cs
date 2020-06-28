namespace Sidea.DocxToPdf.Models.Common
{
    public class Margin
    {
        public Margin(double top, double right, double bottom, double left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        public double Top { get; }
        public double Right { get; }
        public double Bottom { get; }
        public double Left { get; }

        public double HorizontalMargins => this.Left + this.Right;
        public double VerticalMargins => this.Top + this.Bottom;
    }
}
