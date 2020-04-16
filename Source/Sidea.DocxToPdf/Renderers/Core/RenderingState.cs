using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RenderingState
    {
        public static readonly RenderingState Error = new RenderingState(RenderingStatus.Error, new XRect(0,0,0,0));
        public static readonly RenderingState NotStarted = new RenderingState(RenderingStatus.NotStarted, new XRect(0, 0, 0, 0));
        public static readonly RenderingState DoneEmpty = new RenderingState(RenderingStatus.Done, new XRect(0, 0, 0, 0));

        private RenderingState(
            RenderingStatus status,
            XRect renderedArea)
        {
            this.Status = status;
            this.RenderedArea = renderedArea;
        }

        public XRect RenderedArea { get; }

        public RenderingStatus Status { get; }

        public XPoint FinishedAtPosition => this.RenderedArea.BottomRight;

        public static RenderingState EndOfRenderArea(XUnit width, XUnit height)
            => FromStatus(RenderingStatus.ReachedEndOfArea, width, height);

        public static RenderingState EndOfRenderArea(XRect renderedArea)
            => FromStatus(RenderingStatus.ReachedEndOfArea, renderedArea);

        public static RenderingState Done(double width, double height)
            => FromStatus(RenderingStatus.Done, new XRect(new XSize(width, height)));

        public static RenderingState Done(XRect renderedArea)
            => FromStatus(RenderingStatus.Done, renderedArea);

        public static RenderingState FromStatus(RenderingStatus status, double width, double height)
            => FromStatus(status, new XRect(new XSize(width, height)));

        public static RenderingState FromStatus(RenderingStatus status, XRect rect)
            => new RenderingState(status, rect);
    }
}
