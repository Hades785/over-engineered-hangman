using System.Collections.Generic;
using System.Linq;

namespace utilities
{
    // https://stackoverflow.com/a/39997157
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T item, ushort index)> WithIndex<T>(this IEnumerable<T> self) =>
            self.Select((item, index) => (item, (ushort) index));
    }
}