using System.Linq;
using DebtModel;
using Infrastructure;

namespace PayDebt.Model
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContactPickers : StaticStorage<IContactPicker<Contact>, ContactPickers>
    {
        public static bool HasAnyConnected => All.Any(p => p.IsLoggedIn);
        public static bool HasAnyNotConnected => All.Any(p => !p.IsLoggedIn);
    }
}