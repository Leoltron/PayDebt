using System;
using System.Collections.Generic;
using System.Linq;

namespace PayDebt
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<string> GetAllStartsWith(
                this IEnumerable<string> enumerable,
                string prefix,
                StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return enumerable.Where(s => s.StartsWith(prefix, comparison));
        }


        public static IEnumerable<string> GetAllNonEmpty(
            this IEnumerable<string> enumerable)
        {
            return enumerable.Where(s => !string.IsNullOrWhiteSpace(s));
        }

    }
}