using System;

namespace Sidea.DocxToPdf
{
    public class RendererException : Exception
    {
        public RendererException(string message) : base(message)
        {
        }
    }
}
