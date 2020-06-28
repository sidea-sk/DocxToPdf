using System;

namespace Sidea.DocxToPdf.Models.Paragraphs
{
    internal static class Tools
    {
        public static double CalculateSpaceBetween(ContainerElement element, ContainerElement adjacentElement)
        {
            if(adjacentElement == null)
            {
                return 0;
            }

            var spaceAfter = element.SpaceAfter();
            var spaceBefore = adjacentElement.SpaceBefore();

            return Math.Max(spaceAfter, spaceBefore);
        }

        private static double SpaceAfter(this ContainerElement elementBase)
        {
            return (elementBase as Paragraph)?.SpaceAfter ?? 0;
        }

        private static double SpaceBefore(this ContainerElement elementBase)
        {
            return (elementBase as Paragraph)?.SpaceBefore ?? 0;
        }
    }
}
