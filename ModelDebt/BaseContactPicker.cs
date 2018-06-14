using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtModel
{
    [Serializable]
    public class BaseContactPicker<TContact> : IBaseContactPicker<TContact> where TContact : Contact
    {
        private readonly IContactProvider<TContact> provider;
        private List<TContact> allContacts;
        private List<TContact> currentlyDisplayedContacts;
        public List<string> Names { get; }

        public IReadOnlyList<TContact> DisplayedContacts => currentlyDisplayedContacts;
        public TContact this[int x] => DisplayedContacts[x];

        public BaseContactPicker(IContactProvider<TContact> provider)
        {
            this.provider = provider;
            Names = new List<string>();
            allContacts = currentlyDisplayedContacts = new List<TContact>();
        }

        public async Task UpdateContactsAsync()
        {
            var contacts = await provider.GetContactsAsync();
            allContacts = currentlyDisplayedContacts = contacts.ToList();
        }

        public void FilterContacts(string prefix)
        {
            currentlyDisplayedContacts = allContacts
                .Where(c => c != null && c.Name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ToList();
            UpdateNames();
        }

        private void UpdateNames()
        {
            Names.Clear();
            Names.AddRange(currentlyDisplayedContacts.Select(x => x.Name));
        }
    }
}