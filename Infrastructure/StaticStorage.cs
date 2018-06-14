using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure
{
    public class StaticStorage<TValue, TStorage>
    {
        public static IReadOnlyList<TValue> All => all;
        private static readonly List<TValue> all;

        static StaticStorage()
        {
            all = typeof(TStorage)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(x => x.GetValue(x))
                .Where(value => value.GetType().IsEqualOrSubclassOf(typeof(TValue)))
                .OfType<TValue>()
                .ToList();
        }

        public static void Add(TValue value) => all.Add(value);
        
        protected StaticStorage()
        {
            throw new MethodAccessException("Static storage's constructor must not be called");
        }
    }
}