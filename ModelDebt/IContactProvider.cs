using System.Collections.Generic;
using System.Threading.Tasks;

namespace DebtModel
{
    public interface IContactProvider<TContact> where TContact : Contact
    {
        Task<IEnumerable<TContact>> GetContactsAsync();
    }
}