using System;
using System.Collections.Generic;
using System.Linq;
using DebtModel;
using PayDebt.AndroidInfrastructure;

namespace PayDebt.Model
{
    [Serializable]
    public class VKContactProvider : IContactProvider<VKContact>
    {
        public bool TryGetContacts(out IEnumerable<VKContact> contacts)
        {
            try
            {
                contacts = VkFriends.GetVKFriends().Result.Select(p => new VKContact(p.Item2, p.Item1));
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