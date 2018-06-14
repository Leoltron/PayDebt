using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Provider;
using DebtModel;

namespace PayDebt.Model
{
    [Serializable]
    public class PhoneContactProvider : IContactProvider<PhoneContact>
    {
        public async Task<IEnumerable<PhoneContact>> GetContactsAsync()
        {
            return await Task.Run(() => GetContacts());
        }

        private static IEnumerable<PhoneContact> GetContacts()
        {
            var phoneContacts = new List<PhoneContact>();
            using (var phones =
                Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri,
                    null, null, null, null))
            {
                if (phones == null) return phoneContacts;
                while (phones.MoveToNext())
                {
                    var name = phones.GetString(
                        phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                    var phoneNumber =
                        phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));
                    phoneContacts.Add(new PhoneContact(name, phoneNumber));
                }

                phones.Close();
            }

            return phoneContacts;
        }
    }
}