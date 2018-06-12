using System;
using System.Collections.Generic;
using DebtModel;
using PayDebt.AndroidInfrastructure;
using VKontakte.API;

namespace PayDebt.Model
{
    public class VKContactProvider : IContactProvider<VKContact>
    {
        public bool TryGetContacts(out IEnumerable<VKContact> contacts)
        {
            contacts = new[] { new VKContact("Айдар Исламов", "lowgear1000")};
            return true;
        }
    }
}