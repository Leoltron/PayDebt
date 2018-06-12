using System.Collections.Generic;

namespace DebtModel
{
    public interface IContactProvider<TContact> where TContact : Contact
    {
        bool TryGetContacts(out IEnumerable<TContact> contacts);
    }
}