using System.Linq;
using DebtModel;
using Infrastructure;

namespace PayDebt.Model
{
    public class ContactPickers : StaticStorage<IContactPicker<Contact>, ContactPickers>
    {
        public static bool HasAuthorized => All.Any(p => p.IsAuthorized);
        public static string[] Names => All.Select(x => x.Name).ToArray();
    }
}