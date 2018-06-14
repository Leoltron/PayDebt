using System.Diagnostics.CodeAnalysis;
using Infrastructure;

namespace DebtModel
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Currencies : StaticStorage<Currency, Currencies>
    {
    }
}