using System;
using System.Collections.Generic;
using System.Linq;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt.Model
{
    [Serializable]
    public class VkContactProvider : IContactProvider<VkContact>
    {
        public bool TryGetContacts(out IEnumerable<VkContact> contacts)
        {
            try
            {
                var task = VkFriends.GetVKFriends();
                task.Wait();
                contacts = task.Result.Select(p => new VkContact(p.Item2, p.Item1));
                return true;
            }
            catch (SystemException)
            {
                contacts = null;
                return false;
            }
        }
    }
}