﻿using DocumentFormat.OpenXml.Wordprocessing;
using Sidea.DocxToPdf.Renderers.Core;

namespace Sidea.DocxToPdf.Renderers.Bodies
{
    internal class BlockRenderer : CompositeRenderer
    {
        public BlockRenderer(SdtBlock block, RenderingOptions renderingOptions) : base(block.SdtContentBlock, renderingOptions)
        {
        }
    }
}
