using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PayDebt
{
    public class StaticStorage<TValue, TStorage>
    {
        protected static readonly IReadOnlyList<TValue> StaticFieldValues;

        static StaticStorage()
        {
            StaticFieldValues = typeof(TStorage)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => x.GetValue(x))
                .OfType<TValue>()
                .ToList();
        }
    }
}