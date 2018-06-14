using System.Collections.Generic;
using System.Threading.Tasks;

namespace DebtModel
{
    public interface IBaseContactPicker<out TContact> where TContact : Contact
    {
        List<string> Names { get; }
        IReadOnlyList<TContact> DisplayedContacts { get; }
        TContact this[int x] { get; }
        Task UpdateContactsAsync();
        void FilterContacts(string prefix);
    }
}