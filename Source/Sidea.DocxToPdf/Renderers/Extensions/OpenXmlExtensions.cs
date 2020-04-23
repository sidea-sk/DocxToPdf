﻿using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class OpenXmlExtensions
    {
        public static IEnumerable<OpenXmlCompositeElement> RenderableChildren(this OpenXmlElement xmlElement)
        {
            return xmlElement
                .ChildElements
                .Where(c => c is Paragraph || c is Table)
                .Cast<OpenXmlCompositeElement>();
        }

        public static IEnumerable<T> ChildsOfType<T>(this OpenXmlElement xmlElement)
            where T : OpenXmlElement
        {
            return xmlElement.ChildElements.OfType<T>();
        }

        public static bool IsOn(this OnOffType onOffType, bool ifNull = false)
        {
            return onOffType?.Val ?? false;
        }
    }
}
