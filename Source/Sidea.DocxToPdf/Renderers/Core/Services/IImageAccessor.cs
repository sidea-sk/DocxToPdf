using System.IO;

namespace Sidea.DocxToPdf.Renderers.Core.Services
{
    internal interface IImageAccessor
    {
        Stream GetImageStream(string imageId);
    }
}
