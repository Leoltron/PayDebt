using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt.Model
{
    [Serializable]
    public class VkContactProvider : IContactProvider<VkContact>
    {
        public Task<IEnumerable<VkContact>> GetContactsAsync()
        {
            return VkFriends
                .GetVKFriends()
                .ContinueWith(t => t.Result.Select(p => new VkContact(p.Item2, p.Item1)));
        }
    }
}