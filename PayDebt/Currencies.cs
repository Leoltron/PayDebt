using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace PayDebt
{
    public class Currencies : StaticStorage<Currency, Currencies>
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

        public static readonly IReadOnlyList<Currency> CurrencyTypes = StaticFieldValues;
    }
}