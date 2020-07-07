﻿using System;
using Sidea.DocxToPdf.Models.Common;

namespace Sidea.DocxToPdf.Models.Headers
{
    internal abstract class HeaderBase : PageElement
    {
        public HeaderBase(PageMargin pageMargin)
        {
            this.PageMargin = pageMargin;
        }

        protected PageMargin PageMargin { get; }

        public double TopY => this.LastPageRegion.Region.Y;
        public double BottomY => Math.Max(this.LastPageRegion.Region.BottomY, this.PageMargin.Top);

        public abstract void Prepare(IPage page);
    }
}
