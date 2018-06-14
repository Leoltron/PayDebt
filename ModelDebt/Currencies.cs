using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Infrastructure;

namespace DebtModel
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Currencies : StaticStorage<Currency, Currencies>
    {
    }
}