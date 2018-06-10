using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace PayDebt
{
    public class StaticStorage<TValue, TStorage>
    {
        public static readonly IEnumerable<TValue> All;

        static StaticStorage()
        {
            All = typeof(TStorage)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => x.GetValue(x))
                .Where(value => value.GetType().IsEqualOrSubclassOf(typeof(TValue)))
                .OfType<TValue>()
                .ToList();
        }
        
        protected StaticStorage()
        {
            throw new MethodAccessException("Static storage's constructor must not be called");
        }
    }
}