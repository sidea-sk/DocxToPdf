using System.IO;

namespace Sidea.DocxToPdf.Core
{
    internal interface IImageAccessor
    {
        Stream GetImageStream(string imageId);
    }
}
