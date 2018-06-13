using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtModel
{
    [Serializable]
    public class ContactPicker<TContact> where TContact : Contact
    {
        private readonly IContactProvider<TContact> provider;   
        private List<TContact> allContacts;
        private List<TContact> currentlyDisplayedContacts;
        public List<string> Names;

        public IReadOnlyList<TContact> DisplayedContacts => currentlyDisplayedContacts;
        public TContact this[int x] => DisplayedContacts[x];

        public ContactPicker(IContactProvider<TContact> provider)
        {
            this.provider = provider;
            Names = new List<string>();
        }

        public bool UpdateContacts()
        {

            var result = provider.TryGetContacts(out var contacts);
            if (!result) return result;
            allContacts = currentlyDisplayedContacts = contacts.ToList();
            UpdateNames();
            return result;
        }

        public void FilterContacts(string prefix)
        {
            currentlyDisplayedContacts = allContacts.Where(c => c != null && c.Name.StartsWith(prefix)).ToList();
            UpdateNames();
        }

        private void UpdateNames()
        {
            Names.Clear();
            Names.AddRange(currentlyDisplayedContacts.Select(x => x.Name));
        }
    }
}