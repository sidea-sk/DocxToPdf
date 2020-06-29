namespace Sidea.DocxToPdf.Models.Common
{
    internal class Variables
    {
        public Variables(int totalPages)
        {
            this.TotalPages = totalPages;
        }

        public int TotalPages { get; }
    }
}
