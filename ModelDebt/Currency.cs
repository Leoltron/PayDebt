using System;
using System.Globalization;
using Infrastructure;

namespace DebtModel
{
    public class Currency : ValueType<Currency>, IFormatProvider
    {
        public string Name { get; }
        public CultureInfo Culture { get; }

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