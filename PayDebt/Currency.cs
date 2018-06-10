using System;
using System.Globalization;

namespace PayDebt
{
    public class Currency : IFormatProvider
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

        public bool Equals(Currency other)
        {
            return string.Equals(Name, other.Name) && Equals(Culture, other.Culture);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            return obj is Currency currency && Equals(currency);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Culture != null ? Culture.GetHashCode() : 0);
            }
        }
    }
}