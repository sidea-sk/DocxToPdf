﻿using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class PdfSharpExtensions
    {
        public static XUnit Sum(this IEnumerable<XUnit> values)
        {
            return values.Aggregate(new XUnit(0), (agg, v) => agg + v);
        }

        public static XSize Expand(this XSize size, XUnit width, XUnit height)
        {
            return new XSize(size.Width + width, size.Height + height);
        }

        public static XSize Expand(this XSize size, XSize expand)
            => size.Expand(expand.Width, expand.Height);

        public static XSize ExpandHeight(this XSize size, XUnit height)
            => size.Expand(0, height);

        public static XSize ExpandWidth(this XSize size, XUnit width)
         => size.Expand(width, 0);
    }
}
