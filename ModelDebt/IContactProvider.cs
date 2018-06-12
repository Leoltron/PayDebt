using System.Collections.Generic;

namespace DebtModel
{
    public interface IContactProvider<out TContact> where TContact : Contact
    {
        IEnumerable<TContact> GetContacts();
    }
}