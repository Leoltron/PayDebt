using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace PayDebt
{
    public class Currency : IFormatProvider
    {
        [Currency]
        public static readonly Currency AmericanDollars =
            new Currency("USD", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));

        [Currency]
        public static readonly Currency EurosFrenchCulture =
            new Currency("EUR", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR"));

        [Currency]
        public static readonly Currency RussianRoubles =
            new Currency("RUB", CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"));

        public static readonly IReadOnlyList<Currency> Currencies;

        static Currency()
        {
            Currencies = typeof(Currency)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.GetCustomAttributes(false).OfType<CurrencyAttribute>().Any())
                .Select(x => x.GetValue(x))
                .OfType<Currency>()
                .ToList();
        }

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