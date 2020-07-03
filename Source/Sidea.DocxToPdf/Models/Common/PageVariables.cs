﻿using Sidea.DocxToPdf.Core;

namespace Sidea.DocxToPdf.Models.Common
{
    internal class PageVariables : DocumentVariables
    {
        public static PageVariables Empty = new PageVariables(PageNumber.None, 0);

        public PageVariables(PageNumber pageNumber, int totalPages) : base(totalPages)
        {
            this.PageNumber = pageNumber;
        }

        public PageNumber PageNumber { get; }
    }
}
