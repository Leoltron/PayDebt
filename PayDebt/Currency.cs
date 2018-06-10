using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace PayDebt
{
    public class Currency : ValueType<Currency>, IFormatProvider
    {
        public string Name { get; }
        public CultureInfo Culture { get; }

        public IReadOnlyList<Currency> Currencies = PayDebt.Currencies.CurrencyTypes;

        public Currency(string name, CultureInfo culture)
        {
            Name = name;
            Culture = culture;
        }

        public object GetFormat(Type formatType)
        {
            return Culture.GetFormat(formatType);
        }
    }
}