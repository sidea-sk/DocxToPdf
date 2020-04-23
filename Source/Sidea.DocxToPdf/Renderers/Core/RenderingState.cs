using System;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers.Core
{
    internal class RenderingState
    {
        public static readonly RenderingState Unprepared = new RenderingState(RenderingStatus.Unprepared, new XSize(0, 0));
        public static readonly RenderingState Error = new RenderingState(RenderingStatus.Error, new XSize(0,0));
        public static readonly RenderingState NotStarted = new RenderingState(RenderingStatus.NotStarted, new XSize(0, 0));
        public static readonly RenderingState DoneEmpty = new RenderingState(RenderingStatus.Done, new XSize(0, 0));

        private RenderingState(
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

        public static RenderingState EndOfRenderArea(XUnit width, XUnit height)
            => FromStatus(RenderingStatus.ReachedEndOfArea, width, height);

        public static RenderingState EndOfRenderArea(XRect renderedArea)
            => FromStatus(RenderingStatus.ReachedEndOfArea, renderedArea.Size);

        public static RenderingState Done(double width, double height)
            => FromStatus(RenderingStatus.Done, width, height);

        public static RenderingState Done(XRect renderedArea)
            => FromStatus(RenderingStatus.Done, renderedArea.Size);

        public static RenderingState FromStatus(RenderingStatus status, double width, double height)
            => FromStatus(status, new XSize(width, height));

        public static RenderingState FromStatus(RenderingStatus status, XSize size)
            => new RenderingState(status, size);
    }
}
