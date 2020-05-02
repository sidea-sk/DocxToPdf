using System.Collections.Generic;
using System.Linq;

namespace Sidea.DocxToPdf.Renderers
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<(T value, int index)> SelectWithIndeces<T>(this IEnumerable<T> source, params int[] indeces)
        {
            var temp = source.ToArray();
            return indeces.Select(i => (temp[i], i));
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

        public static Stack<T> ToStack<T>(this IEnumerable<T> source, bool reverseOrder = true)
        {
            var content = reverseOrder 
                ? source.Reverse()
                : source;

            return new Stack<T>(content);
        }
    }
}
