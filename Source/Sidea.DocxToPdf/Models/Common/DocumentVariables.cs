﻿namespace Sidea.DocxToPdf.Models.Common
{
    internal class DocumentVariables
    {
        public DocumentVariables(int totalPages)
        {
            this.TotalPages = totalPages;
        }

        public int TotalPages { get; }
    }
}
