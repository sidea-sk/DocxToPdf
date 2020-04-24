using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RenderResult
    {
        public static readonly RenderResult Unprepared = new RenderResult(RenderingStatus.Unprepared, new XSize(0, 0));
        public static readonly RenderResult Error = new RenderResult(RenderingStatus.Error, new XSize(0,0));
        public static readonly RenderResult NotStarted = new RenderResult(RenderingStatus.NotStarted, new XSize(0, 0));
        public static readonly RenderResult DoneEmpty = new RenderResult(RenderingStatus.Done, new XSize(0, 0));

        private RenderResult(
            RenderingStatus status,
            XSize size)
        {
            this.Status = status;
            this.RenderedSize = size;
        }

        public RenderingStatus Status { get; }

        public XSize RenderedSize { get; }

        public XUnit RenderedWidth => this.RenderedSize.Width;

        public XUnit RenderedHeight => this.RenderedSize.Height;

        public static RenderResult EndOfRenderArea(XUnit width, XUnit height)
            => FromStatus(RenderingStatus.ReachedEndOfArea, width, height);

        public static RenderResult EndOfRenderArea(XRect renderedArea)
            => FromStatus(RenderingStatus.ReachedEndOfArea, renderedArea.Size);

        public static RenderResult Done(double width, double height)
            => FromStatus(RenderingStatus.Done, width, height);

        public static RenderResult Done(XRect renderedArea)
            => FromStatus(RenderingStatus.Done, renderedArea.Size);

        public static RenderResult FromStatus(RenderingStatus status, double width, double height)
            => FromStatus(status, new XSize(width, height));

        public static RenderResult FromStatus(RenderingStatus status, XSize size)
            => new RenderResult(status, size);
    }
}
