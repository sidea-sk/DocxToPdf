namespace Sidea.DocxToPdf.Renderers
{
    internal enum RenderingStatus
    {
        NotStarted,
        // Prerendered,
        ReachedEndOfArea, //new page is necessary
        Done,
        Error
    }

    internal static class RenderingStatusExtensions
    {
        public static bool IsFinished(this RenderingStatus status) => status == RenderingStatus.Done || status == RenderingStatus.Error;
    }
}
