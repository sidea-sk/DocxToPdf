using System.Collections.Generic;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class ArrayExtensions
    {
        public static IEnumerable<(T value, int index)> SelectWithIndeces<T>(this IEnumerable<T> source, params int[] indeces)
        {
            return source
                .Where((item, index) => indeces.Contains(index))
                .Select((item, index) => (item, index));
        }

        public static IEnumerable<T> SelectByIndeces<T>(this IEnumerable<T> source, params int[] indeces)
        {
            return source
                .Where((item, index) => indeces.Contains(index));
        }

        public static IEnumerable<T> Update<T>(this IEnumerable<T> source, params (T value, int index)[] values)
        {
            var copy = source.ToArray();
            foreach(var v in values)
            {
                copy[v.index] = v.value;
            }
            return copy;
        }
    }
}
