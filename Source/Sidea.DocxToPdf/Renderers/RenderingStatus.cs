namespace Sidea.DocxToPdf.Renderers
{
    internal enum RenderingStatus
    {
        NotStarted,
        // Prerendered,
        ReachedEndOfArea, //new page is necessary
        Finished,
        Error
    }
}
