using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Infrastructure;

namespace DebtModel
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Currencies : StaticStorage<Currency, Currencies>
    {
        public static readonly Currency AmericanDollars =
            new Currency("USD", CultureInfo.GetCultureInfoByIetfLanguageTag("en-US"));
        public static readonly Currency EurosFrenchCulture =
            new Currency("EUR", CultureInfo.GetCultureInfoByIetfLanguageTag("fr-FR"));
        public static readonly Currency RussianRoubles =
            new Currency("RUB", CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"));
    }
}