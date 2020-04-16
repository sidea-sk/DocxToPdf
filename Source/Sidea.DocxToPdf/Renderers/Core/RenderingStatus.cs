namespace Sidea.DocxToPdf.Renderers.Core
{
    internal enum RenderingStatus
    {
        Unprepared,
        NotStarted,
        ReachedEndOfArea, //new page is necessary
        Done,
        Error
    }

    internal static class RenderingStatusExtensions
    {
        public static bool IsNotFinished(this RenderingStatus status) => !status.IsFinished();
        public static bool IsFinished(this RenderingStatus status) => status == RenderingStatus.Done || status == RenderingStatus.Error;
    }
}
