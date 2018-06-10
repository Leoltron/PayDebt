using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace PayDebt
{
    public static class Currencies
    {
        public static readonly Currency AmericanDollars =
            new Currency("USD", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
        public static readonly Currency EurosFrenchCulture =
            new Currency("EUR", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR"));
        public static readonly Currency RussianRoubles =
            new Currency("RUB", CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"));

        public static IEnumerable<Currency> All { get; private set; }

        static Currencies()
        {
            All = typeof(Currency)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(f => f.GetType().IsEqualOrSubclassOf(typeof(Currency)))
                .Select(x => x.GetValue(x))
                .OfType<Currency>()
                .ToList();
        }
    }
}