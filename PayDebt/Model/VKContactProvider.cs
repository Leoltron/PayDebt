using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt.Model
{
    [Serializable]
    public class VkContactProvider : IContactProvider<VkContact>
    {
        public async Task<IEnumerable<VkContact>> GetContactsAsync()
        {
            var friends = await VkFriends.GetVKFriends();
            return friends.Select(p => p == null ? null : new VkContact(p.Item2, p.Item1));
        }
    }
}